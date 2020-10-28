Param (
    [Parameter(Position = 0, Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$SharePointUrl,

    [Parameter(Position = 1, Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$Owner,

    [Parameter(Position = 2)]
    [string]$SharePointTenantAdminUrl,

    [Parameter(Position = 3)]
    [ValidateSet('2013', '2016', '2019', 'Online')]
    [string]$SharePointVersion = 'Online',

    [Switch]$UseWebLogin
)

# for Sharepoint OnPremise
$IsNewInstallation = $true
$sharepointAdminLogin = "piwik\lkolodziejczak"
$clientIdValue = "7VzT7CvAs9UFwDXTfEFhXsJHOPjFiVKM"
$clientSecretValue = "yaOAuNhEkD8sHC4F3gOzftaDZnD8nlwbpBR0Cc2xJjCLiUGb8Ciz7ljPy2C3mAlL0ObHgNwxdrwZPlui"
$serviceUrlValue ="https://kogifi.piwik.pro"
$wspSolutionPath = ".\solutions\";
$filesSolutionFolder = ".\solutions\build\";
$filesImagesdFolder = ".\assets\";
$MywspName = "PiwikPRO.SharePoint.SP2013.wsp"
$timerJobName = "Piwik PRO Job"


$templatePath = ".\templates\PiwikPROTemplate.xml";
$spfxPackagePath = ".\solutions\piwikpro-sharepoint.sppkg";
$piwikAdminServerRelativeUrl = "/sites/PiwikAdmin";
$piwikAdminUrl = $SharePointUrl + $piwikAdminServerRelativeUrl;

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

function Load-Module {
	Param([string]$name, [string]$version)
	if(-not(Get-Module -name $name)) {
		if(-not(Get-Module -ListAvailable | Where-Object { $_.name -eq $name -and $_.Version -eq $version})) {
			Write-Host "Required module $name with version $version is missing"
			Write-Host "Installing module $name version $version"
			Install-Module -Name $name -RequiredVersion $version -AllowClobber
		} else{
			Write-Host "Module exists $name with version $version"
			Start-sleep -milliseconds 3000
		}
	}
	Write-Host "Importing module $name with version $version"
	Import-Module $name -RequiredVersion $version
}

# ensures that specified module with minimm version is loaded
function Ensure-Module {
    Param(
        [string]$Name,
        [string]$MinimumVersion
    )

    $module = Get-Module -name $name
    if (-not($module) -or -not(($module.Version -ge [Version]$minimumversion))) {
        if (-not(Get-Module -ListAvailable | Where-Object { $_.name -eq $name -and $_.Version -ge [Version]$minimumversion})) {
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

function UploadFiles($siteUrl, $DestFolderUrl, $LocalFileOrFolderPath)
{
	try
	{
	   if((get-item $LocalFileOrFolderPath).PSIsContainer -eq $true)
	   {
		 write-host "Looping Items in Folder: $LocalFileOrFolderPath"
		 Get-ChildItem $LocalFileOrFolderPath | Foreach-Object{
			$name = $_.Name
			$fullName = $_.FullName
			if((get-item $fullName).PSIsContainer -eq $false){ #only files, not folders
					UploadandApprove -siteUrl $siteUrl -DestFolderUrl $DestFolderUrl -LocalFilePath $fullName
				}
				else{
					createSPFolder -siteUrl $siteUrl -DestFolderUrl $DestFolderUrl -LocalFolderName $name | Out-Null
					UploadFiles -siteUrl $siteUrl -DestFolderUrl ($DestFolderUrl + "/" +$name) -LocalFileOrFolderPath $fullName #recursively iterate 
				}
		 
		  }
	   }
	   else
	   {
		 UploadandApprove -siteUrl $siteUrl -DestFolderUrl $DestFolderUrl -LocalFilePath $_.FullName
	   }
    }
    catch
    {
        write-host -ForegroundColor Red "Exception Occurred:"
       Write-Host -ForegroundColor Red $_.Exception.Message
       Write-Host -ForegroundColor Red $_.Exception.stacktrace
    } 
    finally
    {
        $spSite.Dispose()
        $spWeb.Dispose()
    }
}

function createSPFolder($siteUrl,  $DestFolderUrl, $LocalFolderName)
{
    
    write-host "Creating SPFolder: " $LocalFolderName
    ############################################# Upload ###########################################
    try    
    {  
        $destfolder= $spWeb.GetFolder($DestFolderUrl)
        $destfolder.SubFolders.Add($LocalFolderName)
        $destfolder.update();
    }
    catch
    {
        write-host -ForegroundColor Red "Exception Occurred:"
       Write-Host -ForegroundColor Red $_.Exception.Message
       Write-Host -ForegroundColor Red $_.Exception.stacktrace
    } 
}

function UploadandApprove($siteUrl, $DestFolderUrl, $LocalFilePath)
{

    write-host "Uploading " $LocalFilePath
    ############################################# Upload ###########################################
    try    
    {  
        $destfolder= $spWeb.GetFolder($DestFolderUrl) 
        $spList = $destfolder.Item.ParentList            
        $spFiles = $destfolder.Files;  

         $fileInfo = Get-Item $LocalFilePath
		 $fileStream = ([System.IO.FileInfo] (Get-Item $LocalFilePath)).OpenRead()
         $fileName = $fileInfo.Name
         $ExistingFile=$spFiles[$fileName]
         if($ExistingFile.Exists -and $spList.EnableVersioning -eq $true)
         {
            #check out
            $ExistingFile.CheckOut()    
         }
 
         #update file
		try    
		{  
         $spFile = $spFiles.Add($fileName,$fileStream,$true);
		}
		catch{}
             try    
			{  
				if($spFile.CheckOutType  -ne [Microsoft.SharePoint.SPFile+SPCheckOutType]::None)
				{
					$spFile.CheckIn("")    
				}

				if($spList.EnableMinorVersions -eq $true)
				{
					$spFile.publish("");
				}
			}
		catch{}

        ########################### Approve Items ############################################
             try    
			{  
        $destfolder= $spWeb.GetFolder($DestFolderUrl) 
        $spList = $destfolder.Item.ParentList    
        $spFiles = $destfolder.Files;  
 
         $fileInfo = Get-Item $LocalFilePath
         $fileName = $fileInfo.Name
         $ExistingFile=$spFiles[$fileName]
         if($ExistingFile.Exists -and $spList.EnableModeration -eq $true)
         {
            $ExistingFile.Approve('')
    
         } 			
		 }
		catch{}
         #Dispose of Web object
    }
    catch
    {
        write-host -ForegroundColor Red "Exception Occurred:"
       Write-Host -ForegroundColor Red $_.Exception.Message
       Write-Host -ForegroundColor Red $_.Exception.stacktrace
    } 
}

function EnableJSONLight()
{
	$configOwnerName = "JSONLightDependentAssembly"
	$spWebConfigModClass ="Microsoft.SharePoint.Administration.SPWebConfigModification"

	$dependentAssemblyPath ="configuration/runtime/*[local-name()='assemblyBinding' and namespace-uri()='urn:schemas-microsoft-com:asm.v1']"
	$dependentAssemblyNameStart ="*[local-name()='dependentAssembly'][*/@name='"
	$dependentAssemblyNameEnd = "'][*/@publicKeyToken='31bf3856ad364e35'][*/@culture='neutral']"
	$dependentAssemblyValueStart = "<dependentAssembly><assemblyIdentity name='"
	$dependentAssemblyValueEnd ="' publicKeyToken='31bf3856ad364e35' culture='neutral' /><bindingRedirect oldVersion='5.0.0.0' newVersion='5.6.0.0' /></dependentAssembly>"
	$edmAssemblyName ="Microsoft.Data.Edm"
	$odataAssemblyName ="Microsoft.Data.Odata"
	$dataServicesAssemblyName ="Microsoft.Data.Services"
	$dataServicesClientAssemblyName ="Microsoft.Data.Services.Client"
	$spatialAssemblyName ="System.Spatial"
	$assemblyNamesArray = $edmAssemblyName,$odataAssemblyName,$dataServicesAssemblyName,$dataServicesClientAssemblyName, $spatialAssemblyName
	$webService = [Microsoft.SharePoint.Administration.SPWebService]::ContentService

	################ Adds individual assemblies ####################
	For ($i=0; $i -lt 5; $i++)
	{
		echo "Adding Assembly..."$assemblyNamesArray[$i]
		$dependentAssembly = New-Object $spWebConfigModClass
		$dependentAssembly.Path=$dependentAssemblyPath
		$dependentAssembly.Sequence =0 # First item to be inserted
		$dependentAssembly.Owner = $configOwnerName
		$dependentAssembly.Name =$dependentAssemblyNameStart + $assemblyNamesArray[$i] + $dependentAssemblyNameEnd
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

function wait4timer($solutionName) 
 {    
     $solution = Get-SPSolution | where-object {$_.Name -eq $solutionName}    
     if ($solution -ne $null)     
     {        
         Write-Host "Waiting to finish soultion timer job"
         while ($solution.JobExists -eq $true )          
         {               
             Write-Host "Please wait...Either a Retraction/Deployment is happening"       
             sleep 2            
         }                
         Write-Host "Finished the solution timer job"
     }
 }  


if ((-not $SharePointTenantAdminUrl) -and $SharePointVersion -eq 'Online') {
    $SharePointTenantAdminUrl = $SharePointUrl.Replace(".sharepoint.com", "-admin.sharepoint.com");
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
if ($SharePointVersion -eq 'Online') {
	Connect-ToSharePoint -Url $SharePointUrl -TenantAdminUrl $SharePointTenantAdminUrl -Credentials $credentials -UseWebLogin:$UseWebLogin;
}

if ($SharePointVersion -eq '2013') {
	if(!((Get-SPWeb $SharePointTenantAdminUrl -ErrorAction SilentlyContinue) -ne $null)){
		Write-Host "Creating tenant site..."

		New-SPSite $SharePointTenantAdminUrl -OwnerAlias $Owner -Template "STS#0"
	}
	
	Write-Host "Connecting to Tenant Admin site..."
	Connect-PnPOnline -Url $SharePointTenantAdminUrl -Credentials $credentials -ErrorAction Stop
}

if ($SharePointVersion -eq 'Online') {
    Write-Host "Ensuring that Tenant App Catalog exists...";
    Register-PnPAppCatalogSite -Url "$SharePointUrl/sites/AppCatalog" -Owner $Owner -TimeZoneId 4 -ErrorAction SilentlyContinue | Out-Null;
}

if (-not ($SharePointVersion -in "2013", "2016")) {
    Write-Host "Publishing SPFx package to Tenant App Catalog...";
    Add-PnPApp -Path $spfxPackagePath -Scope Tenant -Overwrite -Publish -SkipFeatureDeployment | Out-Null;
}

if ($SharePointVersion -eq 'Online') {
    $appCatalogUrl = Get-PnPTenantAppCatalogUrl;

    Write-Host "Connecting to Tenant App Catalog...";
    Disconnect-PnPOnline;
    Connect-ToSharePoint -Url $appCatalogUrl -TenantAdminUrl $SharePointTenantAdminUrl -Credentials $credentials -UseWebLogin:$UseWebLogin;

    Write-Host "Configuring tenant-wide extensions...";
    Apply-PnPProvisioningTemplate -Path $templatePath -TemplateId "PIWIK-TENANT-WIDE";


Write-Host "Checking if Piwik PRO Administration site already exists...";
if (-not (Get-PnPTenantSite -Url $piwikAdminUrl -ErrorAction SilentlyContinue)) {
    Write-Host "Piwik PRO Administration site doesn't exist. Creating...";
    New-PnPTenantSite -Title "Piwik PRO Administration" -Url $piwikAdminUrl -Owner $Owner -TimeZone 4 -Lcid 1033 -Template "STS#3" -Wait -Force | Out-Null;
}
}

if ($SharePointVersion -eq '2013') {
	$snapin = Get-PSSnapin | Where-Object {$_.Name -eq 'Microsoft.SharePoint.Powershell'}
	if ($snapin -eq $null)
	{
		Write-Host "Loading SharePoint Powershell Snapin"
		Add-PSSnapin "Microsoft.SharePoint.Powershell"
	}

	$site = get-spsite -Identity $SharePointTenantAdminUrl
	$site.AdministrationSiteType = [Microsoft.SharePoint.SPAdministrationSiteType]::TenantAdministration
	$root = $site.rootweb
    try{
	    $root.AllProperties["__WebTemplates"] = ""
	    $root.Update()
    }
    catch
    {
        Write-Host "Web templates already removed"
    }

	Write-Host "Successfully added admin site type property to the site collection tenant admin"
	
		if(!((Get-SPWeb $piwikAdminUrl -ErrorAction SilentlyContinue) -ne $null)){
		Write-Host "Creating Piwik PRO Admin site..."

			$sc = New-SPSite -Url $piwikAdminUrl -OwnerAlias $Owner -Template "STS#0"
			$w=$sc.RootWeb
			$w.CreateDefaultAssociatedGroups($sharepointAdminLogin,$null,$null)
			
		}
}

Write-Host "Connecting to Piwik PRO Administration site...";
Disconnect-PnPOnline;
Start-Sleep -s 5
Connect-ToSharePoint -Url $piwikAdminUrl -TenantAdminUrl $SharePointTenantAdminUrl -Credentials $credentials -UseWebLogin:$UseWebLogin;

Write-Host "Applying Piwik PRO Administration site template...";
Apply-PnPProvisioningTemplate -Path $templatePath -TemplateId "PIWIK-ADMIN-TEMPLATE" -Parameters @{"SharePointUrl" = $SharePointUrl; "PiwikAdminServerRelativeUrl" = $piwikAdminServerRelativeUrl; "Owner" = $Owner };

if ($SharePointVersion -eq '2013') {
Write-Host "Adding items to PiwikConfig list";
$listitem1Get = Get-PnPListItem -List "PiwikConfig" -Query "<View><Query><Where><Eq><FieldRef Name='Title'/><Value Type='Text'>piwik_clientid</Value></Eq></Where></Query></View>"
if($listitem1Get)
{
	Set-PnPListItem -List "PiwikConfig" -Identity $listitem1Get -Values @{"Title" = "piwik_clientid"; "Value"=$clientIdValue}
}
else
{
	$listItem1 = Add-PnPListItem -List "PiwikConfig" -Values @{"Title" = "piwik_clientid"; "Value"=$clientIdValue}
}

$listitem2Get = Get-PnPListItem -List "PiwikConfig" -Query "<View><Query><Where><Eq><FieldRef Name='Title'/><Value Type='Text'>piwik_clientsecret</Value></Eq></Where></Query></View>"
if($listitem2Get)
{
	Set-PnPListItem -List "PiwikConfig" -Identity $listitem2Get -Values @{"Title" = "piwik_clientsecret"; "Value"=$clientSecretValue}
}
else
{
	$listItem2 = Add-PnPListItem -List "PiwikConfig" -Values @{"Title" = "piwik_clientsecret"; "Value"=$clientSecretValue}
}

$listitem3Get = Get-PnPListItem -List "PiwikConfig" -Query "<View><Query><Where><Eq><FieldRef Name='Title'/><Value Type='Text'>piwik_serviceurl</Value></Eq></Where></Query></View>"
if($listitem3Get)
{
	Set-PnPListItem -List "PiwikConfig" -Identity $listitem3Get -Values @{"Title" = "piwik_serviceurl"; "Value"=$serviceUrlValue}
}
else
{
	$listItem3 = Add-PnPListItem -List "PiwikConfig" -Values @{"Title" = "piwik_serviceurl"; "Value"=$serviceUrlValue}
}
}

Disconnect-PnPOnline;

if ($SharePointVersion -eq '2013') {
Write-Host "Adding possibility to upload JSON";

EnableJSONLight

$WebApp = Get-SPWebApplication $SharePointUrl
$Extensions = $WebApp.BlockedFileExtensions
try
{
$Ext = $Extensions.Remove("json")
if($Ext -eq $true){
Write-Host "Filetype $($Extension) has been removed from Web Application $(($WebApp).Name)"
}
else{
Write-Host "Unable to delete filetype $($Extension) from Web Application $(($WebApp).Name) probably it has been removed previously"
}
}
catch{
Write-Host "Json format has been removed previously"
}
$WebApp.Update()

	Write-Host "Adding solution folder";
	$siteUrl = $piwikAdminUrl

    $spSite = New-Object Microsoft.SharePoint.SPSite($siteUrl)
    $spWeb = $spSite.OpenWeb()
	UploadFiles -siteUrl $piwikAdminUrl -DestFolderUrl ($piwikAdminUrl + "/Style%20Library") -LocalFileOrFolderPath $filesSolutionFolder

	UploadFiles -siteUrl $piwikAdminUrl -DestFolderUrl ($piwikAdminUrl + "/Style%20Library") -LocalFileOrFolderPath $filesImagesdFolder


#install package
    try
    {
        $MywspFullPath = $wspSolutionPath + $MywspName
 
        $MyInstalledSolution = Get-SPSolution | Where-Object Name -eq $MywspName
         
        if($MyInstalledSolution -ne $null)
        {
            if($MyInstalledSolution.DeployedWebApplications.Count -gt 0)
            {
                wait4timer($MywspName)  
                Uninstall-SPSolution $MywspName  -AllWebApplications:$true -confirm:$false
                wait4timer($MywspName)   
                Write-Host "Remove the Solution from the Farm" -ForegroundColor Green 
                Remove-SPSolution $MywspName -Confirm:$false 
                sleep 3
            }
            else
            {
                wait4timer($MywspName) 
                Remove-SPSolution $MywspName -Confirm:$false 
                sleep 3
            }
        }
 
        wait4timer($MywspName) 
        Add-SPSolution -LiteralPath "$MywspFullPath"
        install-spsolution -Identity $MywspName -FullTrustBinDeployment:$true -GACDeployment:$true -Force:$true
        wait4timer($MywspName)    
 
        Write-Host "Successfully Deployed to the Farm"
         
    }
    catch
    {
        Write-Host "Exception Occured on DeployWSP : $Error[0].Exception.Message"
    }
	
	try
    {
		Start-Sleep -s 5
		Write-Host "Enabling job feature"
		Enable-SPFeature -Identity fb5decb7-5b5d-49c2-9090-4133b8e80e5e -Url $SharePointUrl -Confirm:$false 
	}
    catch
    {
        Write-Host "Exception Occured on job feature activation : $Error[0].Exception.Message"
    }
		
	try
    {
		Start-Sleep -s 5
		Write-Host "Configuring timer job..."
		#Add property to job of piwik admin url
		$job = Get-SPTimerJob $timerJobName
		$job.Properties.Add("piwik_adminsiteurl", $piwikAdminUrl)
		$job.Update()
	}
    catch
    {
        Write-Host "Exception Occured on job adding property : $Error[0].Exception.Message"
    }

	try
    {
	Write-Host "Enabling other features"
	#Enable push notification service
	Enable-SPFeature -Identity 41e1d4bf-b1a2-47f7-ab80-d5d6cbba3092 -URL $piwikAdminUrl 
	}
    catch
    {
        Write-Host "Exception Occured on enabling features : $Error[0].Exception.Message"
    }
}

Write-Host "Finished." -ForegroundColor Green;