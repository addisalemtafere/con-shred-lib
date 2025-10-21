# Simple Build Script
Write-Host "Building Convex Shared Libraries..." -ForegroundColor Green

dotnet build --configuration Release
dotnet pack --configuration Release --output ./packages --no-build

Write-Host "Build complete! Packages in ./packages folder" -ForegroundColor Green
