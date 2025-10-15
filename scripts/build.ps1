# Build script for Convex Shared Libraries
param(
    [string]$Configuration = "Release",
    [switch]$Pack,
    [switch]$Test,
    [switch]$Clean
)

Write-Host "🚀 Building Convex Shared Libraries..." -ForegroundColor Green

# Set error action preference
$ErrorActionPreference = "Stop"

# Clean if requested
if ($Clean) {
    Write-Host "🧹 Cleaning solution..." -ForegroundColor Yellow
    dotnet clean Convex.Shared.sln --configuration $Configuration
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Clean failed"
        exit 1
    }
}

# Restore packages
Write-Host "📦 Restoring packages..." -ForegroundColor Yellow
dotnet restore Convex.Shared.sln
if ($LASTEXITCODE -ne 0) {
    Write-Error "Package restore failed"
    exit 1
}

# Build solution
Write-Host "🔨 Building solution..." -ForegroundColor Yellow
dotnet build Convex.Shared.sln --configuration $Configuration --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed"
    exit 1
}

# Run tests if requested
if ($Test) {
    Write-Host "🧪 Running tests..." -ForegroundColor Yellow
    dotnet test Convex.Shared.sln --configuration $Configuration --no-build --verbosity normal
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Tests failed"
        exit 1
    }
}

# Pack if requested
if ($Pack) {
    Write-Host "📦 Creating NuGet packages..." -ForegroundColor Yellow
    dotnet pack Convex.Shared.sln --configuration $Configuration --no-build --output ./packages
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Pack failed"
        exit 1
    }
    
    Write-Host "✅ Packages created in ./packages directory" -ForegroundColor Green
}

Write-Host "✅ Build completed successfully!" -ForegroundColor Green