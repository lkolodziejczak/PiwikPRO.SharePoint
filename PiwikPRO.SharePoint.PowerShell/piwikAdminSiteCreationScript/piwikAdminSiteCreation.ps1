Param (
        [string]$sharePointUserName,
        [string]$sharePointPassword,
        [Parameter(Position=0,Mandatory=$true)]
        [string]$sharePointUrl, # "https://kogifidev3.sharepoint.com"
        [Parameter(Position=1,Mandatory=$true)]
        [string]$newSiteName # "PiwikAdmin"
    )
$global:filepath = ".\PiwikProAdminAndConfigLists.xml"
$global:sharePointCredentials = $null
$global:sharePointUserName = $sharePointUserName
$global:sharePointPassword = $sharePointPassword
$global:sharePointUrl = $sharePointUrl
$global:MFAInUse = $useMFA.IsPresent
$IsNewInstallation = $true
$newSiteNameUrl = "{0}/sites/{1}" -f $sharePointUrl, $newSiteName
$isTemplateFileExists = Test-Path $filepath -PathType Leaf
$global:propertyBag = @{
    piwik_templateusegoalpageedited = "true"
    piwik_sha3 = "true"
    piwik_templatesenddepartment = "true"
    piwik_templatesendusername = "true"
    piwik_enforcessl = "true"
    piwik_templateusegoalpageadded = "true"
    sharepointhelpoverride = "SPOLite"
    piwik_templatesendjobtitle = "true"
    piwik_templatesendoffice = "true"
    piwik_templateusegoaldocumentadded = "true"
    piwik_containersurl = "//kogifi.containers.piwik.pro/"
    piwik_serviceurl = "kogifi.piwik.pro"
    piwik_listname = "Piwik Pro Site Directory"
    piwik_trackerjsscripturl = "https://kogifidev3.sharepoint.com/Style Library/js/piwikTracker.js"
    piwik_templatesenduserencoded = "true"
    piwik_templatesenduserextendedinfo = "true"
    piwik_clientid = "JFXCbR26q8wQn5GhqKHQqAQNYNNl0cRR"
    piwik_clientsecret = "sAQmq4IXNa9HSnwra4k7Sjzxgi0Xe82AIOwrDdjXiop0ZO1zjSxA3Xjtj0sqJ3rEOIuiDeVLiyBVlNac"
    piwik_oldapitoken = "ac665b274143d3c994c7426c621e8422"
    piwik_adminsiteurl = "https://piwikpro.madeinpoint.com/sites/piwikproadmin"
}

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

function EnableAddAndCustomizePages {
    Param(
    $siteNameUrl,
    $tenantUr
    )
    $Credentials = New-Object -TypeName System.Management.Automation.PSCredential -argumentlist $global:sharePointUserName, $(convertto-securestring $global:sharePointPassword -asplaintext -force)
  
    $urlSplitted = $global:sharePointUrl.Split(".")
    $tenantName = "$($urlSplitted[0])-admin"
    $WebUrl = "$($tenantName).sharepoint.com"
    Connect-SPOService $WebUrl -Credential $Credentials
    Set-SPOSite $siteNameUrl -DenyAddAndCustomizePages 0
    Disconnect-SPOService
}

function ApplyPropertyBag {
    Param(
    $SiteUrl
    )
    Add-Type -Path ".\Microsoft.SharePoint.Client.dll"
    Add-Type -Path ".\Microsoft.SharePoint.Client.Runtime.dll"
    $pass = ConvertTo-SecureString $global:sharePointPassword -AsPlainText -Force;
    $Credentials = New-Object Microsoft.SharePoint.Client.SharePointOnlineCredentials($global:sharePointUserName, $pass)
   
    $Ctx = New-Object Microsoft.SharePoint.Client.ClientContext($SiteURL)
    $Ctx.Credentials = $Credentials
      
    $Web = $Ctx.Web
    $Ctx.Load($Web)
    $Ctx.Load($Web.AllProperties)
    $Ctx.ExecuteQuery()
  
    foreach ($h in $global:propertyBag.Keys)
    {   
        $Web.AllProperties[$h] = $($global:propertyBag.Item($h))
        $Web.Update()
        $Ctx.ExecuteQuery()
  
        Write-Host -f Green "Property Bag Key '$h' Value Updated to: " $($global:propertyBag.Item($h))
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
EnableAddAndCustomizePages -siteNameUrl $newSiteNameUrl
ApplyPropertyBag -SiteUrl $newSiteNameUrl
