Param (
[Parameter(Position=0,mandatory=$true)]
[string]$resourceGroupNameToCreate
)
Connect-AzAccount
$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
New-AzResourceGroup -Name $resourceGroupNameToCreate -Location westeurope -Force
New-AzResourceGroupDeployment -ResourceGroupName $resourceGroupNameToCreate -TemplateFile $($scriptPath + "\template.json") -TemplateParameterFile $($scriptPath + "\parameters.json")
Compress-Archive -Path $($scriptPath + "\parameters.json"), $($scriptPath + "\template.json") -DestinationPath $($scriptPath + "\piwikpro.zip")
Publish-AzWebapp -ResourceGroupName $resourceGroupNameToCreate -Name piwikpro29632693635888513466 -ArchivePath $($scriptPath + "\piwikpro.zip") -Force

Remove-Item -Path $($scriptPath + "\piwikpro.zip")