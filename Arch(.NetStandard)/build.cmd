
rem call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\Tools\VsDevCmd.bat"
rem dotnet tool install Octopus.DotNet.Cli --global
powershell -command "& { write-output 2020.11.13.2100 | out-file -filepath .\version.tmp -nonewline -encoding ascii }"
rem powershell -command "& { get-date -format yyyy.M.d.HHmm | out-file -filepath .\version.tmp -nonewline -encoding ascii }"
set /p VERSION=< .\version.tmp

rem build and test .net standard/core assemblies...
dotnet restore Bhbk.Lib.sln --no-cache --verbosity quiet
dotnet build Bhbk.Lib.sln --configuration Release --verbosity quiet /p:platform=x64
dotnet test Bhbk.Lib.Cryptography.Tests\Bhbk.Lib.Cryptography.Tests.csproj --configuration Release /p:platform=x64 /p:CollectCoverage=true /p:CoverletOutput=bin\Release\
dotnet test Bhbk.Lib.DataAccess.EFCore.Tests\Bhbk.Lib.DataAccess.EFCore.Tests.csproj --configuration Release /p:platform=x64 /p:CollectCoverage=true /p:CoverletOutput=bin\Release\
dotnet test Bhbk.Lib.DataState.Tests\Bhbk.Lib.DataState.Tests.csproj --configuration Release /p:platform=x64 /p:CollectCoverage=true /p:CoverletOutput=bin\Release\
dotnet test Bhbk.Lib.QueryExpression.Tests\Bhbk.Lib.QueryExpression.Tests.csproj --configuration Release /p:platform=x64 /p:CollectCoverage=true /p:CoverletOutput=bin\Release\
dotnet test Bhbk.Lib.Waf.Tests\Bhbk.Lib.Waf.Tests.csproj --configuration Release /p:platform=x64 /p:CollectCoverage=true /p:CoverletOutput=bin\Release\

rem package .net standard/core assemblies...
dotnet pack Bhbk.Lib.CommandLine\Bhbk.Lib.CommandLine.csproj -p:PackageVersion=%VERSION% --output . --configuration Release /p:platform=x64
dotnet pack Bhbk.Lib.Common\Bhbk.Lib.Common.csproj -p:PackageVersion=%VERSION% --output . --configuration Release /p:platform=x64
dotnet pack Bhbk.Lib.Cryptography\Bhbk.Lib.Cryptography.csproj -p:PackageVersion=%VERSION% --output . --configuration Release /p:platform=x64
dotnet pack Bhbk.Lib.DataAccess.EFCore\Bhbk.Lib.DataAccess.EFCore.csproj -p:PackageVersion=%VERSION% --output . --configuration Release /p:platform=x64
dotnet pack Bhbk.Lib.DataState\Bhbk.Lib.DataState.csproj -p:PackageVersion=%VERSION% --output . --configuration Release /p:platform=x64
dotnet pack Bhbk.Lib.Hosting\Bhbk.Lib.Hosting.csproj -p:PackageVersion=%VERSION% --output . --configuration Release /p:platform=x64
dotnet pack Bhbk.Lib.QueryExpression\Bhbk.Lib.QueryExpression.csproj -p:PackageVersion=%VERSION% --output . --configuration Release /p:platform=x64
dotnet pack Bhbk.Lib.Waf\Bhbk.Lib.Waf.csproj -p:PackageVersion=%VERSION% --output . --configuration Release /p:platform=x64

set VERSION=
rem dotnet tool uninstall Octopus.DotNet.Cli --global
rem powershell -command "& { update-package -reinstall }"
