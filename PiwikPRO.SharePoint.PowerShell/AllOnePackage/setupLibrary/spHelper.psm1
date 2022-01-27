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
Export-ModuleMember -Function 'Connect-ToSharePoint'

function Get-SharePointPnPPowerShell([Parameter(Mandatory = $true)][string]$SharePointVersion)
{
	if (-not (Get-Module -ListAvailable -Name "SharePointPnPPowerShell$($SharePointVersion)")) {
		Add-Log -Level "INFO" -Message "Installing PnP PowerShell $SharePointVersion"
		Install-Module "SharePointPnPPowerShell$($SharePointVersion)" -AllowClobber -Force
		return
	}

	Add-Log -Level "INFO" -Message "PnP PowerShell $SharePointVersion already installed"
}
Export-ModuleMember -Function 'Get-SharePointPnPPowerShell'

function Get-SharePointPnPPowerShell2016([Parameter(Mandatory = $true)][string]$SharePointVersion)
{
	if (-not (Get-Module -ListAvailable -Name "SharePointPnPPowerShell$($SharePointVersion)")) {
		Add-Log -Level "INFO" -Message "Installing PnP PowerShell $SharePointVersion"
		Install-Module "SharePointPnPPowerShell$($SharePointVersion)" -RequiredVersion "3.19.2003.0" -AllowClobber -Force
		return
	}

	Add-Log -Level "INFO" -Message "PnP PowerShell $SharePointVersion already installed"
}

Export-ModuleMember -Function 'Get-SharePointPnPPowerShell2016'