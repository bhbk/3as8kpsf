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
    "Bhbk.Lib.DataAccess.EF"
)

$TestProjects = @(
    "Bhbk.Lib.DataAccess.EF.Tests"
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

function Write-Info {
    param([string]$Text)
    Write-Host "  $Text" -ForegroundColor Gray
}

function Test-ExitCode {
    param([string]$Operation)
    if ($LASTEXITCODE -ne 0) {
        Write-Failure $Operation
        exit $LASTEXITCODE
    }
    Write-Success $Operation
}

function Get-VsWherePath {
    $vswherePaths = @(
        "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe",
        "${env:ProgramFiles}\Microsoft Visual Studio\Installer\vswhere.exe"
    )
    foreach ($path in $vswherePaths) {
        if (Test-Path $path) {
            return $path
        }
    }
    return $null
}

function Get-MsBuildPath {
    $vswhere = Get-VsWherePath
    if (-not $vswhere) {
        Write-Failure "vswhere.exe not found. Please install Visual Studio."
        exit 1
    }

    $msbuildPath = & $vswhere -latest -requires Microsoft.Component.MSBuild -find "MSBuild\**\Bin\MSBuild.exe" | Select-Object -First 1
    if (-not $msbuildPath -or -not (Test-Path $msbuildPath)) {
        Write-Failure "MSBuild.exe not found. Please install Visual Studio with .NET desktop development workload."
        exit 1
    }
    return $msbuildPath
}

function Get-VsTestPath {
    $vswhere = Get-VsWherePath
    if (-not $vswhere) {
        Write-Failure "vswhere.exe not found. Please install Visual Studio."
        exit 1
    }

    $vstestPath = & $vswhere -latest -find "Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" | Select-Object -First 1
    if (-not $vstestPath -or -not (Test-Path $vstestPath)) {
        Write-Failure "vstest.console.exe not found. Please install Visual Studio with test tools."
        exit 1
    }
    return $vstestPath
}

function Get-NuGetPath {
    # Check if nuget is in PATH
    $nuget = Get-Command nuget.exe -ErrorAction SilentlyContinue
    if ($nuget) {
        return $nuget.Source
    }

    # Check common locations
    $commonPaths = @(
        "$env:LOCALAPPDATA\Microsoft\WinGet\Links\nuget.exe",
        "$env:ProgramFiles\NuGet\nuget.exe",
        "${env:ProgramFiles(x86)}\NuGet\nuget.exe"
    )
    foreach ($path in $commonPaths) {
        if (Test-Path $path) {
            return $path
        }
    }

    return $null
}

function Install-NuGet {
    Write-Info "nuget.exe not found. Installing via winget..."
    winget install Microsoft.NuGet --silent --accept-package-agreements --accept-source-agreements 2>&1 | Out-Null
    if ($LASTEXITCODE -ne 0) {
        Write-Failure "Failed to install nuget.exe via winget"
        exit 1
    }

    # Refresh PATH
    $env:Path = [System.Environment]::GetEnvironmentVariable("Path", "Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path", "User")

    $nugetPath = Get-NuGetPath
    if (-not $nugetPath) {
        Write-Failure "nuget.exe still not found after installation. Please restart your terminal."
        exit 1
    }
    Write-Success "nuget.exe installed"
    return $nugetPath
}

# =============================================================================
# Tool Detection
# =============================================================================
Write-Header "Tool Detection"

$NuGetExe = Get-NuGetPath
if (-not $NuGetExe) {
    $NuGetExe = Install-NuGet
} else {
    Write-Success "nuget.exe found"
}

$MSBuildExe = Get-MsBuildPath
Write-Success "msbuild.exe found"

if ($RunTests) {
    $VSTestExe = Get-VsTestPath
    Write-Success "vstest.console.exe found"
}

# =============================================================================
# Main Build Process
# =============================================================================
$StartTime = Get-Date

Write-Host ""
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

# Restore .NET Standard dependencies via dotnet
dotnet restore $SolutionFile --no-cache --verbosity quiet 2>&1 | Out-Null
Test-ExitCode "Solution restore (dotnet)"

# Restore .NET Framework projects via nuget
foreach ($project in ($LibraryProjects + $TestProjects)) {
    $projectPath = "$project\$project.csproj"
    & $NuGetExe restore $projectPath -SolutionDirectory . -NoCache -Verbosity quiet 2>&1 | Out-Null
    Test-ExitCode "$project restore"
}

# -----------------------------------------------------------------------------
# Build
# -----------------------------------------------------------------------------
Write-Header "Build"
& $MSBuildExe $SolutionFile /p:Configuration=Release /verbosity:quiet /nologo 2>&1 | Out-Null
Test-ExitCode "Solution build"

# -----------------------------------------------------------------------------
# Test (optional)
# -----------------------------------------------------------------------------
if ($RunTests) {
    Write-Header "Test"
    foreach ($project in $TestProjects) {
        $testDll = "$project\bin\Release\$project.dll"
        if (Test-Path $testDll) {
            & $VSTestExe $testDll /Logger:console`;verbosity=minimal 2>&1 | Out-Null
            Test-ExitCode $project
        } else {
            Write-Failure "$project - test DLL not found"
            exit 1
        }
    }
}

# -----------------------------------------------------------------------------
# Pack (only if -Version provided)
# -----------------------------------------------------------------------------
if ($Version) {
    Write-Header "Pack"
    foreach ($project in $LibraryProjects) {
        $projectPath = "$project\$project.csproj"
        & $NuGetExe pack $projectPath -Version $Version -OutputDirectory . -Properties Configuration=Release -Verbosity quiet 2>&1 | Out-Null
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
