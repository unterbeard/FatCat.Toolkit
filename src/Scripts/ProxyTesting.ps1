Write-Host "Going to test proxy spike" -ForegroundColor Yellow

$testGetUri = "https://localhost:14555/api/Test"

Invoke-RestMethod $testGetUri `
    -Method Get `
    -Verbose