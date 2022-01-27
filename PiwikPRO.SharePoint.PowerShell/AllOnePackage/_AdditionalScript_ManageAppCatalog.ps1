Param (
    [Parameter(Position = 0, Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$appCatalogUrl,
	
	[Parameter(Position = 1, Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$sharePointTenantAdminUrl,
	
	[Parameter(Position = 2, Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$piwikAdminUrl
)

$FileName = 'installationLogsAppCatalog' 
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
    #New-LogFile -FileName 'installationLogsAppCatalog' -FolderName '.logs' -ErrorAction Stop
    Write-Host "==========START Additional script to manage app catalog=========="
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

Connect-ToSharePoint -Url $appCatalogUrl -TenantAdminUrl $sharePointTenantAdminUrl -Credentials $credentials -UseWebLogin $config.useWebLogin
# Setting extenstiosn to be tenant wide
        Write-Host "Configuring tenant-wide extensions"
		Apply-PnPProvisioningTemplate -Path $config.constants.templatePath -TemplateId "PIWIK-TENANT-WIDE"
		Start-Sleep -s 2
        # Creating piwik admin site
        Write-Host "Checking if Piwik PRO Administration site already exists"
        if (-not (Get-PnPTenantSite -Url $piwikAdminUrl -ErrorAction SilentlyContinue)) {
            Write-Host "Piwik PRO Administration site doesn't exist. Creating"
            New-PnPTenantSite -Title "Piwik PRO Administration" -Url $piwikAdminUrl -Owner $config.sharepointAdminLogin -TimeZone 4 -Lcid 1033 -Template "STS#3" -Wait -Force | Out-Null
			Write-Host "Piwik PRO Administration created."
        }
Start-Sleep -s 2
        Disconnect-PnPOnline
}
catch
{
	$ErrorMessage = $_.Exception.Message
    Write-Host $ErrorMessage
}
Write-Host "==========Finished succesfully=========="
Stop-Transcript