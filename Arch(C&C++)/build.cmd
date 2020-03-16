
rem call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\Tools\VsDevCmd.bat"
rem dotnet tool install Octopus.DotNet.Cli --global
powershell -command "& { write-output 2020.03.14.2130 | out-file -filepath .\version.tmp -nonewline -encoding ascii }"
rem powershell -command "& { get-date -format yyyy.M.d.HHmm | out-file -filepath .\version.tmp -nonewline -encoding ascii }"
set /p VERSION=< .\version.tmp

rem build and test c and c++ assemblies...
dotnet restore Bhbk.Lib.sln --no-cache --verbosity quiet
dotnet build Bhbk.Lib.sln --configuration Release --verbosity quiet /p:platform=x64

set VERSION=
rem dotnet tool uninstall Octopus.DotNet.Cli --global
rem powershell -command "& { update-package -reinstall }"
