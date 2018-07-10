
Set-Variable -Name version -Value "2018.7.9.1439"

cd .\Bhbk.Lib.Helpers
dotnet build "Bhbk.Lib.Helpers.csproj" --configuration Release --framework netcoreapp2.1
dotnet pack "Bhbk.Lib.Helpers.csproj" --configuration Release -p:PackageVersion=$version --output "..\\" -p:TargetFrameworks=netcoreapp2.1
#dotnet publish "Bhbk.Lib.Helpers.csproj" --output "bin\\Release\\netcoreapp2.1\\publish\\" --configuration Release --framework netcoreapp2.1
#octo pack --id="Bhbk.Lib.Helpers" --version=$version --basePath="bin\\Release\\netcoreapp2.1\\publish\\" --outFolder="..\\"

cd ..\Bhbk.Lib.Waf
dotnet build "Bhbk.Lib.Waf.csproj" --configuration Release --framework netcoreapp2.1
#dotnet pack "Bhbk.Lib.Waf.csproj" --configuration Release -p:PackageVersion=$version --output "..\\" -p:TargetFrameworks=netcoreapp2.1
dotnet publish "Bhbk.Lib.Waf.csproj" --output "bin\\Release\\netcoreapp2.1\\publish\\" --configuration Release --framework netcoreapp2.1
octo pack --id="Bhbk.Lib.Waf" --version=$version --basePath="bin\\Release\\netcoreapp2.1\\publish\\" --outFolder="..\\"

cd ..
