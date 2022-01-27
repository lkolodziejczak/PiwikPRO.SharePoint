Param (
    [Switch]$RunWithoutUI
)

function ActivateFeatureInSiteCollectionScope($DisplayName, $siteurl) {
    Write-Host "Activating Feature :- " $DisplayName " -: In Site Collection " $siteurl
    $TempCount = (Get-SPSite  $siteurl | % { Get-SPFeature -Site $_ } | Where-Object { $_.ID -eq $DisplayName } ).Count
    if ($TempCount -eq 0) {
        # if not, Enable the Feature.
        Get-SPFeature -Identity $DisplayName | Enable-SPFeature -Url $siteurl -ErrorAction SilentlyContinue
    }            
    else {
        # If already Activated, then De-Activate and Activate Again.
        Disable-SPFeature -Identity $DisplayName -Url $siteurl  –Confirm:$false
        Get-SPFeature -Identity $DisplayName | Enable-SPFeature -Url $siteurl -ErrorAction SilentlyContinue
    }
}
 
function ActivateFeatureInWebApplicationScope($DisplayName, $webApplicationUrl) {
         
    Write-Host "Activating Feature :- " $DisplayName " -: In Web Application " $webApplicationUrl
    $TempCount = (Get-SPWebApplication $webApplicationUrl | % { Get-SPFeature -WebApplication $_ } | Where-Object { $_.ID -eq $DisplayName } ).Count
    if ($TempCount -eq 0) {
        # if not, Enable the Feature.
        Get-SPFeature  $DisplayName | Enable-SPFeature -Url $webApplicationUrl 
    }            
    else {
        # If already Activated, then De-Activate and Activate Again.
        Disable-SPFeature $DisplayName -Url $webApplicationUrl  –Confirm:$false
        Get-SPFeature  $DisplayName | Enable-SPFeature -Url  $webApplicationUrl 
    }
}

function UploadFiles($siteUrl, $DestFolderUrl, $LocalFileOrFolderPath) {
    try {
        if ((get-item $LocalFileOrFolderPath).PSIsContainer -eq $true) {
            write-host "Looping Items in Folder: $LocalFileOrFolderPath"
            Get-ChildItem $LocalFileOrFolderPath | Foreach-Object {
                $name = $_.Name
                $fullName = $_.FullName
                if ((get-item $fullName).PSIsContainer -eq $false) {
                    #only files, not folders
                    UploadandApprove -siteUrl $siteUrl -DestFolderUrl $DestFolderUrl -LocalFilePath $fullName
                }
                else {
                    CreateSPFolder -siteUrl $siteUrl -DestFolderUrl $DestFolderUrl -LocalFolderName $name | Out-Null
                    UploadFiles -siteUrl $siteUrl -DestFolderUrl ($DestFolderUrl + "/" + $name) -LocalFileOrFolderPath $fullName #recursively iterate 
                }
		 
            }
        }
        else {
            UploadandApprove -spWeb $spWeb -siteUrl $siteUrl -DestFolderUrl $DestFolderUrl -LocalFilePath $_.FullName
        }
    }
    catch {
		Add-Log -Level "ERROR" -Message "Error during upload files"
        write-host -ForegroundColor Red "Exception Occurred:"
        Write-Host -ForegroundColor Red $_.Exception.Message
        Write-Host -ForegroundColor Red $_.Exception.stacktrace
    } 
    finally {
        $spSite.Dispose()
        $spWeb.Dispose()
    }
}

function CreateSPFolder($siteUrl, $DestFolderUrl, $LocalFolderName) {
    
    write-host "Creating SPFolder: " $LocalFolderName
    ############################################# Upload ###########################################
    try {  
        $destfolder = $spWeb.GetFolder($DestFolderUrl)
        $destfolder.SubFolders.Add($LocalFolderName)
        $destfolder.update();
    }
    catch {
		Add-Log -Level "ERROR" -Message "Error during create new SPFolder"
        write-host -ForegroundColor Red "Exception Occurred:"
        Write-Host -ForegroundColor Red $_.Exception.Message
        Write-Host -ForegroundColor Red $_.Exception.stacktrace
    } 
}

