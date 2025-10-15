# Publish script for Convex Shared Libraries
param(
    [string]$ApiKey,
    [string]$Source = "https://api.nuget.org/v3/index.json",
    [string]$Configuration = "Release",
    [switch]$DryRun
)

Write-Host "📦 Publishing Convex Shared Libraries..." -ForegroundColor Green

# Set error action preference
$ErrorActionPreference = "Stop"

# Check if API key is provided
if (-not $ApiKey -and -not $DryRun) {
    Write-Error "API key is required for publishing. Use -ApiKey parameter or -DryRun for testing."
    exit 1
}

# Build and pack first
Write-Host "🔨 Building and packing..." -ForegroundColor Yellow
& "$PSScriptRoot\build.ps1" -Configuration $Configuration -Pack
if ($LASTEXITCODE -ne 0) {
    Write-Error "Build and pack failed"
    exit 1
}

# Get all package files
$packageFiles = Get-ChildItem -Path "./packages" -Filter "*.nupkg" -Recurse
if ($packageFiles.Count -eq 0) {
    Write-Error "No package files found in ./packages directory"
    exit 1
}

Write-Host "📦 Found $($packageFiles.Count) packages to publish:" -ForegroundColor Yellow
foreach ($package in $packageFiles) {
    Write-Host "  - $($package.Name)" -ForegroundColor Cyan
}

# Publish packages
foreach ($package in $packageFiles) {
    Write-Host "📤 Publishing $($package.Name)..." -ForegroundColor Yellow
    
    if ($DryRun) {
        Write-Host "  [DRY RUN] Would publish to: $Source" -ForegroundColor Magenta
    } else {
        $publishArgs = @(
            "nuget", "push", $package.FullName,
            "--api-key", $ApiKey,
            "--source", $Source,
            "--skip-duplicate"
        )
        
        & dotnet @publishArgs
        if ($LASTEXITCODE -ne 0) {
            Write-Error "Failed to publish $($package.Name)"
            exit 1
        }
    }
}

if ($DryRun) {
    Write-Host "✅ Dry run completed successfully!" -ForegroundColor Green
} else {
    Write-Host "✅ All packages published successfully!" -ForegroundColor Green
}