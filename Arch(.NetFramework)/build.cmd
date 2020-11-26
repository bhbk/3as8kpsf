
rem call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\Tools\VsDevCmd.bat"
rem dotnet tool install Octopus.DotNet.Cli --global
powershell -command "& { write-output 2020.11.25.1900 | out-file -filepath .\version.tmp -nonewline -encoding ascii }"
rem powershell -command "& { get-date -format yyyy.M.d.HHmm | out-file -filepath .\version.tmp -nonewline -encoding ascii }"
set /p VERSION=< .\version.tmp

rem build and test .net framework assemblies...
dotnet restore Bhbk.Lib.sln --no-cache --verbosity quiet
nuget restore Bhbk.Lib.DataAccess.EF\Bhbk.Lib.DataAccess.EF.csproj -SolutionDirectory . -NoCache -Verbosity quiet
nuget restore Bhbk.Lib.DataAccess.EF.Tests\Bhbk.Lib.DataAccess.EF.Tests.csproj -SolutionDirectory . -NoCache -Verbosity quiet
dotnet build Bhbk.Lib.sln --configuration Release --verbosity quiet /p:platform=x64
dotnet test Bhbk.Lib.DataAccess.EF.Tests\Bhbk.Lib.DataAccess.EF.Tests.csproj --configuration Release /p:platform=x64

rem package .net framework assemblies...
nuget pack Bhbk.Lib.DataAccess.EF\Bhbk.Lib.DataAccess.EF.csproj -Version %VERSION% -OutputDirectory . -Properties Configuration=Release -Properties Platform=x64

set VERSION=
rem dotnet tool uninstall Octopus.DotNet.Cli --global
rem powershell -command "& { update-package -reinstall }"
