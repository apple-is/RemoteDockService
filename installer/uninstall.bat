@echo off
title RemoteDockService Deinstallation
color 0C
cls

echo RemoteDockService deinstallieren...
echo.
sc.exe stop RemoteDockService >nul 2>&1
sc.exe delete RemoteDockService >nul 2>&1
netsh advfirewall firewall delete rule name="RemoteDockService" >nul 2>&1
echo.
echo Deinstallation abgeschlossen.
pause