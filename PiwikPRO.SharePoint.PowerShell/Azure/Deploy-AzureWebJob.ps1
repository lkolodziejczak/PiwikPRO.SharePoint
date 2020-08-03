Param (
    [Parameter(Position = 0, Mandatory = $true)]
    [string]$ResourceGroupName,
    [Parameter(Position = 1, Mandatory = $true)]
    [string]$WebAppName,
    [Parameter(Position = 2)]
    [string]$ZipFilePath = "./packages/deploy.zip",
    [Switch]$SkipLogin
)

if (-not (Get-Command az -ErrorAction SilentlyContinue) -or [System.Version](az version --query '\"azure-cli\"' -o tsv) -lt [System.Version]"2.9.1") {
    Write-Host "Installing Azure CLI...";
    Invoke-WebRequest -Uri https://aka.ms/installazurecliwindows -OutFile .\AzureCLI.msi; Start-Process msiexec.exe -Wait -ArgumentList '/I AzureCLI.msi /quiet'; rm .\AzureCLI.msi;
    Write-Host "Azure CLI installed.";
    $Env:Path = "${Env:ProgramFiles(x86)}\Microsoft SDKs\Azure\CLI2\wbin;${Env:Path}";
}

if (-not $SkipLogin) {
    az login -o none;
}

az webapp deployment source config-zip -g "$ResourceGroupName" -n "$WebAppName" --src "$ZipFilePath";

if (-not $SkipLogin) {
    az logout;
}