function UploadandApprove($siteUrl, $DestFolderUrl, $LocalFilePath) {

    write-host "Uploading " $LocalFilePath
    ############################################# Upload ###########################################
    try {  
        $destfolder = $spWeb.GetFolder($DestFolderUrl) 
        $spList = $destfolder.Item.ParentList            
        $spFiles = $destfolder.Files;  

        $fileInfo = Get-Item $LocalFilePath
        $fileStream = ([System.IO.FileInfo] (Get-Item $LocalFilePath)).OpenRead()
        $fileName = $fileInfo.Name
        $ExistingFile = $spFiles[$fileName]
        if ($ExistingFile.Exists -and $spList.EnableVersioning -eq $true) {
            #check out
            $ExistingFile.CheckOut()    
        }
 
        #update file
        try {  
            $spFile = $spFiles.Add($fileName, $fileStream, $true);
        }
        catch {}
        try {  
            if ($spFile.CheckOutType -ne [Microsoft.SharePoint.SPFile+SPCheckOutType]::None) {
                $spFile.CheckIn("")    
            }

            if ($spList.EnableMinorVersions -eq $true) {
                $spFile.publish("");
            }
        }
        catch {}

        ########################### Approve Items ############################################
        try {  
            $destfolder = $spWeb.GetFolder($DestFolderUrl) 
            $spList = $destfolder.Item.ParentList    
            $spFiles = $destfolder.Files;  
 
            $fileInfo = Get-Item $LocalFilePath
            $fileName = $fileInfo.Name
            $ExistingFile = $spFiles[$fileName]
            if ($ExistingFile.Exists -and $spList.EnableModeration -eq $true) {
                $ExistingFile.Approve('')
    
            } 			
        }
        catch {}
        #Dispose of Web object
    }
    catch {
        write-host -ForegroundColor Red "Exception Occurred:"
        Write-Host -ForegroundColor Red $_.Exception.Message
        Write-Host -ForegroundColor Red $_.Exception.stacktrace
    } 
}

	$FileName = 'installationLogsGlobal' 
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


