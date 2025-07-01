# Path to the log file
$logFile = "C:\PSScripts\NoticePeriod_Log.txt"

# Optional: Uncomment to reset log each time
# Remove-Item $logFile -ErrorAction SilentlyContinue

# Build the request body as a hashtable
$bodyRaw = @{
    SelectedCollectiveAgreement = 1
    TerminatingParty            = 0
    SalariedEmployee            = 1
    ContractStartDate           = "2010-01-01"
    ContractTerminatedDate      = "2025-06-25"
    BirthdayDate                = "1988-01-27"
}

# Convert to pretty JSON for logging
$bodyFormatted = $bodyRaw | ConvertTo-Json -Depth 5 -Compress:$false

# Log the request body
Add-Content -Path $logFile -Value "=== REQUEST ==="
Add-Content -Path $logFile -Value $bodyFormatted
Add-Content -Path $logFile -Value "`n"

# Convert to compact JSON for the API call
$bodyToSend = $bodyRaw | ConvertTo-Json -Depth 5

try {
    # Send the request
    $response = Invoke-WebRequest -Uri "https://localhost:7236/api/dmcalculators/noticePeriod" `
                                  -Method POST `
                                  -Body $bodyToSend `
                                  -ContentType "application/json" `
                                  -UseBasicParsing

    # Convert and format the response
    $responseFormatted = $response.Content | ConvertFrom-Json | ConvertTo-Json -Depth 10 -Compress:$false

    # Log the response
    Add-Content -Path $logFile -Value "=== RESPONSE ==="
    Add-Content -Path $logFile -Value $responseFormatted
    Add-Content -Path $logFile -Value "`n`n"

    # Also output to console if needed
    $responseFormatted
}
catch {
    Write-Host "`n--- ERROR ON CALL TO API ---`n" -ForegroundColor Red

    if ($_.Exception.Response) {
        $stream = $_.Exception.Response.GetResponseStream()
        $reader = New-Object System.IO.StreamReader($stream)
        $errorResponse = $reader.ReadToEnd()

        # Log error response
        Add-Content -Path $logFile -Value "=== ERROR RESPONSE ==="
        Add-Content -Path $logFile -Value $errorResponse
        Add-Content -Path $logFile -Value "`n`n"

        Write-Host $errorResponse -ForegroundColor Yellow
    }
    else {
        Write-Host $_.Exception.Message -ForegroundColor Red
    }
}
