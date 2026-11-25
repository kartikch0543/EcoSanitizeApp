@echo off
title EcoSanitize SQLite Web Admin
echo ========================================================
echo   EcoSanitize: SQLite Web Database Admin Launcher
echo ========================================================
echo.
echo Database: %~dp0EcoSanitize.db
echo Admin URL: http://127.0.0.1:8080
echo.

netstat -ano | findstr :8080 >nul
if %ERRORLEVEL% EQU 0 (
    echo SQLite Web Admin is already active on port 8080! Opening browser...
    start http://127.0.0.1:8080
    timeout /t 3 >nul
    exit /b 0
)

echo Starting SQLite Web Admin...
start http://127.0.0.1:8080
if exist "C:\Users\karti\AppData\Roaming\Python\Python314\Scripts\sqlite_web.exe" (
    "C:\Users\karti\AppData\Roaming\Python\Python314\Scripts\sqlite_web.exe" "%~dp0EcoSanitize.db" --port 8080 --host 127.0.0.1 -x
) else (
    python -m sqlite_web "%~dp0EcoSanitize.db" --port 8080 --host 127.0.0.1 -x
)
pause
