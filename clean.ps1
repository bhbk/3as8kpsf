
Remove-Item *.nupkg

cd Bhbk.Lib.Helpers
if (Test-Path .\bin) { Remove-Item .\bin -Recurse -Force }
if (Test-Path .\obj) { Remove-Item .\obj -Recurse -Force }

cd ..\Bhbk.Lib.Waf
if (Test-Path .\bin) { Remove-Item .\bin -Recurse -Force }
if (Test-Path .\obj) { Remove-Item .\obj -Recurse -Force }

cd ..\Bhbk.Lib.Waf.Tests
if (Test-Path .\bin) { Remove-Item .\bin -Recurse -Force }
if (Test-Path .\obj) { Remove-Item .\obj -Recurse -Force }

cd..
