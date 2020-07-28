Param (
    [Parameter(Position = 0, Mandatory = $true)]
    [string]$tenant,
    [Parameter(Position = 1, Mandatory = $true)]
    [string]$subscription,
    [Parameter(Position = 2, Mandatory = $true)]
    [string]$resourceGroupName,
    [Parameter(Position = 3, Mandatory = $true)]
    [ValidateLength(1, 16)]
    [string]$webAppSuffix,
    [Parameter(Position = 4, Mandatory = $true)]
    [string]$sharePointUrl,
    [Parameter(Position = 5, Mandatory = $true)]
    [string]$certificatePassword,
    [Parameter(Position = 6)]
    [string]$location = "westeurope"
)

$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition;
$templateFilePath = "$scriptPath\templates\template.json";
$appPermissionsFilePath = "$scriptPath\templates\appPermissions.json";
$webAppName = "piwikpro" + $webAppSuffix;
$piwikAdminSiteUrl = "$sharePointUrl/sites/PiwikAdmin";

if (-not (Get-Command az -ErrorAction SilentlyContinue) -or [System.Version](az version --query '\"azure-cli\"' -o tsv) -lt [System.Version]"2.9.1") {
    Write-Host "Installing Azure CLI...";
    Invoke-WebRequest -Uri https://aka.ms/installazurecliwindows -OutFile .\AzureCLI.msi; Start-Process msiexec.exe -Wait -ArgumentList '/I AzureCLI.msi /quiet'; rm .\AzureCLI.msi;
    Write-Host "Azure CLI installed.";
    $Env:Path = "${Env:ProgramFiles(x86)}\Microsoft SDKs\Azure\CLI2\wbin;${Env:Path}";
}

az login --tenant "$tenant" -o none;
az account set -s "$subscription" -o none;

$cert = New-SelfSignedCertificate -CertStoreLocation "cert:\CurrentUser\My" -Subject "CN=PiwikPROCertificate" -NotAfter (Get-Date).AddYears(6);
$keyValue = [System.Convert]::ToBase64String($cert.GetRawCertData());
$secPassword = ConvertTo-SecureString -String "$certificatePassword" -AsPlainText -Force;
$cert | Export-PfxCertificate -FilePath "$($scriptPath)\PiwikPROCertificate.pfx" -Password $secPassword;

$clientId = az ad app list --display-name "$webAppName" --query '[].appId' -o tsv;
if ($clientId) {
    az ad app update --display-name "$webAppName" --key-value "$keyValue" --start-date "$($cert.NotBefore.ToString('o'))" --end-date "$($cert.NotAfter.ToString('o'))" --required-resource-accesses "@$appPermissionsFilePath" -o none;
}
else {
    $clientId = az ad app create --display-name "$webAppName" --key-value "$keyValue" --start-date "$($cert.NotBefore.ToString('o'))" --end-date "$($cert.NotAfter.ToString('o'))" --required-resource-accesses "@$appPermissionsFilePath" --query 'appId' -o tsv;
}

if (-not (az ad sp list --filter "appId eq '$clientId'" --query '[].appId' -o tsv)) {
    az ad sp create --id "$clientId" -o none;
}

az ad app permission admin-consent --id "$clientId" -o none;

if (-not [System.Convert]::ToBoolean((az group exists -n "$resourceGroupName"))) {
    az group create -l "$location" -n "$resourceGroupName" -o none;
}

az deployment group create -g "$resourceGroupName" --template-file "$templateFilePath" --parameters sites_piwikpro_name="$webAppName" components_piwikpro_name="$webAppName" serverfarms_piwikproPlan_name="$webAppName" storageAccounts_piwikprostorage_name="$webAppName" piwikAdminSiteUrl="$piwikAdminSiteUrl" certificateThumbprint="$($cert.Thumbprint)" tenant="$tenant" clientId="$clientId" -o none;
az webapp config ssl upload --certificate-file "$scriptPath\PiwikPROCertificate.pfx" --certificate-password "$certificatePassword" --name "$webAppName" --resource-group "$resourceGroupName" -o none;

.\Deploy-AzureWebJob -resourceGroupName $resourceGroupName -webAppName $webAppName -skipLogin;

az logout;