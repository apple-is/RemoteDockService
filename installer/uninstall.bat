@echo off
title RemoteDockService Deinstallation
color 0C
cls
echo RemoteDockService wird entfernt...
schtasks /delete /tn "RemoteDockService" /f >nul 2>&1
netsh advfirewall firewall delete rule name="RemoteDockService" >nul 2>&1
echo.
echo Fertig.
pause