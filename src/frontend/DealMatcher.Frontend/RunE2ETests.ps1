$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $scriptDir

$composeDir = Resolve-Path "../../.."

Write-Host "=== Starting Docker Compose ===" -ForegroundColor Cyan
docker-compose -f "$composeDir/docker-compose.yml" up -d

Write-Host "=== Waiting for backend===" -ForegroundColor Cyan
Start-Sleep -Seconds 3

Write-Host "Backend is ready!" -ForegroundColor Green

Write-Host "=== Starting ChromeDriver ===" -ForegroundColor Cyan
$chromedriverJob = Start-Job -ScriptBlock {
    & "$env:USERPROFILE\chromedriver\chromedriver-win64\chromedriver.exe" --port=4444
}
Start-Sleep -Seconds 2

Write-Host "=== Running E2E Tests ===" -ForegroundColor Cyan

$tests = @(
    "integration_test/account_test.dart",
    "integration_test/search_test.dart",
    "integration_test/user_test.dart"
)

$allPassed = $true

foreach ($test in $tests) {
    Write-Host "`n--- Running: $test ---" -ForegroundColor Yellow
    flutter drive `
        --driver=test_driver/integration_test.dart `
        --target=$test `
        -d chrome `
        --dart-define=API_URL=http://localhost:5000 `
        --web-port=8090
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "FAILED: $test" -ForegroundColor Red
        $allPassed = $false
    }
    else {
        Write-Host "PASSED: $test" -ForegroundColor Green
    }
}

Write-Host "=== Cleaning up ===" -ForegroundColor Cyan
Stop-Job -Job $chromedriverJob
Remove-Job -Job $chromedriverJob
docker-compose -f "$composeDir/docker-compose.yml" down

if ($allPassed) {
    Write-Host "`n=== ALL TESTS PASSED ===" -ForegroundColor Green
    exit 0
}
else {
    Write-Host "`n=== SOME TESTS FAILED ===" -ForegroundColor Red
    exit 1
}