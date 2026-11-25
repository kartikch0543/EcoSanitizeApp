# EcoSanitize: SQLite Web Database Admin Launcher
$dbPath = Join-Path $PSScriptRoot "EcoSanitize.db"
Write-Host "========================================================" -ForegroundColor Cyan
Write-Host "  EcoSanitize: SQLite Web Database Admin Launcher" -ForegroundColor Green
Write-Host "========================================================" -ForegroundColor Cyan
Write-Host "Database: $dbPath"
Write-Host "Admin URL: http://127.0.0.1:8080" -ForegroundColor Yellow
Write-Host ""

$portInUse = Get-NetTCPConnection -LocalPort 8080 -ErrorAction SilentlyContinue
if ($portInUse) {
    Write-Host "SQLite Web Admin is already running on http://127.0.0.1:8080!" -ForegroundColor Green
    Start-Process "http://127.0.0.1:8080"
    exit 0
}

Start-Process "http://127.0.0.1:8080"
$exePath = "C:\Users\karti\AppData\Roaming\Python\Python314\Scripts\sqlite_web.exe"
if (Test-Path $exePath) {
    & $exePath $dbPath --port 8080 --host 127.0.0.1 -x
} else {
    python -m sqlite_web $dbPath --port 8080 --host 127.0.0.1 -x
}
