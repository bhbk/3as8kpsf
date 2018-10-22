
Set-Variable -Name version -Value "2018.10.22.6314"

dotnet build "Bhbk.Lib.Core.sln" --configuration Release
dotnet pack "Bhbk.Lib.Core\Bhbk.Lib.Core.csproj" -p:PackageVersion=$version --output ".." --configuration Release -p:TargetFrameworks=netstandard2.0
dotnet pack "Bhbk.Lib.Waf\Bhbk.Lib.Waf.csproj" -p:PackageVersion=$version --output ".." --configuration Release -p:TargetFrameworks=netstandard2.0
