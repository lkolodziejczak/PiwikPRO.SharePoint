$sharePointUserName = "lkolodziejczak@kogifidev3.onmicrosoft.com"
$sharePointPassword = "Testp123@!"

#to test - https://kogifidev3.sharepoint.com/_layouts/15/role.aspx,
#https://kogifidev3.sharepoint.com/_layouts/15/user.aspx?obj=%7b339EB641-ADD1-4331-9644-3FFF7AD8D4C3%7d%2clist&List=%7b339EB641-ADD1-4331-9644-3FFF7AD8D4C3%7d


#Admin user or group
$UserToPermissionOnList = "test1@kogifidev3.onmicrosoft.com"
$GroupToPermissionOnList = "Communication site Visitors"

$SharePointUrl = "https://kogifidev3.sharepoint.com"

$TenatAdminUrl = "https://piwikpro.madeinpoint.com/sites/tenant"



#Not editable variables
$ListName = "Piwik Pro Site Directory"
$ListUrl = "/Lists/PiwikAdmin"
$SourcePermissionLevelName ="Full Control"
$TargetPermissionLevelName ="Full Control Without Delete"


#########################################################################################################################################
# Common
#########################################################################################################################################

function Load-Module {
	Param([string]$name, [string]$version)
	if(-not(Get-Module -name $name)) {
		if(-not(Get-Module -ListAvailable | Where-Object { $_.name -eq $name -and $_.Version -eq $version})) {
			Log-Warn ("Required module $name with version $version is missing")
			Log-Warn ("Installing module $name version $version")
			Install-Module -Name $name -RequiredVersion $version -AllowClobber
		} else{
			Log-Info ("Module exists $name with version $version")
			Start-sleep -milliseconds 3000
		}
	}
	Log-Info ("Importing module $name with version $version")
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
            Log-Warn ("Required module $name with version $minimumversion (or higher) is missing")
            Log-Warn ("Installing module $name version $minimumversion")
            Install-Module -Name $name -RequiredVersion $minimumversion -AllowClobber -Force
        }
        else {
            Log-Info ("Module exists $name with version $minimumversion (or higher)")
            Start-sleep -milliseconds 3000
        }
    }

    Log-Info ("Importing module $name with minimumversion $minimumversion")
    Import-Module $name -MinimumVersion $minimumversion -DisableNameChecking
}


#########################################################################################################################################
# Logging functions
#########################################################################################################################################

function Log-File {
	param (
		$msg
	)

	$msg = [System.DateTime]::Now.ToString("yyyy.MM.dd HH:mm:ss.ffff") + "`t" + $msg
	($msg) | Out-File $global:teamworkLogFile -Append
}

function Log-Info {
	param (
		$msg
	)

    Write-Host $msg -foregroundcolor green
    Log-File $msg
}

function Log-Warn {
	param (
		$msg
	)

    Write-Host $msg -foregroundcolor yellow
    Log-File $msg
}

function Log-Error {
	param (
		$msg
	)

    Write-Host $msg -foregroundcolor red
    Log-File $msg
}


function Print-Logo {

Write-Output( "______ _______ ________ _______ __  __      ______ ______ _______" )
Write-Output( "|   __ \_     _|  |  |  |_     _|  |/  |    |   __ \   __ \       |")
Write-Output( "|    __/_|   |_|  |  |  |_|   |_|     <     |    __/      <   -   |")
Write-Output( "|___|  |_______|________|_______|__|\__|    |___|  |___|__|_______|")
Write-Output( " ")
Write-Output( "Install Script")
                                                                   

}

function Print-InstallationSuccess {
	Write-Host " " -foregroundcolor Green
    Write-Host "          .-.  _                               " -foregroundcolor Green
    Write-Host "          | | / )                  " -foregroundcolor Green
    Write-Host "          | |/ /                               " -foregroundcolor Green
    Write-Host "         _|__ /_     Piwik PRO installed successfully!            " -foregroundcolor Green
    Write-Host "        / __)-' )                              " -foregroundcolor Green
    Write-Host "        \  `(.-')                 " -foregroundcolor Green
    Write-Host "         > ._>-'                               " -foregroundcolor Green
    Write-Host "        / \/                                   " -foregroundcolor Green
	Write-Host " " -foregroundcolor Green
}


#########################################################################################################################################
# Sharepoint
#########################################################################################################################################

