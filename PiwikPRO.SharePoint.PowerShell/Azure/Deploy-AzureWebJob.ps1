Param (
    [Parameter(Position = 0, Mandatory = $true)]
    [string]$resourceGroupName,
    [Parameter(Position = 1, Mandatory = $true)]
    [string]$webAppName,
    [Parameter(Position = 2)]
    [string]$zipFilePath = "./packages/deploy.zip",
    [Switch]$skipLogin
)

if (-not (Get-Command az -ErrorAction SilentlyContinue) -or [System.Version](az version --query '\"azure-cli\"' -o tsv) -lt [System.Version]"2.9.1") {
    Write-Host "Installing Azure CLI...";
    Invoke-WebRequest -Uri https://aka.ms/installazurecliwindows -OutFile .\AzureCLI.msi; Start-Process msiexec.exe -Wait -ArgumentList '/I AzureCLI.msi /quiet'; rm .\AzureCLI.msi;
    Write-Host "Azure CLI installed.";
    $Env:Path = "${Env:ProgramFiles(x86)}\Microsoft SDKs\Azure\CLI2\wbin;${Env:Path}";
}

if (-not $skipLogin) {
    az login -o none;
}

az webapp deployment source config-zip -g "$resourceGroupName" -n "$webAppName" --src "$zipFilePath";

if (-not $skipLogin) {
    az logout;
}