# RemoteDockService

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/Platform-Windows%2010%2F11-brightgreen.svg)]()

**Teil der Remote Dock App**  
PC-Steuerung aus dem lokalen Netzwerk - Open Source, sicher, transparent.

---

## 📥 Installation (für Anwender)

### Schritt 1: Herunterladen
→ [Neueste Version](https://github.com/apple-is/RemoteDockService/releases/latest)
→ `RemoteDockService.zip` entpacken

### Schritt 2: Installieren
1. Rechtsklick auf `install.bat`
2. **"Als Administrator ausführen"**
3. Fertig!

### Schritt 3: Prüfen
Browser: `http://localhost:8080/status`  
→ `{"online":true,"hostname":"DEIN-PC"}`

### Deinstallation
`uninstall.bat` als Administrator ausführen.

---

## 🔒 Sicherheit

- ✅ Nur lokales Netzwerk (192.168.x.x)
- ✅ Nicht aus dem Internet erreichbar
- ✅ Keine Datensammlung
- ✅ Open Source - jeder kann Code prüfen

---

## 🛠 Entwickler

```bash
git clone https://github.com/apple-is/RemoteDockService.git
cd RemoteDockService/src
dotnet restore
dotnet publish -c Release -r win-x64 --self-contained