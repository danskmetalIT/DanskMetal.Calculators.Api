# Call for Rest Period
# Path to the log file
$logFile = "C:\PSScripts\RestPeriod_Log.txt"

# Optional: Uncomment the line below to clear the log file each time
# Remove-Item $logFile -ErrorAction SilentlyContinue

# Build the request body as a raw hashtable
$bodyRaw = @{
    isEightHoursRules = 0
    t1 = 1; t2 = 1; t3 = 1; t4 = 1; t5 = 1; t6 = 1; t7 = 0; t8 = 0
    t9 = 0; t10 = 0; t11 = 0; t12 = 0; t13 = 0; t14 = 0; t15 = 0; t16 = 0
    t17 = 1; t18 = 1; t19 = 1; t20 = 1; t21 = 1; t22 = 1; t23 = 1; t24 = 1
}

# Convert the hashtable to pretty-formatted JSON for logging
$bodyFormatted = $bodyRaw | ConvertTo-Json -Depth 5 -Compress:$false

# Log the request body
Add-Content -Path $logFile -Value "=== REQUEST ==="
Add-Content -Path $logFile -Value $bodyFormatted
Add-Content -Path $logFile -Value "`n"

# Convert to compact JSON for sending in the request
$bodyToSend = $bodyRaw | ConvertTo-Json -Depth 5

# Send the request
$response = Invoke-WebRequest -Uri "https://localhost:7236/api/dmcalculators/restPeriod" `
                              -Method POST `
                              -Body $bodyToSend `
                              -ContentType "application/json" `
                              -UseBasicParsing

# Convert the response to a formatted JSON string
$responseFormatted = $response.Content | ConvertFrom-Json | ConvertTo-Json -Depth 10 -Compress:$false

# Log the response content
Add-Content -Path $logFile -Value "=== RESPONSE ==="
Add-Content -Path $logFile -Value $responseFormatted
Add-Content -Path $logFile -Value "`n`n"
