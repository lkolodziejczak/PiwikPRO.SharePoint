Param (
    [Parameter(Position = 0, Mandatory = $true)]
    [string]$tenantName,
    [Parameter(Position = 1, Mandatory = $true)]
    [string]$owner
)

$templatePath = ".\PiwikPROTemplate.xml";
$sharePointUrl = "https://$($tenantName).sharepoint.com";
$sharePointTenantAdminUrl = "https://$($tenantName)-admin.sharepoint.com";
$piwikAdminServerRelativeUrl = "/sites/PiwikAdmin";
$piwikAdminUrl = $sharePointUrl + $piwikAdminServerRelativeUrl;

if (-not (Get-Module -ListAvailable -Name SharePointPnPPowerShellOnline)) {
    Write-Host "You need to install PnP PowerShell to run this script." -ForegroundColor Yellow;
    Install-Module SharePointPnPPowerShellOnline;
}

if (Test-Path $templatePath -PathType Leaf) {
    Write-Host "Logging in to tenant admin site...";
    Connect-PnPOnline -Url $sharePointTenantAdminUrl -UseWebLogin;
    Write-Host "Checking if Piwik PRO Admin site already exists...";
    if (-not (Get-PnPTenantSite -Url $piwikAdminUrl -ErrorAction SilentlyContinue)) {
        Write-Host "Piwik PRO Admin site doesn't exist. Creating...";
        New-PnPTenantSite -Title "Piwik PRO Admin" -Url $piwikAdminUrl -Owner $owner -TimeZone 4 -Lcid 1033 -Template "STS#3" -Wait -Force;
        Write-Host "Piwik PRO Admin site created.";
    } else {
        Write-Host "Using existing Piwik PRO Admin site...";
    }
    Disconnect-PnPOnline;

    Write-Host "Logging in to Piwik PRO Admin site...";
    Connect-PnPOnline -Url $piwikAdminUrl -UseWebLogin;
    Write-Host "Applying Piwik PRO Admin site template...";
    Apply-PnPProvisioningTemplate -Path $templatePath -TemplateId "PIWIK-ADMIN-TEMPLATE" -Parameters @{"SharePointUrl" = $sharePointUrl; "PiwikAdminServerRelativeUrl" = $piwikAdminServerRelativeUrl; "Owner" = $owner};
    Disconnect-PnPOnline;

    Write-Host "Finished." -ForegroundColor Green;
}
else {
    Write-Host "Template file not found, nothing to do, exiting." -ForegroundColor Red;
}