Param (
[Parameter(Position=0,mandatory=$true)]
[string]$resourceGroupName,
[Parameter(Position=1,mandatory=$true)]
[ValidateLength(1,16)]
[string]$webAppSuffix,
[Parameter(Position=2,mandatory=$true)]
[string]$subscriptionName
)

# <<input parameters>>
#--------------------------------------------------------------------------#
# $login = "<<yourlogin>>"
# $password = ConvertTo-SecureString '<yourpass>' -AsPlainText -Force
$appDomain="localhost";
$appUrl="https://localhost.com";
$appName="testapp02"
#--------------------------------------------------------------------------#

$clientID = (New-Guid).Guid;
$bytes = New-Object Byte[] 32
$rand = [System.Security.Cryptography.RandomNumberGenerator]::Create()
$rand.GetBytes($bytes)
$rand.Dispose()
$clientSecret = [System.Convert]::ToBase64String($bytes)
$dtStart = [System.DateTime]::Now
$dtEnd = $dtStart.AddYears(10)
$servicePrincipalName = @("$clientID/$appDomain")
$credential = New-Object System.Management.Automation.PSCredential($login, $password)


function WaitFor-IEReady {
    Param
    (
        [Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=0)]
        $ie,
        [Parameter(Mandatory=$false, Position=1)]
        $initialWaitInSeconds = 1
    )
 
    sleep -Seconds $initialWaitInSeconds
    while ($ie.Busy) {
        sleep -milliseconds 50
    }
}
function GrantAccessToTenant($clientID) { 
  $ie = New-Object -com internetexplorer.application
  try {
      $urlSplitted = (Get-MsolDomain).Name.Split(".")
      $tenantName = "$($urlSplitted[0])-admin"
      $WebUrl = "https://$($tenantName).sharepoint.com"
      $ie.Visible = $true
      $ie.Navigate2($WebUrl)
      $authorizeURL = "$($WebUrl.TrimEnd('/'))/_layouts/15/appinv.aspx"
      $ie.Visible = $false
      $ie.Navigate2($authorizeURL)
      WaitFor-IEReady $ie -initialWaitInSeconds 3
      $appIdInput = $ie.Document.IHTMLDocument3_getElementById('ctl00_ctl00_PlaceHolderContentArea_PlaceHolderMain_IdTitleEditableInputFormSection_ctl01_TxtAppId')
      $appIdInput.value = $clientID
      $lookupBtn = $ie.Document.IHTMLDocument3_getElementById('ctl00_ctl00_PlaceHolderContentArea_PlaceHolderMain_IdTitleEditableInputFormSection_ctl01_BtnLookup')
      $lookupBtn.Click()
      WaitFor-IEReady $ie -initialWaitInSeconds 3
      $appIdInput = $ie.Document.IHTMLDocument3_getElementById('ctl00_ctl00_PlaceHolderContentArea_PlaceHolderMain_TitleDescSection_ctl01_TxtPerm')
      $appIdInput.value = '<AppPermissionRequests AllowAppOnlyPolicy="true"> <AppPermissionRequest Scope="http://sharepoint/content/tenant" Right="FullControl" /> </AppPermissionRequests>'
      $createBtn = $ie.Document.IHTMLDocument3_getElementById('ctl00_ctl00_PlaceHolderContentArea_PlaceHolderMain_ctl01_RptControls_BtnCreate')
      $createBtn.Click()
      WaitFor-IEReady $ie -initialWaitInSeconds 3
      $trustBtn = $ie.Document.IHTMLDocument3_getElementById('ctl00_ctl00_PlaceHolderContentArea_PlaceHolderMain_BtnAllow')
      $trustBtn.Click()
      WaitFor-IEReady $ie -initialWaitInSeconds 3
   }
    catch {
    Write-Host "Unknown Error" -ForegroundColor Red
    }
    finally {
        $ie.Quit()
    } 
  }
function RegisterNewApp($servicePrincipalName,$clientID,$appName,$appUrl,$dtStart,$dtEnd,$clientSecret) {
    New-MsolServicePrincipal -ServicePrincipalNames $servicePrincipalName -AppPrincipalId $clientID -DisplayName $appName -Type Symmetric -Usage Verify -Value $clientSecret -Addresses (New-MsolServicePrincipalAddresses -Address $appUrl) -StartDate $dtStart  –EndDate $dtEnd
    New-MsolServicePrincipalCredential -AppPrincipalId $clientId -Type Symmetric -Usage Sign -Value $newClientSecret -StartDate $dtStart  –EndDate $dtEnd
    New-MsolServicePrincipalCredential -AppPrincipalId $clientId -Type Password -Usage Verify -Value $newClientSecret -StartDate $dtStart  –EndDate $dtEnd
}

Connect-MsolService #-Credential $credential # uncomment parameter -Credencial if $login and $password defined

RegisterNewApp $servicePrincipalName $clientID $appName $appUrl $dtStart $dtEnd
GrantAccessToTenant $clientID 

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
    $appSettingsHash['PiwikAzureAppKey'] = "$($clientId)"
    $appSettingsHash['PiwikAzureAppSecret'] = "$($clientSecret)"
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
