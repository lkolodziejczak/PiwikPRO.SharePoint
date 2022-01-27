Param (
	[Parameter(Position = 0, Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$sharePointTenantAdminUrl,
	
	[Parameter(Position = 1, Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$piwikAdminUrl
)

	$FileName = 'installationLogsPiwikAdmin' 
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
    #New-LogFile -FileName 'installationLogsPiwikAdmin' -FolderName '.logs' -ErrorAction Stop
    Write-Host "==========START Additional script to manage Piwik Admin site=========="
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
		
		$tenantUrl = ([System.Uri]$config.sharePointUrl).GetLeftPart([System.UriPartial]::Authority)
		
		    Write-Host "Checking credentials"
    $credentials
    if (-not $config.useWebLogin) {
        $credentials = Get-Credential -Message "Please provide credentials to SharePoint" -ErrorAction Stop
    }

	Connect-ToSharePoint -Url $piwikAdminUrl -TenantAdminUrl $sharePointTenantAdminUrl -Credentials $credentials -UseWebLogin $config.useWebLogin
	
	if ($config.onlineParams.useSiteScope) {
	Set-PnPSite -NoScriptSite:$false
	}
		
	Write-Host "Applying Piwik PRO Administration site template"
	try
	{
		Apply-PnPProvisioningTemplate -Path $config.constants.templatePath -TemplateId "PIWIK-ADMIN-TEMPLATE" -Parameters @{"SharePointUrl" = $tenantUrl; "PiwikAdminServerRelativeUrl" = $config.constants.piwikAdminServerRelativeUrl; "Owner" = $config.sharepointAdminLogin }
	}
	catch
	{
		$ErrorMessage = $_.Exception.Message
		Write-Host $ErrorMessage
	}
	
	Write-Host "Checking if Everyone user has read permission."
		try
		{
			$isEveryoneAdded = $false
		   $usersAdded = Get-PnPUser -WithRightsAssigned
		   foreach($ua in $usersAdded) {
			   if($ua.Title -eq "Everyone")
			   {
					$isEveryoneAdded = $true
					 write-host "Everyone user has already read permission."
			   }
		   }

		   if($isEveryoneAdded -eq $false)
		   {
			write-host "Adding Everyone user to PiwikAdmin permissions"
			Set-PnPWebPermission -User 'c:0(.s|true' -AddRole 'Read' 
		   }
	   	}
		catch
		{
			$ErrorMessage = $_.Exception.Message
			Write-Host $ErrorMessage
		}
		
		$additionalPiwikAdmin = $config.additionalPiwikSiteCollAdmin
		if($additionalPiwikAdmin)
		{
			Write-Host "Adding additional sitecollection admins to PiwikAdmin site."
			try
			{
				
				Add-PnPSiteCollectionAdmin -Owners $additionalPiwikAdmin
			}
			catch
			{
				$ErrorMessage = $_.Exception.Message
				Write-Host $ErrorMessage
			}
		}

	# Adding PiwikConfig list items
        Write-Host "Adding items to PiwikConfig list";
        $listitem1Get = Get-PnPListItem -List "PiwikConfig" -Query "<View><Query><Where><Eq><FieldRef Name='Title'/><Value Type='Text'>piwik_clientid</Value></Eq></Where></Query></View>"
        if ($listitem1Get) {
            Set-PnPListItem -List "PiwikConfig" -Identity $listitem1Get -Values @{"Title" = "piwik_clientid"; "Value" = $config.clientIdValue }
        }
        else {
            $listItem1 = Add-PnPListItem -List "PiwikConfig" -Values @{"Title" = "piwik_clientid"; "Value" = $config.clientIdValue }
        }

        $listitem2Get = Get-PnPListItem -List "PiwikConfig" -Query "<View><Query><Where><Eq><FieldRef Name='Title'/><Value Type='Text'>piwik_clientsecret</Value></Eq></Where></Query></View>"
        if ($listitem2Get) {
            Set-PnPListItem -List "PiwikConfig" -Identity $listitem2Get -Values @{"Title" = "piwik_clientsecret"; "Value" = $config.clientSecretValue }
        }
        else {
            $listItem2 = Add-PnPListItem -List "PiwikConfig" -Values @{"Title" = "piwik_clientsecret"; "Value" = $config.clientSecretValue }
        }

        # Adding Property bag settings
        Write-Host "Adding Property Bag Settings";
		Set-PnPPropertyBagValue -Key "piwik_jobversion" -Value $config.onlineParams.constantsOnline.webJobVersion
        Set-PnPPropertyBagValue -Key "piwik_serviceurl" -Value $config.serviceUrlValue
        Start-Sleep -s 1

        if ($config.containersUrlValue) {
            Set-PnPPropertyBagValue -Key "piwik_containersurl" -Value $config.containersUrlValue
        }
        else {
            try {
                $webToContainers = Get-SPWeb $piwikAdminUrl
                $webToContainers.AllProperties.Add("piwik_containersurl", "")
                $webToContainers.Update()
            }
            catch {
                Write-Host "Containers url property bag is already exists"
            }
        }

	# Adding tagmanager.json
	$folderExists = Resolve-PnPFolder -SiteRelativePath "Style Library/PROD"

	if (!$folderExists) {
		Add-PnPFolder -Name PROD -Folder "Style Library"
	}
	$tagManagerPath = $config.constants.tagManagerFolder + "tagmanager.json"
	Add-PnPFile -Path $tagManagerPath -Folder "Style Library/PROD" | Out-Null

	Disconnect-PnPOnline
	}
catch
{
	$ErrorMessage = $_.Exception.Message
    Write-Host $ErrorMessage
}
	Write-Host "==========Finished succesfully=========="
	Stop-Transcript