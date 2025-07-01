# Call for Serverence Pay
# Path to the log file
$logFile = "C:\PSScripts\SeverancePay_Log.txt"

# Optional: Uncomment to clear the log on each run
# Remove-Item $logFile -ErrorAction SilentlyContinue

# Build the request body as a hashtable
$bodyRaw = @{
    salaryPerHour     = 210.50
    workingHours      = 37
    fritvalgsPercent  = 9.45
    UnemploymentMonthlyRate  = 21092
}

# Convert to pretty JSON for logging
$bodyFormatted = $bodyRaw | ConvertTo-Json -Depth 5 -Compress:$false

# Log the request body
Add-Content -Path $logFile -Value "=== REQUEST ==="
Add-Content -Path $logFile -Value $bodyFormatted
Add-Content -Path $logFile -Value "`n"

# Convert to compact JSON for the actual request
$bodyToSend = $bodyRaw | ConvertTo-Json -Depth 5

# Send the request
$response = Invoke-WebRequest -Uri "https://localhost:7236/api/dmcalculators/severancepay" `
                              -Method POST `
                              -Body $bodyToSend `
                              -ContentType "application/json" `
                              -UseBasicParsing

# Convert and format the response
$responseFormatted = $response.Content | ConvertFrom-Json | ConvertTo-Json -Depth 10 -Compress:$false

# Log the response content
Add-Content -Path $logFile -Value "=== RESPONSE ==="
Add-Content -Path $logFile -Value $responseFormatted
Add-Content -Path $logFile -Value "`n`n"