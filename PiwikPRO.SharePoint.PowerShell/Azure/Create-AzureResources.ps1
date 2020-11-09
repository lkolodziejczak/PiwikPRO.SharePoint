Param (
    [Parameter(Position = 0, Mandatory = $true)]
    [string]$Tenant,
    [Parameter(Position = 1, Mandatory = $true)]
    [string]$Subscription,
    [Parameter(Position = 2, Mandatory = $true)]
    [string]$ResourceGroupName,
    [Parameter(Position = 3, Mandatory = $true)]
    [ValidateLength(1, 16)]
    [string]$WebAppSuffix,
    [Parameter(Position = 4, Mandatory = $true)]
    [string]$SharePointUrl,
    [Parameter(Position = 5)]
    [SecureString]$CertificatePassword,
    [Parameter(Position = 6)]
    [string]$Location = "westeurope",
    [Switch]$NoConsent
)

$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition;
$templateFilePath = "$scriptPath\templates\template.json";
$appPermissionsFilePath = "$scriptPath\templates\appPermissions.json";
$webAppName = "piwikpro" + $WebAppSuffix;
$piwikAdminSiteUrl = "$SharePointUrl/sites/PiwikAdmin";

if (-not (Get-Command az -ErrorAction SilentlyContinue) -or [System.Version](az version --query '\"azure-cli\"' -o tsv) -lt [System.Version]"2.9.1") {
    Write-Host "Installing Azure CLI...";
    Invoke-WebRequest -Uri https://aka.ms/installazurecliwindows -OutFile .\AzureCLI.msi; Start-Process msiexec.exe -Wait -ArgumentList '/I AzureCLI.msi /quiet'; rm .\AzureCLI.msi;
    Write-Host "Azure CLI installed.";
    $Env:Path = "${Env:ProgramFiles(x86)}\Microsoft SDKs\Azure\CLI2\wbin;${Env:Path}";
}

$SharePointUrl -match "([a-zA-Z0-9-_]+)\.sharepoint\.com" | Out-Null;
$sharePointTenant = "$($Matches[1]).onmicrosoft.com";

Write-Host "Connecting to Office 365 tenant...";
az login --tenant "$sharePointTenant" --allow-no-subscriptions -o none;

$cert = New-SelfSignedCertificate -CertStoreLocation "cert:\CurrentUser\My" -Subject "CN=PiwikPRO" -NotAfter (Get-Date).AddYears(10);
$keyValue = [System.Convert]::ToBase64String($cert.GetRawCertData());

if (-not $CertificatePassword)
{
    $CertificatePassword = Read-Host -Prompt "Enter password to protect private key for certificate" -AsSecureString;
}

$cert | Export-PfxCertificate -FilePath "$($scriptPath)\PiwikPRO.pfx" -Password $CertificatePassword;

$clientId = az ad app list --display-name "$webAppName" --query '[].appId' -o tsv;
if ($clientId) {
    az ad app update --id "$clientId" --display-name "$webAppName" --key-value "$keyValue" --start-date "$($cert.NotBefore.ToString('o'))" --end-date "$($cert.NotAfter.ToString('o'))" --required-resource-accesses "@$appPermissionsFilePath" -o none;
}
else {
    $clientId = az ad app create --display-name "$webAppName" --key-value "$keyValue" --start-date "$($cert.NotBefore.ToString('o'))" --end-date "$($cert.NotAfter.ToString('o'))" --required-resource-accesses "@$appPermissionsFilePath" --query 'appId' -o tsv;
}

if (-not (az ad sp list --filter "appId eq '$clientId'" --query '[].appId' -o tsv)) {
    az ad sp create --id "$clientId" -o none;
}

if (-not $NoConsent) {
    az ad app permission admin-consent --id "$clientId" -o none;
}

if ($sharePointTenant -ne $Tenant) {
    Write-Host "Connecting to Azure tenant...";
    az logout;
    az login --tenant "$Tenant" -o none;
}

az account set -s "$Subscription" -o none;

if (-not [System.Convert]::ToBoolean((az group exists -n "$ResourceGroupName"))) {
    az group create -l "$Location" -n "$ResourceGroupName" -o none;
}

az deployment group create -g "$ResourceGroupName" --template-file "$templateFilePath" --parameters sites_piwikpro_name="$webAppName" components_piwikpro_name="$webAppName" serverfarms_piwikproPlan_name="$webAppName" storageAccounts_piwikprostorage_name="$webAppName" piwikAdminSiteUrl="$piwikAdminSiteUrl" certificateThumbprint="$($cert.Thumbprint)" tenant="$sharePointTenant" clientId="$clientId" -o none;
az webapp config ssl upload --certificate-file "$scriptPath\PiwikPRO.pfx" --certificate-password "$([Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($CertificatePassword)))" --name "$webAppName" --resource-group "$ResourceGroupName" -o none;

$certs = Get-ChildItem -Path "cert:\CurrentUser\My" | Where-Object {$_.Subject -eq "CN=PiwikPRO"};
foreach ($c in $certs)
{
    Remove-Item $c.PSPath;
}

.\Deploy-AzureWebJob -ResourceGroupName $ResourceGroupName -WebAppName $webAppName -SkipLogin;

az logout;