@echo off
title RemoteDockService Installation
color 0A
cls
echo.
echo   RemoteDockService v1.0.1
echo   github.com/apple-is/RemoteDockService
echo.
echo   Installiere...
echo.

REM Scheduled Task erstellen (unsichtbar)
schtasks /create /tn "RemoteDockService" /tr "%~dp0RemoteDockService.exe" /sc onlogon /ru "%USERNAME%" /rl highest /f >nul 2>&1

REM Firewall
netsh advfirewall firewall delete rule name="RemoteDockService" >nul 2>&1
netsh advfirewall firewall add rule name="RemoteDockService" dir=in action=allow protocol=TCP localport=8080 >nul 2>&1

REM Starten
schtasks /run /tn "RemoteDockService" >nul 2>&1

echo.
echo   FERTIG!
echo   Test: http://localhost:8080/status
echo   Deinstallieren: uninstall.bat
echo.
pause