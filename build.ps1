
Set-Variable -Name version -Value "2018.7.11.2159"

cd .\Bhbk.Lib.Helpers
dotnet build "Bhbk.Lib.Helpers.csproj" --configuration Release --framework netcoreapp2.1
dotnet pack "Bhbk.Lib.Helpers.csproj" --configuration Release -p:PackageVersion=$version --output "..\\" -p:TargetFrameworks=netcoreapp2.1

cd ..\Bhbk.Lib.Waf
dotnet build "Bhbk.Lib.Waf.csproj" --configuration Release --framework netcoreapp2.1
dotnet pack "Bhbk.Lib.Waf.csproj" --configuration Release -p:PackageVersion=$version --output "..\\" -p:TargetFrameworks=netcoreapp2.1

cd ..
