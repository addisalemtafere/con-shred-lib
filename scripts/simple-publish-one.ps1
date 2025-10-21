# Simple Publish One Package Script
param(
    [string]$PackageName = "",
    [string]$ApiKey = $env:NUGET_API_KEY
)

if ([string]::IsNullOrWhiteSpace($PackageName)) {
    Write-Host "Available packages:" -ForegroundColor Yellow
    Get-ChildItem ./packages/*.nupkg | ForEach-Object { Write-Host "  $($_.Name)" -ForegroundColor White }
    Write-Host "`nUsage: .\simple-publish-one.ps1 -PackageName 'Convex.Shared.Business'" -ForegroundColor Yellow
    exit
}

$package = Get-ChildItem ./packages/*$PackageName*.nupkg | Select-Object -First 1

if (-not $package) {
    Write-Host "Package not found: $PackageName" -ForegroundColor Red
    exit
}

Write-Host "Publishing: $($package.Name)" -ForegroundColor Green
dotnet nuget push $package.FullName --api-key $ApiKey --source https://api.nuget.org/v3/index.json --skip-duplicate
Write-Host "Done!" -ForegroundColor Green
