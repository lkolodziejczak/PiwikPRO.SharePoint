function Wait4timer($solutionName) {    
    $solution = Get-SPSolution | where-object { $_.Name -eq $solutionName }    
    if ($solution -ne $null) {        
        Write-Host "Waiting to finish soultion timer job"
        while ($solution.JobExists -eq $true ) {               
            Write-Host "Please wait...Either a Retraction/Deployment is happening"       
            sleep 2            
        }                
        Write-Host "Finished the solution timer job"
    }
}  
Export-ModuleMember -Function 'Wait4timer'