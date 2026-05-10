@echo off
title RemoteDockService Installation
color 0A
cls

echo.
echo  ╔══════════════════════════════════════════╗
echo  ║     RemoteDockService v1.0.0              ║
echo  ║     Open Source - MIT License             ║
echo  ╚══════════════════════════════════════════╝
echo.
echo  GitHub: github.com/apple-is/RemoteDockService
echo.
echo  -------------------------------------------------
echo  Dieser Dienst ermoeglicht der "Remote Dock" App,
echo  diesen PC aus dem lokalen Netzwerk zu steuern.
echo.
echo  WAS PASSIERT JETZT?
echo  - Ein Windows Dienst wird installiert
echo  - Der Dienst startet automatisch mit Windows
echo  - Firewall-Regel fuer Port 8080 wird erstellt
echo  - KEINE Daten werden ins Internet gesendet
echo  -------------------------------------------------
echo.
echo  Quellcode einsehbar auf GitHub!
echo.
set /p confirm="Fortfahren? (J/N): "
if /i not "%confirm%"=="J" exit

echo.
echo  [1/4] Alten Dienst stoppen...
sc.exe stop RemoteDockService >nul 2>&1
sc.exe delete RemoteDockService >nul 2>&1
timeout /t 2 >nul

echo  [2/4] Dienst installieren...
sc.exe create RemoteDockService binPath= "%~dp0RemoteDockService.exe" start= auto
if errorlevel 1 (
    echo  FEHLER: Bitte als Administrator ausfuehren!
    pause
    exit
)

echo  [3/4] Firewall konfigurieren...
netsh advfirewall firewall delete rule name="RemoteDockService" >nul 2>&1
netsh advfirewall firewall add rule name="RemoteDockService" dir=in action=allow protocol=TCP localport=8080

echo  [4/4] Dienst starten...
sc.exe start RemoteDockService

echo.
echo  ╔══════════════════════════════════════════╗
echo  ║     INSTALLATION ERFOLGREICH!             ║
echo  ╚══════════════════════════════════════════╝
echo.
echo  Der Dienst laeuft jetzt im Hintergrund.
echo  TESTEN: http://localhost:8080/status
echo  DEINSTALLIEREN: uninstall.bat
echo.
pause