Param (
    [Parameter(Position = 0, Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$SharePointUrl,
	
	[Switch]$UseWebLogin
    )

$piwikAdminServerRelativeUrl = "/sites/PiwikAdmin";
$tenantUrl = ([System.Uri]$SharePointUrl).GetLeftPart([System.UriPartial]::Authority);
$piwikAdminUrl = $tenantUrl + $piwikAdminServerRelativeUrl;

if (-not (Get-Module -ListAvailable -Name "SharePointPnPPowerShell$('Online')")) {
    Write-Host "You need to install PnP PowerShell $($SharePointVersion) to run this script." -ForegroundColor Yellow
    Install-Module "SharePointPnPPowerShell$('Online')"
}

if (-not $SharePointTenantAdminUrl) {
    $SharePointTenantAdminUrl = $tenantUrl.Replace(".sharepoint.com", "-admin.sharepoint.com")
}

    $credentials
    if (-not $UseWebLogin) {
        $credentials = Get-Credential -Message "Please provide credentials to SharePoint" -ErrorAction Stop
    }

if (-not $UseWebLogin) {
Connect-PnPOnline -Url $piwikAdminUrl -TenantAdminUrl $SharePointTenantAdminUrl -Credentials $credentials
}
else
{
Connect-PnPOnline -Url $piwikAdminUrl -TenantAdminUrl $SharePointTenantAdminUrl -UseWebLogin
}
$urlsToDeleteCustomAction = [System.Collections.ArrayList]@()
    $listItems= (Get-PnPListItem -List "Piwik PRO Site Directory" -Fields "Title","pwk_url")  
    foreach($listItem in $listItems){  
    
       Write-Host "Title" : $listItem["Title"]  
       Write-Host "Url" : $listItem["pwk_url"].Url
       $urlsToDeleteCustomAction.Add($listItem["pwk_url"].Url)
    }
    Disconnect-PnPOnline
    foreach($urlItem in $urlsToDeleteCustomAction){  
    Write-Host "Processing:" $urlItem
	
	if (-not $UseWebLogin) {
	Connect-PnPOnline -Url $urlItem -TenantAdminUrl $SharePointTenantAdminUrl -Credentials $credentials
	}
	else
	{
	Connect-PnPOnline -Url $urlItem -TenantAdminUrl $SharePointTenantAdminUrl -UseWebLogin
	}
    $customActionToDelete = Get-PnPCustomAction -Scope Site |? Name -eq "PiwikPRO.SharePoint365.Tracking"
        if($customActionToDelete)
        {
            Write-Host "Removing custom action..."
            Remove-PnPCustomAction -Identity $customActionToDelete.Id -Scope Site -Confirm -Force
            Write-Host "Done." -ForegroundColor Green
        }
    }
    Write-Host "Script has been finished." -ForegroundColor Green