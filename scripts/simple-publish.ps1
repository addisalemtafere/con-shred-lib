# Simple Publish Script
param(
    [string]$ApiKey = $env:NUGET_API_KEY
)

Write-Host "Publishing to NuGet.org..." -ForegroundColor Green

Get-ChildItem ./packages/*.nupkg | ForEach-Object { 
    dotnet nuget push $_.FullName --api-key $ApiKey --source https://api.nuget.org/v3/index.json --skip-duplicate
}

Write-Host "Publish complete!" -ForegroundColor Green
