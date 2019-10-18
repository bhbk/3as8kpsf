
rem call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\Tools\VsDevCmd.bat"
rem dotnet tool install Octopus.DotNet.Cli --global --version 5.2.2
powershell -command "& { get-date -format yyyy.M.d.HHmm | out-file -filepath .\version.tmp -nonewline -encoding ascii }"
set /p VERSION=< .\version.tmp

dotnet restore Bhbk.Lib.Core.sln --no-cache --verbosity quiet
dotnet build Bhbk.Lib.Core.sln --configuration Release --verbosity quiet

dotnet test Bhbk.Lib.DataAccess.EFCore.Tests\Bhbk.Lib.DataAccess.EFCore.Tests.csproj --configuration Release /p:CollectCoverage=true /p:CoverletOutput=bin\Release\
dotnet test Bhbk.Lib.DataState.Tests\Bhbk.Lib.DataState.Tests.csproj --configuration Release /p:CollectCoverage=true /p:CoverletOutput=bin\Release\
dotnet test Bhbk.Lib.Waf.Tests\Bhbk.Lib.Waf.Tests.csproj --configuration Release /p:CollectCoverage=true /p:CoverletOutput=bin\Release\

dotnet pack Bhbk.Lib.CommandLine\Bhbk.Lib.CommandLine.csproj -p:PackageVersion=%VERSION% --output . --configuration Release
dotnet pack Bhbk.Lib.Common\Bhbk.Lib.Common.csproj -p:PackageVersion=%VERSION% --output . --configuration Release
dotnet pack Bhbk.Lib.Cryptography\Bhbk.Lib.Cryptography.csproj -p:PackageVersion=%VERSION% --output . --configuration Release
dotnet pack Bhbk.Lib.DataAccess.EFCore\Bhbk.Lib.DataAccess.EFCore.csproj -p:PackageVersion=%VERSION% --output . --configuration Release
dotnet pack Bhbk.Lib.DataState\Bhbk.Lib.DataState.csproj -p:PackageVersion=%VERSION% --output . --configuration Release
dotnet pack Bhbk.Lib.Hosting\Bhbk.Lib.Hosting.csproj -p:PackageVersion=%VERSION% --output . --configuration Release
dotnet pack Bhbk.Lib.Waf\Bhbk.Lib.Waf.csproj -p:PackageVersion=%VERSION% --output . --configuration Release

rem dotnet tool uninstall Octopus.DotNet.Cli --global
set VERSION=

rem powershell -command "& { update-package -reinstall }"
