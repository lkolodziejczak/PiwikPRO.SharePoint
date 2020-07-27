Param (
    [Parameter(Position = 0, mandatory = $true)]
    [string]$tenant,
    [Parameter(Position = 1, mandatory = $true)]
    [string]$subscription,
    [Parameter(Position = 2, mandatory = $true)]
    [string]$resourceGroupName,
    [Parameter(Position = 3, mandatory = $true)]
    [ValidateLength(1, 16)]
    [string]$webAppSuffix,
    [Parameter(Position = 4, mandatory = $true)]
    [string]$sharePointUrl
)

$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition;
$templateFilePath = $($scriptPath + "\template.json");
$webAppName = 'piwikpro' + $webAppSuffix;

Connect-AzAccount -Subscription $subscription -Tenant $tenant;

$cert = New-SelfSignedCertificate -CertStoreLocation "cert:\CurrentUser\My" -Subject "CN=PiwikPROCertificate";
$keyValue = [System.Convert]::ToBase64String($cert.GetRawCertData());
$password = [System.Web.Security.Membership]::GeneratePassword(15, 0);
$secPassword = ConvertTo-SecureString -String $password -AsPlainText -Force;
$cert | Export-PfxCertificate -FilePath "$($scriptPath)/PiwikPROCertificate.pfx" -Password $secPassword;
  
$principal = New-AzADServicePrincipal -DisplayName $webAppName -CertValue $keyValue -EndDate $cert.NotAfter -StartDate $cert.NotBefore;

if (-not (Get-AzResourceGroup -Name $resourceGroupName -ErrorAction SilentlyContinue)) {
    New-AzResourceGroup -Name $resourceGroupName -Location westeurope -Force;
}
New-AzResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile $templateFilePath -TemplateParameterObject @{
    "sites_piwikpro_name"                  = $webAppName;
    "components_piwikpro_name"             = $webAppName;
    "serverfarms_piwikproPlan_name"        = $webAppName;
    "storageAccounts_piwikprostorage_name" = $webAppName;
};

$certificate = New-AzApplicationGatewaySslCertificate -Name "PiwikPROCertificate" -CertificateFile "$($scriptPath)/PiwikPROCertificate.pfx" -Password $secPassword;
New-AzResource -ResourceName "PiwikPROCertificate.pfx" -Location "westeurope" -PropertyObject @{
    pfxBlob      = $certificate.Data;  
    password     = $password;       
    ResourceType = "Microsoft.Web/Certificates"
} -ResourceGroupName $resourceGroupName -ResourceType Microsoft.Web/certificates -ApiVersion '2018-02-01' -Force;

$instrumentationKey = (Get-AzApplicationInsights -ResourceGroupName $resourceGroupName -Name $webAppName).InstrumentationKey;
$saKey = (Get-AzStorageAccountKey -ResourceGroupName $resourceGroupName -Name $webAppName)[0].Value;
$appSettingsHash = @{};
$appSettingsHash['APPINSIGHTS_INSTRUMENTATIONKEY'] = $instrumentationKey;
$appSettingsHash['APPINSIGHTS_PROFILERFEATURE_VERSION'] = '1.0.0';
$appSettingsHash['ApplicationInsightsAgent_EXTENSION_VERSION'] = '~2';
$appSettingsHash['DiagnosticServices_EXTENSION_VERSION'] = '~3';
$appSettingsHash['ClientId'] = $principal.Id;
$appSettingsHash['PiwikAdminSiteUrl'] = $sharePointUrl + "/sites/PiwikAdmin";
$appSettingsHash['Tenant'] = $tenant;
$appSettingsHash['Thumbprint'] = $cert.Thumbprint;
$appSettingsHash['WEBSITE_LOAD_CERTIFICATES '] = $cert.Thumbprint;
$connectionString = "DefaultEndpointsProtocol=https;AccountName=$($webAppName);AccountKey=$($saKey);EndpointSuffix=core.windows.net";
$connectionStringsHash = @{ AzureWebJobsDashboard = @{ Type = "Custom"; Value = $connectionString }; AzureWebJobsStorage = @{ Type = "Custom"; Value = $connectionString } };
Set-AzWebApp -ResourceGroupName $resourceGroupName -Name $webAppName -AppSettings $appSettingsHash -ConnectionStrings $connectionStringsHash;