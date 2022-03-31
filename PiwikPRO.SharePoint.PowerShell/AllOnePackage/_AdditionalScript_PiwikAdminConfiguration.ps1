Param (
	[Parameter(Position = 0, Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$sharePointTenantAdminUrl,
	
	[Parameter(Position = 1, Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$piwikAdminUrl
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
    #Import-Module '.\setupLibrary\setupLibrary.psd1' -Force -ErrorAction Stop

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