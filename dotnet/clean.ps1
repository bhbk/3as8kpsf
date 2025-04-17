#Requires -Version 5.1

$ErrorActionPreference = "SilentlyContinue"

# =============================================================================
# Configuration - Add/remove projects here
# =============================================================================
$AllProjects = @(
    "Bhbk.Lib.CommandLine",
    "Bhbk.Lib.Common",
    "Bhbk.Lib.Cryptography",
    "Bhbk.Lib.Cryptography.Tests",
    "Bhbk.Lib.DataAccess.EF",
    "Bhbk.Lib.DataAccess.EF.Tests",
    "Bhbk.Lib.DataState",
    "Bhbk.Lib.DataState.Tests",
    "Bhbk.Lib.Hosting",
    "Bhbk.Lib.QueryExpression",
    "Bhbk.Lib.QueryExpression.Tests",
    "Bhbk.Lib.Waf",
    "Bhbk.Lib.Waf.Tests",
    "Bhbk.WebApi.Sample"
)

# =============================================================================
# Helper Functions
# =============================================================================
function Write-Status {
    param([string]$Text)
    Write-Host "  $Text" -ForegroundColor Gray
}

# =============================================================================
# Clean Process
# =============================================================================
Write-Host "Cleaning..." -ForegroundColor Cyan

# Remove NuGet packages
$nupkgFiles = Get-ChildItem -Path "." -Filter "*.nupkg" -ErrorAction SilentlyContinue
if ($nupkgFiles) {
    Remove-Item "*.nupkg" -Force
    Write-Status "Removed $($nupkgFiles.Count) .nupkg file(s)"
}

# Remove bin/obj folders from each project
$removedCount = 0
foreach ($project in $AllProjects) {
    $binPath = Join-Path $project "bin"
    $objPath = Join-Path $project "obj"

    if (Test-Path $binPath) {
        Remove-Item $binPath -Recurse -Force
        $removedCount++
    }
    if (Test-Path $objPath) {
        Remove-Item $objPath -Recurse -Force
        $removedCount++
    }
}

if ($removedCount -gt 0) {
    Write-Status "Removed $removedCount bin/obj folder(s)"
}

Write-Host "Clean completed" -ForegroundColor Green
