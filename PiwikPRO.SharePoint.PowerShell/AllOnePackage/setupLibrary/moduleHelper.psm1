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
Export-ModuleMember -Function 'Load-Module'

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
Export-ModuleMember -Function 'Ensure-Module'