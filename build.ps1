
Set-Variable -Name version -Value "2018.7.13.3291"

dotnet build "Bhbk.Lib.Core.sln" --configuration Release
dotnet pack "Bhbk.Lib.Helpers\Bhbk.Lib.Helpers.csproj" -p:PackageVersion=$version --output ".." --configuration Release -p:TargetFrameworks=netstandard2.0
dotnet pack "Bhbk.Lib.Waf\Bhbk.Lib.Waf.csproj" -p:PackageVersion=$version --output ".." --configuration Release -p:TargetFrameworks=netstandard2.0
