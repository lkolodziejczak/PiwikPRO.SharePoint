Param (
    [Parameter(Position = 0, Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$sharePointUrl,

    [Parameter(Position = 1, Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$owner,

    [Parameter(Position = 2)]
    [string]$sharePointTenantAdminUrl,

    [Parameter(Position = 3)]
    [ValidateSet('2013', '2016', '2019', 'Online')]
    [string]$sharePointVersion = 'Online',

    [Switch]$useWebLogin
)

function Connect-ToSharePoint {
    Param (
        [Parameter(Position = 0, Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]$Url,
    
        [Parameter(Position = 1)]
        [string]$TenantAdminUrl,

        [Parameter(Position = 2)]
        [PSCredential]$Credentials,

        [Switch]$UseWebLogin
    )

    if ($UseWebLogin) {
        Connect-PnPOnline -Url $Url -TenantAdminUrl $TenantAdminUrl -UseWebLogin;
    }
    else {
        Connect-PnPOnline -Url $Url -TenantAdminUrl $TenantAdminUrl -Credentials $Credentials;
    }
}

$templatePath = ".\templates\PiwikPROTemplate.xml";
$spfxPackagePath = ".\solutions\piwikpro-sharepoint.sppkg";
$piwikAdminServerRelativeUrl = "/sites/PiwikAdmin";
$piwikAdminUrl = $sharePointUrl + $piwikAdminServerRelativeUrl;

if ((-not $sharePointTenantAdminUrl) -and $sharePointVersion -eq 'Online') {
    $sharePointTenantAdminUrl = $sharePointUrl.Replace(".sharepoint.com", "-admin.sharepoint.com");
}

if (-not (Get-Module -ListAvailable -Name "SharePointPnPPowerShell$($sharePointVersion)")) {
    Write-Host "You need to install PnP PowerShell $($sharePointVersion) to run this script." -ForegroundColor Yellow;
    Install-Module "SharePointPnPPowerShell$($sharePointVersion)";
}

$credentials;
if (-not $useWebLogin) {
    $credentials = Get-Credential -Message "Please provide credentials to SharePoint";
}

Write-Host "Connecting to SharePoint...";
Connect-ToSharePoint -Url $sharePointUrl -TenantAdminUrl $sharePointTenantAdminUrl -Credentials $credentials -UseWebLogin:$useWebLogin;

if ($sharePointVersion -eq 'Online') {
    Write-Host "Ensuring that Tenant App Catalog exists...";
    Register-PnPAppCatalogSite -Url "$sharePointUrl/sites/AppCatalog" -Owner $owner -TimeZoneId 4 -ErrorAction SilentlyContinue | Out-Null;
}

if (-not ($sharePointVersion -in "2013", "2016")) {
    Write-Host "Publishing SPFx package to Tenant App Catalog...";
    Add-PnPApp -Path $spfxPackagePath -Scope Tenant -Overwrite -Publish -SkipFeatureDeployment | Out-Null;
}

if ($sharePointVersion -eq 'Online') {
    $appCatalogUrl = Get-PnPTenantAppCatalogUrl;

    Write-Host "Connecting to Tenant App Catalog...";
    Disconnect-PnPOnline;
    Connect-ToSharePoint -Url $appCatalogUrl -TenantAdminUrl $sharePointTenantAdminUrl -Credentials $credentials -UseWebLogin:$useWebLogin;

    Write-Host "Configuring tenant-wide extensions...";
    Apply-PnPProvisioningTemplate -Path $templatePath -TemplateId "PIWIK-TENANT-WIDE";
}

Write-Host "Checking if Piwik PRO Admin site already exists...";
if (-not (Get-PnPTenantSite -Url $piwikAdminUrl -ErrorAction SilentlyContinue)) {
    Write-Host "Piwik PRO Admin site doesn't exist. Creating...";
    New-PnPTenantSite -Title "Piwik PRO Admin" -Url $piwikAdminUrl -Owner $owner -TimeZone 4 -Lcid 1033 -Template "STS#3" -Wait -Force | Out-Null;
}

Write-Host "Connecting to Piwik PRO Admin site...";
Disconnect-PnPOnline;
Connect-ToSharePoint -Url $piwikAdminUrl -TenantAdminUrl $sharePointTenantAdminUrl -Credentials $credentials -UseWebLogin:$useWebLogin;

Write-Host "Applying Piwik PRO Admin site template...";
Apply-PnPProvisioningTemplate -Path $templatePath -TemplateId "PIWIK-ADMIN-TEMPLATE" -Parameters @{"SharePointUrl" = $sharePointUrl; "PiwikAdminServerRelativeUrl" = $piwikAdminServerRelativeUrl; "Owner" = $owner };

Disconnect-PnPOnline;

Write-Host "Finished." -ForegroundColor Green;