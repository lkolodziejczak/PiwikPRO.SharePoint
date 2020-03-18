Param (
        [string]$sharePointUserName,
        [string]$sharePointPassword,
        [Parameter(Position=0,Mandatory=$true)]
        [string]$sharePointUrl, # "https://kogifidev3.sharepoint.com"
        [Parameter(Position=1,Mandatory=$true)]
        [string]$newSiteName, # "PiwikAdmin"
        [Parameter(Position=2,Mandatory=$true)]
        [string]$filepath # = ".\PiwikProAdminAndConfigLists.xml"

    )
$global:sharePointCredentials = $null
$global:sharePointUserName = $sharePointUserName
$global:sharePointPassword = $sharePointPassword
$global:MFAInUse = $useMFA.IsPresent
$IsNewInstallation = $true
$newSiteNameUrl = "{0}/sites/{1}" -f $sharePointUrl, $newSiteName
$isTemplateFileExists = Test-Path $filepath -PathType Leaf
function Ensure-PnPConnection {
    Param (
        $url
    )

    $connection = $null
    $connectionTries = 0
    do {
        $connectionTries++
        try {
			if ($global:MFAInUse) {
				$connection = Connect-PnPOnline -Url $url -UseWebLogin -ReturnConnection -ErrorAction Stop
			} else {
				if ($global:sharePointCredentials -eq $null) {
					$cred = $null

					if ($global:sharePointUserName -and $global:sharePointPassword) {

						$pw = ConvertTo-SecureString $($global:sharePointPassword) -AsPlainText -Force
						$cred = New-Object System.Management.Automation.PsCredential($($global:sharePointUserName),$pw)
					}

					$global:sharePointCredentials = $cred
				}

				$connection = Connect-PnPOnline -Url $url -Credentials $($global:sharePointCredentials) -ErrorAction Stop
			}

			return $connection
        }
        catch {
        }
    } while ($connectionTries -lt 3)

    if ($connectionTries -eq 3 -and $connection -eq $null) {
        throw "Error connecting site $url"
    }
}
function CheckSiteExistence {
    Param (
    $url
    )
$site = ""
Try
{
    Write-Host "Checking, if site already exists..."
    $site = Get-PnPTenantSite -Url $url -ErrorAction SilentlyContinue
}
Catch
{    
}
if ($site -ne $null)
{
    Write-Host "Site already exists, use existing..." -ForegroundColor Yellow
    return $true
}
else
{
    Write-Host "Site doesn't exist, creating new..." -ForegroundColor Green
    return $false
}
}
if ($isTemplateFileExists -eq $true) {
Ensure-PnPConnection -url $sharePointUrl
$siteExist = CheckSiteExistence -url $newSiteNameUrl
if($siteExist -eq $false) {
    New-PnPSite -Type CommunicationSite -Title "Piwik Admin" -Url $newSiteNameUrl
}
Disconnect-PnPOnline
Ensure-PnPConnection -url $newSiteNameUrl
Write-Host "Applying template to site " $newSiteNameUrl", please wait..." -ForegroundColor Green
Apply-PnPProvisioningTemplate $filepath
Disconnect-PnPOnline
Write-Host "Finished" -ForegroundColor Green
} else {
Write-Host "Template file not found, nothing to do, exiting." -ForegroundColor Red
}
