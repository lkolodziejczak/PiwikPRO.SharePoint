Param (
	[Parameter(Position = 0, Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$sharePointTenantAdminUrl
)

$FileName = 'installationLogsTenant' 
$FolderName = '.logs'
	$isFolderPresent = Test-Path ".\$FolderName\logs"
	if($isFolderPresent -eq $false)
	{
		New-Item -Path ".\$FolderName\logs" -ItemType Directory
    }
    $FileName = $FileName + (Get-Date).toString('yyyyMMddHHmmss')
	$path = ".\$FolderName\logs\$FileName.log"

$ErrorActionPreference="SilentlyContinue"
Stop-Transcript | out-null
$ErrorActionPreference = "Continue"
Start-Transcript -path $path -append

    # Get config json file
    $configFileName = '.\installationConfig'

    # Import module
    Import-Module '.\setupLibrary\setupLibrary.psd1' -Force -ErrorAction Stop

    # Created log file for script and start logs in file
    Write-Host "==========START Additional script to manage tenant=========="
    Write-Host "Piwik - setup"
try
{
    # Get json config file
    Write-Host "Loading configuration file"
    $isAbleToLoadConfigFile = Get-Config -JsonConfigFileName $configFileName
    if($isAbleToLoadConfigFile -eq $false)
    {
        Write-Host "Error when loading configuration file -> exit"
        exit
    }
    Write-Host "Configuration file loaded"
    $config = $global:JsonConfig

        Get-SharePointPnPPowerShell -SharePointVersion 'Online'
		Start-Sleep -s 1
		
		    Write-Host "Checking credentials"
    $credentials
    if (-not $config.useWebLogin) {
        $credentials = Get-Credential -Message "Please provide credentials to SharePoint" -ErrorAction Stop
    }

Write-Host "Connecting to SharePoint"
        Connect-ToSharePoint -Url $config.sharePointUrl -TenantAdminUrl $sharePointTenantAdminUrl -Credentials $credentials -UseWebLogin $config.useWebLogin

        # Checking if app catalog is present
        if (!$config.onlineParams.useSiteScope) {
            Write-Host "Ensuring that Tenant App Catalog exists, if not then creating..."
			try
			{
				if(Get-PnPTenantAppCatalogUrl)
				{
				
				}
				else
				{
					Write-Host "Creating App Catalog site... it could take a few minutes."
					$tenantUrl = ([System.Uri]$config.sharePointUrl).GetLeftPart([System.UriPartial]::Authority)
					$spUrl = $tenantUrl + "/sites/AppCatalog"
					Register-PnPAppCatalogSite -Url $spUrl -Owner $config.sharepointAdminLogin -TimeZoneId 4 -ErrorAction SilentlyContinue | Out-Null -WarningAction Ignore
					Start-Sleep -s 30
					$timerWaiter = 1				
					Do {
						if($timerWaiter%8 -eq 0)
						{
							Write-Host "Refreshing connection.."				
							Disconnect-PnPOnline
							Start-Sleep -s 5	
							Connect-ToSharePoint -Url $config.sharePointUrl -TenantAdminUrl $sharePointTenantAdminUrl -Credentials $credentials -UseWebLogin $config.useWebLogin
							Start-Sleep -s 2
						}
						Write-Host "Waiting for fully creation of app catalog site..."
						$appCatalogUrl = Get-PnPTenantAppCatalogUrl
						Start-Sleep -s 5
						if($timerWaiter%64 -eq 0)
						{
							Write-Host "Some problem occurs, registering app catalog again."
							Register-PnPAppCatalogSite -Url $spUrl -Owner $config.sharepointAdminLogin -TimeZoneId 4 -ErrorAction SilentlyContinue | Out-Null -WarningAction Ignore
							Start-Sleep -s 30
						}
						$timerWaiter++
					}
					while ([string]::IsNullOrEmpty($appCatalogUrl))
					
					Write-Host "Finishing creation of App Catalog site"
					Start-Sleep -s 100
				}
				# If not site scope connect to app catalog
				if (!$config.onlineParams.useSiteScope) {
					$appCatalogUrl = Get-PnPTenantAppCatalogUrl
					$config.onlineParams.constantsOnline | % {$_.appCatalogUrl=$appCatalogUrl}
					$config | ConvertTo-Json -depth 32| set-content '.\installationConfig.json'
					Start-Sleep -s 1
				}
			}
			catch [Exception]
			{
				Write-Host $PSItem.ToString()
				$ErrorMessage = $_.Exception.Message
				Write-Host $ErrorMessage
			}
		}
        else {
            Write-Host "Ensuring that Site Collection App Catalog exists"
            Add-PnPSiteCollectionAppCatalog -Site $config.sharePointUrl -ErrorAction SilentlyContinue | Out-Null
            Start-Sleep -s 2
            if ($config.sharePointUrl -notlike "*$config.constants.piwikAdminServerRelativeUrl*") {
                Write-Host "Ensuring that Site Collection no scripts is turned off"
                Set-PnPSite -NoScriptSite:$false
				Start-Sleep -s 2

                Write-Host "Ensuring property piwik_metasitenamestored is set"
                Set-PnPPropertyBagValue -Key $config.onlineParams.constantsOnline.piwikMetaSiteNameStored -Value $config.onlineParams.piwikSiteId

                Write-Host "Ensuring property piwik_istrackingactive is set"
                Set-PnPPropertyBagValue -Key $config.onlineParams.constantsOnline.piwikIsTrackingActive -Value 'true'
				
				$alreadyAddedCustomaction = $false
				Get-PnPCustomAction -Scope Site | ForEach-Object {
					if($_.Name -Match "piwikpro")
					{
						$alreadyAddedCustomaction = $true
					}
				}
				
				if(!$alreadyAddedCustomaction)
				{
					Write-Host "Adding custom actions"
					Add-PnPCustomAction -Title "piwikpro-sharepoint-ListTracking-100" -Name "piwikpro-sharepoint-ListTracking-100" -Location "ClientSideExtension.ListViewCommandSet" -ClientSideComponentId a0a0acea-cd3c-454b-9376-9cd0e98f5847 -ClientSideComponentProperties "{}" -Scope Site
					Add-PnPCustomAction -Title "piwikpro-sharepoint-ListTracking-101" -Name "piwikpro-sharepoint-ListTracking-101" -Location "ClientSideExtension.ListViewCommandSet" -ClientSideComponentId a0a0acea-cd3c-454b-9376-9cd0e98f5847 -ClientSideComponentProperties "{}" -Scope Site
					Add-PnPCustomAction -Title "piwikpro-sharepoint-Tracking" -Name "piwikpro-sharepoint-Tracking" -Location "ClientSideExtension.ApplicationCustomizer" -ClientSideComponentId 2ff5e374-69cb-4645-9083-b6317019705b -ClientSideComponentProperties "{}" -Scope Site
					Add-PnPCustomAction -Title "piwikpro-sharepoint-ListTracking-119" -Name "piwikpro-sharepoint-ListTracking-119" -Location "ClientSideExtension.ListViewCommandSet" -ClientSideComponentId a0a0acea-cd3c-454b-9376-9cd0e98f5847 -ClientSideComponentProperties "{}" -Scope Site
				}
            }
        }
		Start-Sleep -s 10
        # Publish Spfx
        Write-Host "Publishing SPFx package to App Catalog"
		try
		{
			$spfxPackagePath2 = $config.constants.spfxPackagePath
			if (!$config.onlineParams.useSiteScope) {
				Add-PnPApp -Path $spfxPackagePath2 -Scope Tenant -Overwrite -Publish -SkipFeatureDeployment | Out-Null
			}
			else {
				Add-PnPApp -Path $spfxPackagePath2 -Scope Site -Overwrite -Publish -SkipFeatureDeployment | Out-Null
			}
		}
		catch [Exception]
		{
			Write-Host $PSItem.ToString()
		}
		Disconnect-PnPOnline
}
catch [Exception]
{
	$ErrorMessage = $_.Exception.Message
    Write-Host $ErrorMessage
}
Write-Host "==========Finished succesfully=========="

Stop-Transcript