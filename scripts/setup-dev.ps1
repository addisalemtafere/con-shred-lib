# Development setup script for Convex Shared Libraries
param(
    [switch]$Docker,
    [switch]$Clean
)

Write-Host "🛠️ Setting up Convex Shared Libraries development environment..." -ForegroundColor Green

# Set error action preference
$ErrorActionPreference = "Stop"

# Clean if requested
if ($Clean) {
    Write-Host "🧹 Cleaning development environment..." -ForegroundColor Yellow
    
    # Stop and remove containers
    if (Get-Command docker -ErrorAction SilentlyContinue) {
        Write-Host "  Stopping Docker containers..." -ForegroundColor Yellow
        docker-compose down -v
    }
    
    # Clean build artifacts
    Write-Host "  Cleaning build artifacts..." -ForegroundColor Yellow
    dotnet clean Convex.Shared.sln
    Remove-Item -Path "./packages" -Recurse -Force -ErrorAction SilentlyContinue
    Remove-Item -Path "./logs" -Recurse -Force -ErrorAction SilentlyContinue
}

# Check prerequisites
Write-Host "🔍 Checking prerequisites..." -ForegroundColor Yellow

# Check .NET 9.0
$dotnetVersion = dotnet --version
if ($LASTEXITCODE -ne 0) {
    Write-Error ".NET SDK not found. Please install .NET 9.0 SDK."
    exit 1
}
Write-Host "  ✅ .NET SDK: $dotnetVersion" -ForegroundColor Green

# Check Docker if requested
if ($Docker) {
    if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
        Write-Error "Docker not found. Please install Docker Desktop."
        exit 1
    }
    Write-Host "  ✅ Docker: $(docker --version)" -ForegroundColor Green
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
dotnet build Convex.Shared.sln
if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed"
    exit 1
}

# Start Docker services if requested
if ($Docker) {
    Write-Host "🐳 Starting Docker services..." -ForegroundColor Yellow
    docker-compose up -d
    
    # Wait for services to be ready
    Write-Host "⏳ Waiting for services to be ready..." -ForegroundColor Yellow
    Start-Sleep -Seconds 10
    
    # Check Redis
    Write-Host "  Checking Redis..." -ForegroundColor Yellow
    $redisReady = $false
    for ($i = 0; $i -lt 30; $i++) {
        try {
            $result = docker exec convex-redis redis-cli ping
            if ($result -eq "PONG") {
                $redisReady = $true
                break
            }
        } catch {
            Start-Sleep -Seconds 2
        }
    }
    
    if ($redisReady) {
        Write-Host "  ✅ Redis is ready" -ForegroundColor Green
    } else {
        Write-Warning "  ⚠️ Redis may not be ready yet"
    }
    
    # Check Kafka
    Write-Host "  Checking Kafka..." -ForegroundColor Yellow
    $kafkaReady = $false
    for ($i = 0; $i -lt 30; $i++) {
        try {
            $result = docker exec convex-kafka kafka-topics --bootstrap-server localhost:9092 --list
            if ($LASTEXITCODE -eq 0) {
                $kafkaReady = $true
                break
            }
        } catch {
            Start-Sleep -Seconds 2
        }
    }
    
    if ($kafkaReady) {
        Write-Host "  ✅ Kafka is ready" -ForegroundColor Green
    } else {
        Write-Warning "  ⚠️ Kafka may not be ready yet"
    }
}

# Create directories
Write-Host "📁 Creating directories..." -ForegroundColor Yellow
New-Item -ItemType Directory -Path "./logs" -Force | Out-Null
New-Item -ItemType Directory -Path "./packages" -Force | Out-Null

Write-Host "✅ Development environment setup completed!" -ForegroundColor Green
Write-Host ""
Write-Host "🚀 Next steps:" -ForegroundColor Cyan
Write-Host "  1. Run tests: dotnet test" -ForegroundColor White
Write-Host "  2. Build packages: .\scripts\build.ps1 -Pack" -ForegroundColor White
Write-Host "  3. Start services: docker-compose up -d" -ForegroundColor White
Write-Host ""
Write-Host "📚 Documentation:" -ForegroundColor Cyan
Write-Host "  - README.md - Main documentation" -ForegroundColor White
Write-Host "  - src/*/README.md - Individual library docs" -ForegroundColor White
Write-Host ""
Write-Host "🌐 Services (if Docker started):" -ForegroundColor Cyan
Write-Host "  - Redis: localhost:6379" -ForegroundColor White
Write-Host "  - Kafka: localhost:9092" -ForegroundColor White
Write-Host "  - Kafka UI: http://localhost:8080" -ForegroundColor White
Write-Host "  - Seq: http://localhost:5341" -ForegroundColor White
