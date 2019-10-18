
rem call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\Tools\VsDevCmd.bat"
rem dotnet tool install Octopus.DotNet.Cli --global --version 5.2.2
powershell -command "& { get-date -format yyyy.M.d.HHmm | out-file -filepath .\version.tmp -nonewline -encoding ascii }"
set /p VERSION=< .\version.tmp

nuget restore Bhbk.Lib.Core.sln -NoCache -Verbosity quiet
msbuild /t:build Bhbk.Lib.Core.sln /p:Configuration=Release /verbosity:quiet

mstest /testcontainer:Bhbk.Lib.DataAccess.EF.Tests\bin\Release\Bhbk.Lib.DataAccess.EF.Tests.dll

nuget pack Bhbk.Lib.DataAccess.EF\Bhbk.Lib.DataAccess.EF.csproj -Version %VERSION% -OutputDirectory . -Properties Configuration=Release

rem dotnet tool uninstall Octopus.DotNet.Cli --global
set VERSION=

rem powershell -command "& { update-package -reinstall }"
