
powershell -command "& { Remove-Item *.nupkg }"
powershell -command "& { Remove-Item *.tmp }"
powershell -command "& { if (Test-Path .\TestResults) { Remove-Item .\TestResults -Recurse -Force } }"
rem powershell -command "& { if (Test-Path .\Packages) { Remove-Item .\Packages -Recurse -Force } }"

cd Bhbk.Lib.DataAccess.EF
powershell -command "& { if (Test-Path .\bin) { Remove-Item .\bin -Recurse -Force } }"
powershell -command "& { if (Test-Path .\obj) { Remove-Item .\obj -Recurse -Force } }"

cd ..\Bhbk.Lib.DataAccess.EF.Tests
powershell -command "& { if (Test-Path .\bin) { Remove-Item .\bin -Recurse -Force } }"
powershell -command "& { if (Test-Path .\obj) { Remove-Item .\obj -Recurse -Force } }"

cd..