function Ensure-PnPConnection {
    Param (
        $url
    )

    Log-Info ("Connecting to site " + $url)
    $connection = $null
    $connectionTries = 0
    do {
        $connectionTries++
        try {
			if ($global:MFAInUse) {
				Log-Info("Using MFA authentication")
				$connection = Connect-PnPOnline -Url $url -UseWebLogin -ReturnConnection -ErrorAction Stop
			} else {
				if ($global:sharePointCredentials -eq $null) {
					$cred = $null

					if ($global:sharePointUserName -and $global:sharePointPassword) {
						Log-Info ("Using username/password")

						$pw = ConvertTo-SecureString $($global:sharePointPassword) -AsPlainText -Force
						$cred = New-Object System.Management.Automation.PsCredential($($global:sharePointUserName),$pw)
					} elseif($global:sharePointCredentialStoreKey){
					Log-Info ("Using credentialStoreKey: " + $global:sharePointCredentialStoreKey)
					$cred = Ensure-Credentials $($global:sharePointCredentialStoreKey)
					} else{
					$cred = Ensure-Credentials $false "Connecting to SharePoint"
					}

					$global:sharePointCredentials = $cred
				}

				$connection = Connect-PnPOnline -Url $url -Credentials $($global:sharePointCredentials) -ErrorAction Stop
			}

			return $connection
        }
        catch {
            Log-Error ("Error connecting, please try again")
            Log-Error ($_.Exception.Message)
        }
    } while ($connectionTries -lt 3)

    if ($connectionTries -eq 3 -and $connection -eq $null) {
        Log-Error ("Something went wrong while connecting to SharePoint. Please ensure credentials and parameters.")
        throw "Error connecting site $url"
    }
}


#########################################################################################################################################
# Start script
#########################################################################################################################################

Print-Logo

# initialization
$logSuffix = [System.DateTime]::Now.ToString("yyyyMMddHHmmss")

$global:teamworkLogFile = [System.IO.Path]::Combine($PSScriptRoot, [System.String]::Format("teamwork_{0}.txt", $logSuffix))
$global:teamworkPnPLogFile = [System.IO.Path]::Combine($PSScriptRoot, [System.String]::Format("teamwork_pnplog_{0}.txt", $logSuffix))

# check PS version
if ($PSVersionTable.PSVersion.Major -le 5 -and $PSVersionTable.PSVersion.Minor -lt 1) {
	Log-Error ("Current PowerShell version is '" + $PSVersionTable.PSVersion + "'. Minimal supported version is 5.1")
	return
}

Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force

Load-Module -name "SharePointPnPPowerShellOnline" -version "3.14.1910.1"

Set-PnPTraceLog -On -LogFile $global:teamworkPnPLogFile -Level Debug

$global:sharePointCredentials = $null
$global:sharePointCredentialStoreKey = $sharePointCredentialStoreKey
$global:sharePointUserName = $sharePointUserName
$global:sharePointPassword = $sharePointPassword
$global:MFAInUse = $useMFA.IsPresent

# Connect to Sharepoint and insert Credentials [Sharepoint Online, 2019 2016, 2013]
Log-Info ("Connecting to Piwik PRO Admin site...")
Ensure-PnPConnection -url $SharePointUrl 

Try {
Log-Info ("Creating new role")
   $siteRoleDef = Get-PnPRoleDefinition -Identity $SourcePermissionLevelName
   $newRoleDef = Add-PnPRoleDefinition -RoleName $TargetPermissionLevelName -Clone $siteRoleDef -Exclude DeleteListItems
   }
Catch {
    write-host -f Red "Creating New Role!" $_.Exception.Message
}
Try
{
    Log-Info ("Breaking list inheritance")
    $spoList= Get-PnPList $ListName 
    $spoList.BreakRoleInheritance($false, $true)
    $spoList.Update()
    $spoList.Context.Load($spoList)
    $spoList.Context.ExecuteQuery()
}
Catch {
    write-host -f Red "Breaking list inheritance!" $_.Exception.Message
}
Try
{
   if(![string]::IsNullOrEmpty($UserToPermissionOnList))
   {
    Log-Info ("Adding user to role")
    Set-PnPListPermission -Identity $ListName -User $UserToPermissionOnList -AddRole $TargetPermissionLevelName
   }
}
Catch {
    write-host -f Red "Adding user to role!" $_.Exception.Message
}
Try
{
   if(![string]::IsNullOrEmpty($GroupToPermissionOnList))
   {
    Log-Info ("Adding group to role")
    Set-PnPListPermission -Identity $ListName -Group $GroupToPermissionOnList -AddRole $TargetPermissionLevelName
   }
}
Catch {
    write-host -f Red "Adding group to role!" $_.Exception.Message
}

Print-InstallationSuccess

Log-Info ("Permission of list has been set.") 

Log-Info ("Exit script")

