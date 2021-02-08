#variables - to change
$IISWebSiteName = "piwikpro.piwik.local"
$IISAppPoolName = "piwikpro.piwik.local"
$WebApplicationTrackerUrl = "http://piwik-sp2013:35086/"

Write-Host "Removing WebSite from IIS..."
try
{
Remove-WebSite -Name $IISWebSiteName
Write-Host "WebSite has been removed from IIS." -ForegroundColor Green 
}
catch
{
Write-Host "Problem with removing WebSite from IIS"
}

Write-Host "Removing AppPool from IIS..."
try
{
Remove-WebAppPool -Name $IISAppPoolName
Write-Host "AppPool has been removed from IIS." -ForegroundColor Green 
}
catch
{
Write-Host "Problem with removing AppPool from IIS"
}

Add-PSSnapin "Microsoft.SharePoint.PowerShell" -ErrorAction SilentlyContinue

Write-Host "Iterating all Site Collections inside Web Aplication:" $WebApplicationTrackerUrl

try
{
	$SPWebApp = Get-SPWebApplication $WebApplicationTrackerUrl
	foreach ($SPSite in $SPWebApp.Sites)
	{
		if ($SPSite -ne $null)
		{
			try
			{
				 $appInstance = Get-SPAppInstance -Web $SPSite.url | where-object {$_.Title -eq "Piwik PRO"}

				### Check if the app exists in the site
				if ($appInstance -ne $null)
				{
				Write-Host "Uninstalling the app from the SharePoint site...." $SPSite.url

				### Remove the app from the site
				Uninstall-SPAppInstance –Identity $appInstance -Confirm:$false
				Write-Host -ForegroundColor Green "Piwik PRO app has been uninstalled from site" $SPSite.url		
				}
				
				
				### Remove custom action for Piwik PRO old tracker
				try {
                    $Web = Get-SPWeb $SPSite.url
					#Get the Custom Actions Filter by Title
					$CustomActions = $Web.UserCustomActions | Where { $_.Title -eq "PiwikPRO" } | Select ID, Title
				 
					if($CustomActions -ne $null)
					{
						#Delete custom action(s)
						$CustomActions | ForEach-Object {
							#Remove the custom action
							$Web.UserCustomActions.Item($_.Id).Delete()
							Write-Host -ForegroundColor Green "Custom Action Deleted Successfully!"
						}
					}
				} catch {
					Write-Host -ForegroundColor Red "Error:" $_.Exception.Message
				}
			}
			catch
			{
				Write-Host "Problem with remove app from Site collection:" $SPSite.url
			}
			$SPSite.Dispose()
			
		}
	}
}
catch
{
	Write-Host "Problem with connection to the WebApplication"
}