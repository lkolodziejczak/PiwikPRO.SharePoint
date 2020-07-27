Param(
[Parameter(Position=0,Mandatory=$true)]
$resourceGroupName = "piwikpro",
[Parameter(Position=1,Mandatory=$true)]
$webAppName = "piwikpro01",
[Parameter(Position=2,Mandatory=$true)]
$zipfilePath
)
$Apiversion = '2015-08-01'

#Function to get Publishing credentials for the WebApp :
function Get-PublishingProfileCredentials($resourceGroupName, $webAppName){

$resourceType = "Microsoft.Web/sites/config"
$resourceName = "$webAppName/publishingcredentials"
$publishingCredentials = Invoke-AzResourceAction -ResourceGroupName $resourceGroupName -ResourceType $resourceType -ResourceName $resourceName -Action list -ApiVersion $Apiversion -Force
   return $publishingCredentials
}

#Pulling authorization access token :
function Get-KuduApiAuthorisationHeaderValue($resourceGroupName, $webAppName){

$publishingCredentials = Get-PublishingProfileCredentials $resourceGroupName $webAppName
return ("Basic {0}" -f [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $publishingCredentials.Properties.PublishingUserName, $publishingCredentials.Properties.PublishingPassword))))
}
$accessToken = Get-KuduApiAuthorisationHeaderValue $resourceGroupName $webAppname
$filename = $zipfilePath.Split("\")[-1]
#Generating header to create and publish the Webjob :
$Header = @{
'Content-Disposition'="attachment; attachment; filename=$($filename)"
'Authorization'=$accessToken
        }
$apiUrl = "https://$webAppName.scm.azurewebsites.net/api/continuouswebjobs/webjob01"
Invoke-RestMethod -Uri $apiUrl -Headers $Header -Method put -InFile $zipfilePath -ContentType 'application/zip'