Try
{
	Write-Host "========== Checking environment =========="
	if((Get-Host).Version.Major -gt 4)
	{
		Write-Host "Powershell version is correct." -ForegroundColor Green
	}
	else
	{
		Write-Host "Powershell version is too low. Please install Windows Management Framework 5.1 from: https://www.microsoft.com/en-us/download/details.aspx?id=54616" -ForegroundColor Yellow
		break
	}
	
	if ((Get-command -Module PowerShellGet).count -eq 0) 
	{ 
		Write-Output -InputObject "NuGet package provider is not available on this machine, trying to install it now"
		Install-PackageProvider -Name NuGet -Force
	}
	
	Write-Host "========== End checking environment =========="

    # Get config json file
    $configFileName = '.\installationConfig'

    # Import module
    Import-Module '.\setupLibrary\setupLibrary.psd1' -Force -ErrorAction Stop

    # Created log file for script and start logs in file
    #New-LogFile -FileName 'installationLogs' -FolderName '.logs' -ErrorAction Stop
    Add-Log -Level "INFO" -Message "==========START=========="
    Add-Log -Level "INFO" -Message "Piwik - setup"
	
	if($RunWithoutUI -eq $false)
    {
	
	$UIAppPath = $PSScriptRoot + '\PiwikPRO_InstallScriptUI\PiwikPRO_InstallScriptUI.exe'
	
	$procUI = Start-Process -Wait -FilePath $UIAppPath
	do{sleep 1}while(Get-Process -Name "PiwikPRO_InstallScriptUI.exe" -ErrorAction SilentlyContinue)
	}

    # Get json config file
    Add-Log -Level "INFO" -Message "Loading configuration file"
    $isAbleToLoadConfigFile = Get-Config -JsonConfigFileName $configFileName
    if($isAbleToLoadConfigFile -eq $false)
    {
        Add-Log -Level "ERROR" -Message "Error when loading configuration file -> exit"
        exit
    }
    Add-Log -Level "INFO" -Message "Configuration file loaded"
    $config = $global:JsonConfig
	
	if($RunWithoutUI -eq $false)
	{
		if ($config.cancelInstallation) {
			Add-Log -Level "INFO" -Message "Installation has been cancelled."
			break
		}
	}

    # Checking if credentials needed or use web login
    Add-Log -Level "INFO" -Message "Checking credentials"
    $credentials
    if ((-not $config.useWebLogin) -or ($config.sharePointVersion -in "2013", "2016", "2019")) {
        $credentials = Get-Credential -Message "Please provide credentials to SharePoint" -ErrorAction Stop
    }

    # Define tenant url and piwik admin url
    $tenantUrl = ([System.Uri]$config.sharePointUrl).GetLeftPart([System.UriPartial]::Authority)
    $piwikAdminUrl = $tenantUrl + $config.constants.piwikAdminServerRelativeUrl

    # Define admin site url
    Add-Log -Level "INFO" -Message "Setting SharePointTenantAdminUrl"
    $sharePointTenantAdminUrl = $config.sharePointTenantAdminUrl
    if ($sharePointTenantAdminUrl -eq '') {
        $sharePointTenantAdminUrl = $tenantUrl.Replace(".sharepoint.com", "-admin.sharepoint.com")
    }
    Add-Log -Level "INFO" -Message "SharePointTenantAdminUrl set to $sharePointTenantAdminUrl"
	
	$toChangeServiceUrl = $false
	$serviceUrl = $config.serviceUrlValue
	if($serviceUrl[$serviceUrl.Length-1] -eq '/') { 
		$toChangeServiceUrl = $true
		$serviceUrl = $serviceUrl.Substring(0,$serviceUrl.Length-1) 
	}
	if($toChangeServiceUrl -eq $true)
	{
		$config | % {$_.serviceUrlValue=$serviceUrl}
		$config | ConvertTo-Json -depth 32| set-content '.\installationConfig.json'
	}

    # Define setup installation type
    if ($config.sharePointVersion -eq 'Online') {

        Add-Log -Level "INFO" -Message "Setup is running sharepoint online installation"
		
		Add-Log -Level "INFO" -Message "Checking and uninstalling SharePointPnPPowerShell other versions module"
        if (Get-Module -ListAvailable -Name "SharePointPnPPowerShell2019") {
            Uninstall-Module "SharePointPnPPowerShell2019"
            Start-Sleep -s 2
        }
		if (Get-Module -ListAvailable -Name "SharePointPnPPowerShell2016") {
            Uninstall-Module "SharePointPnPPowerShell2016"
            Start-Sleep -s 2
        }
		if (Get-Module -ListAvailable -Name "SharePointPnPPowerShell2013") {
            Uninstall-Module "SharePointPnPPowerShell2013"
            Start-Sleep -s 2
        }
		
		Add-Log -Level "INFO" -Message "Checking and uninstalling PnP.Powershell module"
        if (Get-Module -ListAvailable -Name "PnP.PowerShell") {
            Uninstall-Module -Name "PnP.PowerShell"
            Start-Sleep -s 5
        }
		
		# Load needed libraries
        Add-Log -Level "INFO" -Message "Checking and installing SharePointPnPPowerShell module"
        Get-SharePointPnPPowerShell -SharePointVersion $config.sharePointVersion
		Start-Sleep -s 1
        # Connect
        Add-Log -Level "INFO" -Message "Connecting to SharePoint Tenant and configuring..."
			$ScriptPath = $PSScriptRoot + '\_AdditionalScript_ManageTenant.ps1'
			$procTenant = (Start-Process powershell.exe "$ScriptPath -sharePointTenantAdminUrl $sharePointTenantAdminUrl" -PassThru)
			$procTenant | Wait-Process 
        
        # If not site scope connect to app catalog
        if (!$config.onlineParams.useSiteScope) {
			 # Get json config file
			Add-Log -Level "INFO" -Message "Loading again configuration file"
			$isAbleToLoadConfigFile = Get-Config -JsonConfigFileName $configFileName
			if($isAbleToLoadConfigFile -eq $false)
			{
				Add-Log -Level "ERROR" -Message "Error when loading configuration file -> exit"
				exit
			}
			Add-Log -Level "INFO" -Message "Configuration file loaded"
			$config = $global:JsonConfig
			
			Start-Sleep -s 1
			$appCatalogUrl = $config.onlineParams.constantsOnline.appCatalogUrl
			Add-Log -Level "INFO" -Message "Connecting to Tenant App Catalog, adding app and configuring..."
			$ScriptPath = $PSScriptRoot + '\_AdditionalScript_ManageAppCatalog.ps1'
			$procAppCatalog = (Start-Process powershell.exe "$ScriptPath -appCatalogUrl $appCatalogUrl -sharePointTenantAdminUrl $sharePointTenantAdminUrl -piwikAdminUrl $piwikAdminUrl" -PassThru)
			$procAppCatalog | Wait-Process
        }

        # Apply piwik admin template
        Add-Log -Level "INFO" -Message "Connecting to Piwik PRO Administration site and configuring"
        Start-Sleep -s 1

		$ScriptPathPiwikAdmin = $PSScriptRoot + '\_AdditionalScript_PiwikAdminConfiguration.ps1'
		$procPA = (Start-Process powershell.exe "$ScriptPathPiwikAdmin -sharePointTenantAdminUrl $sharePointTenantAdminUrl -piwikAdminUrl $piwikAdminUrl" -PassThru)
		$procPA | Wait-Process
		
		if($config.onlineParams.includeAzureInstallation -eq $true)
		{
			Add-Log -Level "INFO" -Message "Running Azure resources installation..."
			$ScriptPathAzureScript = $PSScriptRoot + '\Create-AzureResources.ps1'
			$AzureTenant = $config.onlineParams.AzureTenant
			$AzureSubscription = $config.onlineParams.AzureSubscription
			$AzureResourceGroupName = $config.onlineParams.AzureResourceGroupName
			$AzureWebAppSuffix = $config.onlineParams.AzureWebAppSuffix
			$AzureLocation = $config.onlineParams.AzureLocation
			$procPAz = (Start-Process powershell.exe "$ScriptPathAzureScript -Tenant $AzureTenant -Subscription '$AzureSubscription' -ResourceGroupName $AzureResourceGroupName -WebAppSuffix $AzureWebAppSuffix -SharePointUrl $tenantUrl -Location $AzureLocation" -PassThru)
			$procPAz | Wait-Process
		}
    } 
    else 
    {
        Add-Log -Level "INFO" -Message "Setup is running sharepoint onPrem installation for version $config.sharePointVersion"

        # Load needed libraries
        Add-Log -Level "INFO" -Message "Importing Microsoft.SharePoint.PowerShell PSSnapin"
        Add-PSSnapin "Microsoft.SharePoint.PowerShell" -ErrorAction SilentlyContinue

        Add-Log -Level "INFO" -Message "Checking and uninstalling SharePointPnPPowerShellOnline module"
        if (Get-Module -ListAvailable -Name "SharePointPnPPowerShellOnline") {
            Uninstall-Module "SharePointPnPPowerShellOnline"
            Start-Sleep -s 5
        }
		
		Add-Log -Level "INFO" -Message "Checking and uninstalling PnP.Powershell module"
        if (Get-Module -ListAvailable -Name "PnP.PowerShell") {
            Uninstall-Module -Name "PnP.PowerShell"
            Start-Sleep -s 5
        }

        Add-Log -Level "INFO" -Message "Checking and installing SharePointPnPPowerShell module"
		
		if ($config.sharePointVersion -eq '2016') {
			$sp2016PnPVersion = Get-Module -ListAvailable -Name "SharePointPnPPowerShell2016" | Select-Object Version
			if($sp2016PnPVersion)
			{
				if($sp2016PnPVersion.Version.ToString() -ne "3.19.2003.0")
				{
					Add-Log -Level "INFO" -Message "Uninstalling SharePointPnPPowerShell2016 another version module and installing correct one"
					Uninstall-Module "SharePointPnPPowerShell2016"
					Start-Sleep -s 2
					Get-SharePointPnPPowerShell2016 -SharePointVersion $config.sharePointVersion
				}
			}
			else
			{
				Add-Log -Level "INFO" -Message "Installing SharePointPnPPowerShell2016 module"
				Get-SharePointPnPPowerShell2016 -SharePointVersion $config.sharePointVersion
			}
		}
		else
		{
			Get-SharePointPnPPowerShell -SharePointVersion $config.sharePointVersion
		}

        Add-Log -Level "INFO" -Message "Checking SharePoint Powershell Snapin"
        $snapin = Get-PSSnapin | Where-Object { $_.Name -eq 'Microsoft.SharePoint.Powershell' }
        if ($null -eq $snapin) {
            Add-Log -Level "INFO" -Message "Loading SharePoint Powershell Snapin"
            Add-PSSnapin "Microsoft.SharePoint.Powershell"
        }
		
		# Checking if is managed path "/sites/"
		Add-Log -Level "INFO" -Message "Checking if sites is in managed path."
		$isSitesInManagedPath = $false
		$managedPaths = Get-SPManagedPath -WebApplication $config.sharePointUrl
		$managedPaths | ForEach-Object {
			if($_.Name.toString() -eq "sites")
			{
				$isSitesInManagedPath = $true
			}
		}

		if($isSitesInManagedPath)
		{
			Add-Log -Level "INFO" -Message "Sites is in managed path already."
		}
		else
		{
			Add-Log -Level "INFO" -Message "Adding sites to the managed path."
			New-SPManagedPath -WebApplication $config.sharePointUrl -RelativeURL "sites"
		}

				$tenantAdminUrl = ([System.Uri]$config.sharePointUrl).GetLeftPart([System.UriPartial]::Authority);
			$sharePointTenantAdminUrl = $tenantAdminUrl + $config.onPremParams.constantsOnPrem.tenantAdminServerRelativeUrl;

        # Checking if tenant admin site exists
        Add-Log -Level "INFO" -Message "Checking tenant admin site"
        if ($null -eq (Get-SPWeb $sharePointTenantAdminUrl -ErrorAction SilentlyContinue)) {
            Add-Log -Level "INFO" -Message "Creating tenant site..."

            New-SPSite $sharePointTenantAdminUrl -OwnerAlias $config.sharepointAdminLogin -Template "STS#0"
            Start-Sleep -s 15
        }
        
        # Connect
        Add-Log -Level "INFO" -Message "Connecting to Tenant Admin site..."
        Connect-PnPOnline -Url $sharePointTenantAdminUrl -Credentials $credentials -ErrorAction Stop -WarningAction Ignore

        # Getting current user
        Add-Log -Level "INFO" -Message "Getting current user"
        $currentUser = (Get-PnPProperty (Get-PnPWeb) -Property CurrentUser).LoginName
        $currentUser = $currentUser -replace 'i:0#.f\|membership\|'

        # Publish Spfx for SP 2019
        if ($config.sharePointVersion -eq "2019") {
		
		    $tenantUrl = ([System.Uri]$config.sharePointUrl).GetLeftPart([System.UriPartial]::Authority);
			$appCatalogUrl = $tenantUrl + $config.onPremParams.constantsOnPrem.appCatalogUrl;
			
			Connect-ToSharePoint -Url $appCatalogUrl -TenantAdminUrl $SharePointTenantAdminUrl -Credentials $credentials
		
            Add-Log -Level "INFO" -Message "Publishing SPFx package to App Catalog"
            if (-not $UseSiteScope) {
                Add-PnPApp -Path $config.constants.spfxPackagePath2019 -Scope Tenant -Overwrite -Publish -SkipFeatureDeployment | Out-Null
            }
            else {
                Add-PnPApp -Path $config.constants.spfxPackagePath2019 -Scope Site -Overwrite -Publish -SkipFeatureDeployment | Out-Null
            }
        }
        
        # Creating piwik admin site
        $site = get-spsite -Identity $sharePointTenantAdminUrl
        $site.AdministrationSiteType = [Microsoft.SharePoint.SPAdministrationSiteType]::TenantAdministration
        $root = $site.rootweb
        try {
            $root.AllProperties["__WebTemplates"] = ""
            $root.Update()
        }
        catch {
            Add-Log -Level "INFO" -Message "Web templates already removed"
        }

        Add-Log -Level "INFO" -Message "Successfully added admin site type property to the site collection tenant admin"
        
        if ($null -eq (Get-SPWeb $piwikAdminUrl -ErrorAction SilentlyContinue)) {
            Add-Log -Level "INFO" -Message "Creating Piwik PRO Admin site"

			if ($config.sharePointVersion -eq "2019") {
			$sc = New-SPSite -Url $piwikAdminUrl -OwnerAlias $config.sharepointAdminLogin -Template "STS#3"
			}
			if ($config.sharePointVersion -in "2013", "2016") {
			$sc = New-SPSite -Url $piwikAdminUrl -OwnerAlias $config.sharepointAdminLogin -Template "STS#0"
			}

            $w = $sc.RootWeb

            $userA = $w.EnsureUser($config.sharepointAdminLogin)
            $w.CreateDefaultAssociatedGroups($userA, $null, $null)	
        }

        Disconnect-PnPOnline;
		
        Start-Sleep -s 15
		# Enabling JSON upload
        Add-Log -Level "INFO" -Message "Adding possibility to upload JSON";
        EnableJSONLight

        $WebApp = Get-SPWebApplication $SharePointUrl
        $Extensions = $WebApp.BlockedFileExtensions
        try {
            $Ext = $Extensions.Remove("json")
            if ($Ext -eq $true) {
                Add-Log -Level "INFO" -Message "Filetype $($Extension) has been removed from Web Application $(($WebApp).Name)"
            }
            else {
                Add-Log -Level "INFO" -Message "Unable to delete filetype $($Extension) from Web Application $(($WebApp).Name) probably it has been removed previously"
            }
        }
        catch {
            Add-Log -Level "INFO" -Message "Json format has been removed previously"
        }
        $WebApp.Update()
		Start-Sleep -s 3
		
		# Apply piwik admin template
        Add-Log -Level "INFO" -Message "Connecting to Piwik PRO Administration site"
        Connect-ToSharePoint -Url $piwikAdminUrl -TenantAdminUrl $sharePointTenantAdminUrl -Credentials $credentials

        Add-Log -Level "INFO" -Message "Applying Piwik PRO Administration site template"
        Apply-PnPProvisioningTemplate -Path $config.constants.templatePath -TemplateId "PIWIK-ADMIN-TEMPLATE" -Parameters @{"SharePointUrl" = $tenantUrl; "PiwikAdminServerRelativeUrl" = $config.constants.piwikAdminServerRelativeUrl; "Owner" = $currentUser }
		
		Add-Log -Level "INFO" -Message "Checking if Everyone user has read permission."
		try
		{
			$isEveryoneAdded = $false
		   $usersAdded = Get-PnPUser -WithRightsAssigned
		   foreach($ua in $usersAdded) {
			   if($ua.Title -eq "Everyone")
			   {
					$isEveryoneAdded = $true
					 Add-Log -Level "INFO" -Message "Everyone user has already read permission."
			   }
		   }

		   if($isEveryoneAdded -eq $false)
		   {
			Add-Log -Level "INFO" -Message "Adding Everyone user to PiwikAdmin permissions"
			Set-PnPWebPermission -User 'c:0(.s|true' -AddRole 'Read' 
		   }
	   	}
		catch
		{
			$ErrorMessage = $_.Exception.Message
			Add-Log -Level "ERROR" -Message $ErrorMessage
		}
		
		$additionalPiwikAdmin = $config.additionalPiwikSiteCollAdmin
		if($additionalPiwikAdmin)
		{
			Add-Log -Level "INFO" -Message "Adding additional sitecollection admins to PiwikAdmin site."
			try
			{
				
				Add-PnPSiteCollectionAdmin -Owners $additionalPiwikAdmin
			}
			catch
			{
				$ErrorMessage = $_.Exception.Message
				Add-Log -Level "ERROR" -Message $ErrorMessage
			}
		}

        # Adding PiwikConfig list items
        Add-Log -Level "INFO" -Message "Adding items to PiwikConfig list";
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
        Add-Log -Level "INFO" -Message "Adding Property Bag Settings";
        Set-PnPPropertyBagValue -Key "piwik_jobversion" -Value $config.onPremParams.constantsOnPrem.webJobVersion
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
                Add-Log -Level "INFO" -Message "Containers url property bag is already exists"
            }
        }

		$folderExists = Resolve-PnPFolder -SiteRelativePath "Style Library/PROD"

		if(!$folderExists)
		{
			Add-PnPFolder -Name PROD -Folder "Style Library"
		}
        Start-Sleep -s 2
        Disconnect-PnPOnline;

        # Adding solutions
        Add-Log -Level "INFO" -Message "Adding solution folder";

        $spSite = New-Object Microsoft.SharePoint.SPSite($piwikAdminUrl)
        $spWeb = $spSite.OpenWeb()
		
            Copy-Item -Path "$($config.constants.filesSolutionFolder)PROD_OnPrem\" -Destination "$($config.constants.filesSolutionFolder)PROD\" -Recurse -force
        
        if ($config.sharePointVersion -eq '2013') {
            Copy-Item -Path "$($config.constants.filesSolutionFolder)PROD\piwik-config-onprem-2013.json" -Destination "$($config.constants.filesSolutionFolder)PROD\piwik-config.json" -Recurse -force
        }
        
        if ($config.sharePointVersion -eq '2016') {
            Copy-Item -Path "$($config.constants.filesSolutionFolder)PROD\piwik-config-onprem-2016.json" -Destination "$($config.constants.filesSolutionFolder)PROD\piwik-config.json" -Recurse -force
        }
		
		if ($config.sharePointVersion -eq '2019') {
            Copy-Item -Path "$($config.constants.filesSolutionFolder)PROD\piwik-config-onprem-2019.json" -Destination "$($config.constants.filesSolutionFolder)PROD\piwik-config.json" -Recurse -force
        }
		
        UploadFiles -siteUrl $piwikAdminUrl -DestFolderUrl ($piwikAdminUrl + "/Style%20Library/PROD") -LocalFileOrFolderPath ($config.constants.filesSolutionFolder+"PROD\")

        # Install package
        try {
		$MywspName = $config.onPremParams.constantsOnPrem.MywspName2013
			if ($config.sharePointVersion -eq '2019') {
				$MywspName = $config.onPremParams.constantsOnPrem.MywspName2019
			}

            $MywspFullPath = $PSScriptRoot + $config.onPremParams.constantsOnPrem.wspSolutionPath + $MywspName

            $MyInstalledSolution = Get-SPSolution | Where-Object Name -eq $MywspName
            
            if ($MyInstalledSolution -ne $null) {
                if ($MyInstalledSolution.Deployed.Count -gt 0) {
                    Add-Log -Level "INFO" -Message "Updating solution..."
                    Update-SPSolution –Identity $MywspName –LiteralPath $MywspFullPath -GACDeployment:$true -FullTrustBinDeployment:$true -Force:$true
                    wait4timer($MywspName)
                    
                    iisreset
                    net stop "Sharepoint Timer Service"
                    net start "Sharepoint Timer Service"
                }
                else {
                    wait4timer($MywspName) 
                    Remove-SPSolution $MywspName -Confirm:$false 
                    Add-Log -Level "INFO" -Message "Removed the Solution from the Farm"
                    sleep 3
                    
                    Add-SPSolution -LiteralPath "$MywspFullPath"
                    sleep 3
                    install-spsolution -Identity $MywspName -FullTrustBinDeployment:$true -GACDeployment:$true -Force:$true
                    wait4timer($MywspName)
                    
                    iisreset
                    net stop "Sharepoint Timer Service"
                    net start "Sharepoint Timer Service"				
                }
            }
            else {
                wait4timer($MywspName) 
                Add-SPSolution -LiteralPath "$MywspFullPath"
                sleep 3
                install-spsolution -Identity $MywspName -FullTrustBinDeployment:$true -GACDeployment:$true -Force:$true
                wait4timer($MywspName)    
            }
            
            Add-Log -Level "INFO" -Message "Successfully Deployed to the Farm"
            
        }
        catch {
            Add-Log -Level "ERROR" -Message "Exception Occured on DeployWSP"
        }
        
        try {
            Start-Sleep -s 5
            Add-Log -Level "INFO" -Message "Enabling job feature"
            ActivateFeatureInWebApplicationScope -DisplayName "fb5decb7-5b5d-49c2-9090-4133b8e80e5e" -webApplicationUrl $config.sharePointUrl
        }
        catch {
            Add-Log -Level "ERROR" -Message "Exception Occured on job feature activation"
        }
            
        try {
            Start-Sleep -s 15
            Add-Log -Level "INFO" -Message "Configuring timer job..."
            #Add property to job of piwik admin url
            $job = Get-SPTimerJob $config.onPremParams.constantsOnPrem.timerJobName -WebApplication $config.sharePointUrl
            Start-Sleep -s 2
            $job.Properties.Add("piwik_adminsiteurl", $piwikAdminUrl)
            Start-Sleep -s 2
            $job.Update()
        }
        catch {
            Add-Log -Level "ERROR" -Message "Exception Occured on job adding property"
        }

        try {
            Add-Log -Level "INFO" -Message "Enabling other features"
            #Enable push notification service
            ActivateFeatureInSiteCollectionScope -DisplayName "41e1d4bf-b1a2-47f7-ab80-d5d6cbba3092" -siteurl $piwikAdminUrl
        }
        catch {
            Add-Log -Level "ERROR" -Message "Exception Occured on enabling features"
        }
        
        if ($config.onPremParams.activateFeatureStapplerOnDefault -eq $true) {
            try {
                Start-Sleep -s 5
                Add-Log -Level "INFO" -Message "Enabling stappler feature"
                ActivateFeatureInWebApplicationScope -DisplayName "a07b809f-3b99-4177-90b7-181c33b11c92" -webApplicationUrl $config.sharePointUrl
            }
            catch {
                Add-Log -Level "ERROR" -Message "Exception Occured on job feature activation"
            }
        }
        
        if ($config.onPremParams.activateTrackerOnOldConnectorSites -eq $true) {
            Add-Log -Level "INFO" -Message "Enabling tracker on sites where old connector was enabled"
            try {
                Start-Sleep -s 5
                $SPWebApp = Get-SPWebApplication $config.sharePointUrl
                
                $webPiwikAdmin = Get-SPWeb -Identity $piwikAdminUrl  
                $prositeDirList = $webPiwikAdmin.Lists["Piwik PRO Site Directory"]    

                foreach ($SPSite in $SPWebApp.Sites) {
                    if ($SPSite -ne $null) {
                        if ($SPSite.RootWeb.Properties["piwik_metasitenamestored"]) {
                            Add-Log -Level "INFO" -Message "Activating tracker on site: " $SPSite.url
                            try {
                                $SPSite.RootWeb.Properties["piwik_istrackingactive"] = "true"
                                $SPSite.RootWeb.Properties.Update()
                            }
                            catch {
                                Add-Log -Level "ERROR" -Message "Problem with set property bag on site: " $SPSite.url
                            }
                            
                            try {
                                Add-Log -Level "INFO" -Message "Enabling tracking feature"
                                ActivateFeatureInSiteCollectionScope -DisplayName "274e477e-287d-4b22-a411-c691e999379f" -siteurl $SPSite.url
                                Start-Sleep -s 1
                            }
                            catch {
                                Add-Log -Level "ERROR" -Message "Exception Occured on enabling feature"
                            }
                            
                            try {
                                Add-Log -Level "INFO" -Message "Adding entry on Piwik PRO Site Directory list"
                                $newItem = $prositeDirList.items.add()
                                $newitem["Title"] = $SPSite.RootWeb.Title
                                $newitem["pwk_url"] = $SPSite.url
                                $newitem["pwk_siteId"] = $SPSite.RootWeb.Properties["piwik_metasitenamestored"]
                                $newitem["pwk_status"] = "Active"
                                $newitem.update()
                            }
                            catch {
                                Add-Log -Level "ERROR" -Message "Exception Occured on enabling feature"
                            }

                            Add-Log -Level "INFO" -Message "Tracker activated on site." -ForegroundColor Green
                        
                        }
                        $SPSite.Dispose()
                    }
                }
                
            }
            catch {
                Add-Log -Level "ERROR" -Message "Exception Occured on activate tracker on old sites"
            }
        }
    }

    Add-Log -Level "INFO" -Message "Setup finished"
}
Catch
{
    Add-Log -Level "INFO" -Message "Setup error"
    $ErrorMessage = $_.Exception.Message
    Add-Log -Level "ERROR" -Message $ErrorMessage
}
Finally
{
    Add-Log -Level "INFO" -Message "===========END==========="
	Stop-Transcript
}