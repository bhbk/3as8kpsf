
rem call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\Tools\VsDevCmd.bat"
rem dotnet tool install Octopus.DotNet.Cli --global --version 5.2.2
powershell -command "& { get-date -format yyyy.M.d.HHmm | out-file -filepath .\version.tmp -nonewline -encoding ascii }"
set /p VERSION=< .\version.tmp

dotnet restore Bhbk.Lib.Core.sln --no-cache --verbosity minimal
dotnet build Bhbk.Lib.Core.sln --configuration Release --verbosity minimal
dotnet test Bhbk.Lib.Core.Tests\Bhbk.Lib.Core.Tests.csproj --configuration Release /p:CollectCoverage=true /p:CoverletOutput=bin\Release\
dotnet test Bhbk.Lib.Waf.Tests\Bhbk.Lib.Waf.Tests.csproj --configuration Release /p:CollectCoverage=true /p:CoverletOutput=bin\Release\
dotnet pack Bhbk.Lib.Core\Bhbk.Lib.Core.csproj -p:PackageVersion=%VERSION% --output .. --configuration Release -p:TargetFrameworks=netstandard2.0
dotnet pack Bhbk.Lib.Waf\Bhbk.Lib.Waf.csproj -p:PackageVersion=%VERSION% --output .. --configuration Release -p:TargetFrameworks=netstandard2.0

rem dotnet tool uninstall Octopus.DotNet.Cli --global
set VERSION=
