# Create new log file
function New-LogFile(
    [Parameter(Mandatory=$True)][string]$FileName, 
    [Parameter(Mandatory=$True)][string]$FolderName)
{
	$isFolderPresent = Test-Path ".\$FolderName\logs"
	if($isFolderPresent -eq $false)
	{
		New-Item -Path ".\$FolderName\logs" -ItemType Directory
    }
    $FileName = $FileName + (Get-Date).toString('yyyyMMddHHmmss')
	$path = ".\$FolderName\logs\$FileName.log"
	$global:LogFile = New-Item -Path $path -ItemType File
}
Export-ModuleMember -Function 'New-LogFile'

# Add string log to file and output it to the console
function Add-Log(
		[Parameter(Mandatory=$False)][ValidateSet('INFO','WARN','ERROR')][String]$Level = 'INFO',
		[Parameter(Mandatory=$True)][string]$Message)
{
	$timeStamp = (Get-Date).toString('yyyy/MM/dd HH:mm:ss')
    $line = "$timeStamp $Level -> $Message"
    if($global:LogFile)
	{
        Add-Content $global:LogFile -Value $line -ErrorAction Stop
	}
	$color = "White"
	if($Level -eq 'WARN')
	{
		$color = "DarkYellow"
	}
	elseif ($Level -eq 'ERROR') 
	{
		$color = "DarkRed"
	}
	Write-Host $line -ForegroundColor $Color
}
Export-ModuleMember -Function 'Add-Log'