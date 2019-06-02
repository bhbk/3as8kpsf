
powershell -command "& { Remove-Item *.nupkg }"
powershell -command "& { Remove-Item *.tmp }"

cd Bhbk.Lib.CommandLine
powershell -command "& { if (Test-Path .\bin) { Remove-Item .\bin -Recurse -Force } }"
powershell -command "& { if (Test-Path .\obj) { Remove-Item .\obj -Recurse -Force } }"

cd ..\Bhbk.Lib.Common
powershell -command "& { if (Test-Path .\bin) { Remove-Item .\bin -Recurse -Force } }"
powershell -command "& { if (Test-Path .\obj) { Remove-Item .\obj -Recurse -Force } }"

cd ..\Bhbk.Lib.Cryptography
powershell -command "& { if (Test-Path .\bin) { Remove-Item .\bin -Recurse -Force } }"
powershell -command "& { if (Test-Path .\obj) { Remove-Item .\obj -Recurse -Force } }"

cd ..\Bhbk.Lib.DataAccess
powershell -command "& { if (Test-Path .\bin) { Remove-Item .\bin -Recurse -Force } }"
powershell -command "& { if (Test-Path .\obj) { Remove-Item .\obj -Recurse -Force } }"

cd ..\Bhbk.Lib.Hosting
powershell -command "& { if (Test-Path .\bin) { Remove-Item .\bin -Recurse -Force } }"
powershell -command "& { if (Test-Path .\obj) { Remove-Item .\obj -Recurse -Force } }"

cd ..\Bhbk.Lib.Paging
powershell -command "& { if (Test-Path .\bin) { Remove-Item .\bin -Recurse -Force } }"
powershell -command "& { if (Test-Path .\obj) { Remove-Item .\obj -Recurse -Force } }"

cd ..\Bhbk.Lib.Waf
powershell -command "& { if (Test-Path .\bin) { Remove-Item .\bin -Recurse -Force } }"
powershell -command "& { if (Test-Path .\obj) { Remove-Item .\obj -Recurse -Force } }"

cd ..\Bhbk.Lib.Waf.Tests
powershell -command "& { if (Test-Path .\bin) { Remove-Item .\bin -Recurse -Force } }"
powershell -command "& { if (Test-Path .\obj) { Remove-Item .\obj -Recurse -Force } }"

cd ..\Bhbk.WebApi.Sample
powershell -command "& { if (Test-Path .\bin) { Remove-Item .\bin -Recurse -Force } }"
powershell -command "& { if (Test-Path .\obj) { Remove-Item .\obj -Recurse -Force } }"

cd..
