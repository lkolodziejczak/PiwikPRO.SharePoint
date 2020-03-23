Param (
[Parameter(Position=0,mandatory=$true)]
[string]$resourceGroupName,
[Parameter(Position=1,mandatory=$true)]
[ValidateLength(1,16)]
[string]$webAppSuffix,
[Parameter(Position=2,mandatory=$true)]
[string]$subscriptionName
)
Connect-AzAccount

$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
$parametersFilePath = $($scriptPath + "\parameters.json")
$temporaryParametersFilePath = $($parametersFilePath + '_temporary')
$templateFilePath = $($scriptPath + "\template.json")
$zippedFilePath = $($scriptPath + "\piwikpro.zip")
$webAppName = 'piwikpro' + $webAppSuffix
$subscriptionId = (Get-AzSubscription -SubscriptionName $subscriptionName).Id
$appSettingsHash = @{}

Try {
    Copy-Item $parametersFilePath $temporaryParametersFilePath
    (Get-Content $parametersFilePath).Replace('{labelToBeReplaced}', $webAppName).Replace('{subscriptionId}', $subscriptionId).Replace('{resourcegroupname}', $resourceGroupName) | Set-Content $parametersFilePath
    New-AzResourceGroup -Name $resourceGroupName -Location westeurope -Force
    New-AzResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile $templateFilePath -TemplateParameterFile $parametersFilePath
    Compress-Archive -Path $parametersFilePath, $templateFilePath -DestinationPath $zippedFilePath
    Publish-AzWebapp -ResourceGroupName $resourceGroupName -Name $webAppName -ArchivePath $zippedFilePath -Force
    $instrumentationKey = (Get-AzApplicationInsights -ResourceGroupName $resourceGroupName -Name $webAppName).InstrumentationKey
    $saKey = (Get-AzStorageAccountKey -ResourceGroupName $resourceGroupName -Name $webAppName)[0].Value
    $appSettingsHash['APPINSIGHTS_INSTRUMENTATIONKEY'] = "$($instrumentationKey)"
    $appSettingsHash['APPINSIGHTS_PROFILERFEATURE_VERSION'] = '1.0.0'
    $appSettingsHash['ApplicationInsightsAgent_EXTENSION_VERSION'] = '~2'
    $appSettingsHash['DiagnosticServices_EXTENSION_VERSION'] = '~3'
    $connectionString = "DefaultEndpointsProtocol=https;AccountName=$($webAppName);AccountKey=$($saKey);EndpointSuffix=core.windows.net"
    $connectionStringsHash = @{ AzureWebJobsDashboard = @{ Type = "Custom"; Value = $connectionString }; AzureWebJobsStorage = @{ Type = "Custom"; Value = $connectionString }}
    Set-AzWebApp -ResourceGroupName $resourceGroupName -Name $webAppName -AppSettings $appSettingsHash -ConnectionStrings $connectionStringsHash
}
Catch {}
Finally {
    Remove-Item $parametersFilePath 
    Rename-Item -Path $temporaryParametersFilePath -NewName $parametersFilePath
    Remove-Item -Path $zippedFilePath
}
