Param (
    [Parameter(Position = 0, Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$SharePointUrl,

    [Parameter(Position = 1)]
    [string]$SharePointTenantAdminUrl,

    [Parameter(Position = 2)]
    [ValidateSet('2013', '2016', '2019', 'Online')]
    [string]$SharePointVersion = 'Online',

    [Switch]$UseSiteScope,

    [Switch]$UseWebLogin
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
$tenantUrl = ([System.Uri]$SharePointUrl).GetLeftPart([System.UriPartial]::Authority);
$piwikAdminUrl = $tenantUrl + $piwikAdminServerRelativeUrl;

if ((-not $SharePointTenantAdminUrl) -and $SharePointVersion -eq 'Online') {
    $SharePointTenantAdminUrl = $tenantUrl.Replace(".sharepoint.com", "-admin.sharepoint.com");
}

if (-not (Get-Module -ListAvailable -Name "SharePointPnPPowerShell$($SharePointVersion)")) {
    Write-Host "You need to install PnP PowerShell $($SharePointVersion) to run this script." -ForegroundColor Yellow;
    Install-Module "SharePointPnPPowerShell$($SharePointVersion)";
}

$credentials;
if (-not $UseWebLogin) {
    $credentials = Get-Credential -Message "Please provide credentials to SharePoint";
}

Write-Host "Connecting to SharePoint...";
Connect-ToSharePoint -Url $SharePointUrl -TenantAdminUrl $SharePointTenantAdminUrl -Credentials $credentials -UseWebLogin:$UseWebLogin;

$currentUser = (Get-PnPProperty (Get-PnPWeb) -Property CurrentUser).LoginName;

if ($SharePointVersion -eq 'Online') {
    if (-not $UseSiteScope) {
        Write-Host "Ensuring that Tenant App Catalog exists...";
        Register-PnPAppCatalogSite -Url "$tenantUrl/sites/AppCatalog" -Owner $currentUser -TimeZoneId 4 -ErrorAction SilentlyContinue | Out-Null;
    } else {
        Write-Host "Ensuring that Site Collection App Catalog exists...";
        Add-PnPSiteCollectionAppCatalog -Site $SharePointUrl -ErrorAction SilentlyContinue | Out-Null;
    }
}

if (-not ($SharePointVersion -in "2013", "2016")) {
    Write-Host "Publishing SPFx package to App Catalog...";
    if (-not $UseSiteScope) {
        Add-PnPApp -Path $spfxPackagePath -Scope Tenant -Overwrite -Publish -SkipFeatureDeployment | Out-Null;
    } else {
        Add-PnPApp -Path $spfxPackagePath -Scope Site -Overwrite -Publish -SkipFeatureDeployment | Out-Null;
    }
}

if ($SharePointVersion -eq 'Online') {
    if (-not $UseSiteScope) {
        $appCatalogUrl = Get-PnPTenantAppCatalogUrl;

        Write-Host "Connecting to Tenant App Catalog...";
        Disconnect-PnPOnline;
        Connect-ToSharePoint -Url $appCatalogUrl -TenantAdminUrl $SharePointTenantAdminUrl -Credentials $credentials -UseWebLogin:$UseWebLogin;
    }

    Write-Host "Configuring tenant-wide extensions...";
    Apply-PnPProvisioningTemplate -Path $templatePath -TemplateId "PIWIK-TENANT-WIDE";
}

Write-Host "Checking if Piwik PRO Administration site already exists...";
if (-not (Get-PnPTenantSite -Url $piwikAdminUrl -ErrorAction SilentlyContinue)) {
    Write-Host "Piwik PRO Administration site doesn't exist. Creating...";
    New-PnPTenantSite -Title "Piwik PRO Administration" -Url $piwikAdminUrl -Owner $currentUser -TimeZone 4 -Lcid 1033 -Template "STS#3" -Wait -Force | Out-Null;
}

Write-Host "Connecting to Piwik PRO Administration site...";
Disconnect-PnPOnline;
Connect-ToSharePoint -Url $piwikAdminUrl -TenantAdminUrl $SharePointTenantAdminUrl -Credentials $credentials -UseWebLogin:$UseWebLogin;

Write-Host "Applying Piwik PRO Administration site template...";
Apply-PnPProvisioningTemplate -Path $templatePath -TemplateId "PIWIK-ADMIN-TEMPLATE" -Parameters @{"SharePointUrl" = $tenantUrl; "PiwikAdminServerRelativeUrl" = $piwikAdminServerRelativeUrl; "Owner" = $currentUser };

Disconnect-PnPOnline;

Write-Host "Finished." -ForegroundColor Green;