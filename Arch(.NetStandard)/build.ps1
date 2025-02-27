#Requires -Version 5.1
param(
    [string]$Version,
    [switch]$RunTests
)

$ErrorActionPreference = "Stop"

# =============================================================================
# Configuration - Add/remove projects here
# =============================================================================
$SolutionFile = "Bhbk.Lib.sln"

$LibraryProjects = @(
    "Bhbk.Lib.CommandLine",
    "Bhbk.Lib.Common",
    "Bhbk.Lib.Cryptography",
    "Bhbk.Lib.DataAccess.EFCore",
    "Bhbk.Lib.DataState",
    "Bhbk.Lib.Hosting",
    "Bhbk.Lib.QueryExpression",
    "Bhbk.Lib.Waf"
)

$TestProjects = @(
    "Bhbk.Lib.Cryptography.Tests",
    "Bhbk.Lib.DataAccess.EFCore.Tests",
    "Bhbk.Lib.DataState.Tests",
    "Bhbk.Lib.QueryExpression.Tests",
    "Bhbk.Lib.Waf.Tests"
)

# =============================================================================
# Helper Functions
# =============================================================================
function Write-Header {
    param([string]$Text)
    Write-Host ""
    Write-Host "=== $Text ===" -ForegroundColor Cyan
}

function Write-Success {
    param([string]$Text)
    Write-Host "  [OK] $Text" -ForegroundColor Green
}

function Write-Failure {
    param([string]$Text)
    Write-Host "  [FAIL] $Text" -ForegroundColor Red
}

function Test-ExitCode {
    param([string]$Operation)
    if ($LASTEXITCODE -ne 0) {
        Write-Failure $Operation
        exit $LASTEXITCODE
    }
    Write-Success $Operation
}

# =============================================================================
# Main Build Process
# =============================================================================
$StartTime = Get-Date

Write-Host "Build started at $($StartTime.ToString('HH:mm:ss'))" -ForegroundColor White
if ($Version) {
    Write-Host "Version: $Version" -ForegroundColor White
}
if ($RunTests) {
    Write-Host "Tests: Enabled" -ForegroundColor White
}

# -----------------------------------------------------------------------------
# Restore
# -----------------------------------------------------------------------------
Write-Header "Restore"
dotnet restore $SolutionFile --no-cache --verbosity quiet 2>&1 | Out-Null
Test-ExitCode "Solution restore"

# -----------------------------------------------------------------------------
# Build
# -----------------------------------------------------------------------------
Write-Header "Build"
dotnet build $SolutionFile --configuration Release --verbosity quiet --no-restore 2>&1 | Out-Null
Test-ExitCode "Solution build"

# -----------------------------------------------------------------------------
# Test (optional)
# -----------------------------------------------------------------------------
if ($RunTests) {
    Write-Header "Test"
    foreach ($project in $TestProjects) {
        $projectPath = "$project\$project.csproj"
        dotnet test $projectPath --configuration Release --no-build --verbosity quiet 2>&1 | Out-Null
        Test-ExitCode $project
    }
}

# -----------------------------------------------------------------------------
# Pack (only if -Version provided)
# -----------------------------------------------------------------------------
if ($Version) {
    Write-Header "Pack"
    foreach ($project in $LibraryProjects) {
        $projectPath = "$project\$project.csproj"
        dotnet pack $projectPath --output . --configuration Release --no-build /p:PackageVersion=$Version 2>&1 | Out-Null
        Test-ExitCode $project
    }
}

# -----------------------------------------------------------------------------
# Summary
# -----------------------------------------------------------------------------
$EndTime = Get-Date
$Duration = $EndTime - $StartTime

Write-Host ""
Write-Host "Build completed in $($Duration.TotalSeconds.ToString('0.0'))s" -ForegroundColor Green
