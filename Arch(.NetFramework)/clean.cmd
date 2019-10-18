
powershell -command "& { if (Test-Path *.nupkg) { Remove-Item *.nupkg -Recurse -Force } }"
powershell -command "& { if (Test-Path *.tmp) { Remove-Item *.tmp -Recurse -Force } }"
powershell -command "& { if (Test-Path .\Packages) { Remove-Item .\Packages -Recurse -Force } }"
powershell -command "& { if (Test-Path .\TestResults) { Remove-Item .\TestResults -Recurse -Force } }"

cd Bhbk.Lib.DataAccess.EF
powershell -command "& { if (Test-Path .\bin) { Remove-Item .\bin -Recurse -Force } }"
powershell -command "& { if (Test-Path .\obj) { Remove-Item .\obj -Recurse -Force } }"

cd ..\Bhbk.Lib.DataAccess.EF.Tests
powershell -command "& { if (Test-Path .\bin) { Remove-Item .\bin -Recurse -Force } }"
powershell -command "& { if (Test-Path .\obj) { Remove-Item .\obj -Recurse -Force } }"

cd..
