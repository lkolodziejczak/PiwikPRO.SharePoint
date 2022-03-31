Param (
	[Parameter(Position = 0, Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$sharePointTenantAdminUrl
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

#Get configuration from JSON file
function Get-Config([Parameter(Mandatory=$True)][string]$JsonConfigFileName)
{
	$IsAbleToLoadConfigFile = $true
	Try
    {
        Add-Log -Level "INFO" -Message "Getting and parsing config file"
		$global:JsonConfig = Get-Content ".\$JsonConfigFileName.json" | Out-String | ConvertFrom-Json -ErrorAction Stop
	}
	Catch
    {
        Add-Log -Level "INFO" -Message "Error Getting and parsing config file"
        $IsAbleToLoadConfigFile = $false
	}
	return $IsAbleToLoadConfigFile
}

#Method for logs
function New-LogFile(
    [Parameter(Mandatory=$True)][string]$FileName, 
    [Parameter(Mandatory=$True)][string]$FolderName)
{
	$isFolderPresent = Test-Path ".\$FolderName\logs"
	if($isFolderPresent -eq $false)
	{
		New-Item -Path ".\$FolderName\logs" -ItemType Directory
    }
    $FileName = $FileName + (Get-Date).toString('yyyyMMddHHmmss')
	$path = ".\$FolderName\logs\$FileName.log"
	$global:LogFile = New-Item -Path $path -ItemType File
}

# Add string log to file and output it to the console
function Add-Log(
		[Parameter(Mandatory=$False)][ValidateSet('INFO','WARN','ERROR')][String]$Level = 'INFO',
		[Parameter(Mandatory=$True)][string]$Message)
{
	$timeStamp = (Get-Date).toString('yyyy/MM/dd HH:mm:ss')
    $line = "$timeStamp $Level -> $Message"
    if($global:LogFile)
	{
        Add-Content $global:LogFile -Value $line -ErrorAction Stop
	}
	$color = "White"
	if($Level -eq 'WARN')
	{
		$color = "DarkYellow"
	}
	elseif ($Level -eq 'ERROR') 
	{
		$color = "DarkRed"
	}
	Write-Host $line -ForegroundColor $Color
}

#modules helper
function Load-Module([string]$name, [string]$version) 
{
    if (-not(Get-Module -name $name)) {
        if (-not(Get-Module -ListAvailable | Where-Object { $_.name -eq $name -and $_.Version -eq $version })) {
            Write-Host "Required module $name with version $version is missing"
            Write-Host "Installing module $name version $version"
            Install-Module -Name $name -RequiredVersion $version -AllowClobber
        }
        else {
            Write-Host "Module exists $name with version $version"
            Start-sleep -milliseconds 3000
        }
    }
    Write-Host "Importing module $name with version $version"
    Import-Module $name -RequiredVersion $version
}

# ensures that specified module with minimm version is loaded
function Ensure-Module([string]$name, [string]$minimumVersion) 
{
    $module = Get-Module -name $name
    if (-not($module) -or -not(($module.Version -ge [Version]$minimumversion))) {
        if (-not(Get-Module -ListAvailable | Where-Object { $_.name -eq $name -and $_.Version -ge [Version]$minimumversion })) {
            Write-Host "Required module $name with version $minimumversion (or higher) is missing"
            Write-Host "Installing module $name version $minimumversion"
            Install-Module -Name $name -RequiredVersion $minimumversion -AllowClobber -Force
        }
        else {
            Write-Host "Module exists $name with version $minimumversion (or higher)"
            Start-sleep -milliseconds 3000
        }
    }

    Write-Host "Importing module $name with minimumversion $minimumversion"
    Import-Module $name -MinimumVersion $minimumversion -DisableNameChecking
}

#enable json function
function EnableJSONLight() {
    $configOwnerName = "JSONLightDependentAssembly"
    $spWebConfigModClass = "Microsoft.SharePoint.Administration.SPWebConfigModification"

    $dependentAssemblyPath = "configuration/runtime/*[local-name()='assemblyBinding' and namespace-uri()='urn:schemas-microsoft-com:asm.v1']"
    $dependentAssemblyNameStart = "*[local-name()='dependentAssembly'][*/@name='"
    $dependentAssemblyNameEnd = "'][*/@publicKeyToken='31bf3856ad364e35'][*/@culture='neutral']"
    $dependentAssemblyValueStart = "<dependentAssembly><assemblyIdentity name='"
    $dependentAssemblyValueEnd = "' publicKeyToken='31bf3856ad364e35' culture='neutral' /><bindingRedirect oldVersion='5.0.0.0' newVersion='5.6.0.0' /></dependentAssembly>"
    $edmAssemblyName = "Microsoft.Data.Edm"
    $odataAssemblyName = "Microsoft.Data.Odata"
    $dataServicesAssemblyName = "Microsoft.Data.Services"
    $dataServicesClientAssemblyName = "Microsoft.Data.Services.Client"
    $spatialAssemblyName = "System.Spatial"
    $assemblyNamesArray = $edmAssemblyName, $odataAssemblyName, $dataServicesAssemblyName, $dataServicesClientAssemblyName, $spatialAssemblyName
    $webService = [Microsoft.SharePoint.Administration.SPWebService]::ContentService

    ################ Adds individual assemblies ####################
    For ($i = 0; $i -lt 5; $i++) {
        echo "Adding Assembly..."$assemblyNamesArray[$i]
        $dependentAssembly = New-Object $spWebConfigModClass
        $dependentAssembly.Path = $dependentAssemblyPath
        $dependentAssembly.Sequence = 0 # First item to be inserted
        $dependentAssembly.Owner = $configOwnerName
        $dependentAssembly.Name = $dependentAssemblyNameStart + $assemblyNamesArray[$i] + $dependentAssemblyNameEnd
        $dependentAssembly.Type = 0 #Ensure Child Node
        $dependentAssembly.Value = $dependentAssemblyValueStart + $assemblyNamesArray[$i] + $dependentAssemblyValueEnd
        $webService.WebConfigModifications.Add($dependentAssembly)
    }
    ###############################################################

    echo "Saving Web Config Modification"
    $webService.Update()
    $webService.ApplyWebConfigModifications()
    echo "Update Complete"
}

#onPrem install wsp helper
function Wait4timer($solutionName) {    
    $solution = Get-SPSolution | where-object { $_.Name -eq $solutionName }    
    if ($solution -ne $null) {        
        Write-Host "Waiting to finish soultion timer job"
        while ($solution.JobExists -eq $true ) {               
            Write-Host "Please wait...Either a Retraction/Deployment is happening"       
            sleep 2            
        }                
        Write-Host "Finished the solution timer job"
    }
}

#SP Helper and modules
function Connect-ToSharePoint(
	[Parameter(Position = 0, Mandatory = $true)][ValidateNotNullOrEmpty()][string]$Url,
	[Parameter(Position = 1)][string]$TenantAdminUrl,
	[Parameter(Position = 2)][PSCredential]$Credentials,
	[bool]$UseWebLogin = $false)
{
    if ($UseWebLogin) {
        Connect-PnPOnline -Url $Url -TenantAdminUrl $TenantAdminUrl -UseWebLogin -WarningAction Ignore
    }
    else {
        Connect-PnPOnline -Url $Url -TenantAdminUrl $TenantAdminUrl -Credentials $Credentials -WarningAction Ignore
    }
}

function Get-SharePointPnPPowerShell([Parameter(Mandatory = $true)][string]$SharePointVersion)
{
	if (-not (Get-Module -ListAvailable -Name "SharePointPnPPowerShell$($SharePointVersion)")) {
		Add-Log -Level "INFO" -Message "Installing PnP PowerShell $SharePointVersion"
		Install-Module "SharePointPnPPowerShell$($SharePointVersion)" -AllowClobber -Force
		return
	}

	Add-Log -Level "INFO" -Message "PnP PowerShell $SharePointVersion already installed"
}

function Get-SharePointPnPPowerShell2016([Parameter(Mandatory = $true)][string]$SharePointVersion)
{
	if (-not (Get-Module -ListAvailable -Name "SharePointPnPPowerShell$($SharePointVersion)")) {
		Add-Log -Level "INFO" -Message "Installing PnP PowerShell $SharePointVersion"
		Install-Module "SharePointPnPPowerShell$($SharePointVersion)" -RequiredVersion "3.19.2003.0" -AllowClobber -Force
		return
	}

	Add-Log -Level "INFO" -Message "PnP PowerShell $SharePointVersion already installed"
}

#END BLOCK of declaration methods

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
    #Import-Module '.\setupLibrary\setupLibrary.psd1' -Force -ErrorAction Stop

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