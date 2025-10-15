# Convex Shared Libraries

Professional shared libraries for Convex microservices architecture.

## 🚀 Overview

This repository contains a comprehensive set of shared libraries designed for modern microservices architecture. All libraries are built with .NET 9.0 and follow enterprise-grade patterns and practices.

## 📦 Libraries

### Core Libraries

| Library | Description | NuGet Package |
|---------|-------------|---------------|
| **Convex.Shared.Common** | Base models, DTOs, and utilities | `Convex.Shared.Common` |
| **Convex.Shared.Http** | HTTP client utilities | `Convex.Shared.Http` |
| **Convex.Shared.Logging** | Structured logging with Serilog | `Convex.Shared.Logging` |
| **Convex.Shared.Security** | JWT, API keys, and security utilities | `Convex.Shared.Security` |
| **Convex.Shared.Validation** | FluentValidation integration | `Convex.Shared.Validation` |
| **Convex.Shared.Caching** | Memory and Redis caching | `Convex.Shared.Caching` |
| **Convex.Shared.Messaging** | Apache Kafka messaging | `Convex.Shared.Messaging` |

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    Convex Shared Libraries                 │
├─────────────────────────────────────────────────────────────┤
│  Convex.Shared.Common    │  Base models, DTOs, extensions   │
│  Convex.Shared.Http     │  HTTP client utilities          │
│  Convex.Shared.Logging  │  Structured logging (Serilog)   │
│  Convex.Shared.Security │  JWT, API keys, authentication │
│  Convex.Shared.Validation│ FluentValidation integration    │
│  Convex.Shared.Caching  │  Memory & Redis caching         │
│  Convex.Shared.Messaging│ Apache Kafka messaging          │
└─────────────────────────────────────────────────────────────┘
```

## 🚀 Quick Start

### 1. Install Packages

```xml
<PackageReference Include="Convex.Shared.Common" Version="1.0.0" />
<PackageReference Include="Convex.Shared.Http" Version="1.0.0" />
<PackageReference Include="Convex.Shared.Logging" Version="1.0.0" />
<PackageReference Include="Convex.Shared.Security" Version="1.0.0" />
<PackageReference Include="Convex.Shared.Validation" Version="1.0.0" />
<PackageReference Include="Convex.Shared.Caching" Version="1.0.0" />
<PackageReference Include="Convex.Shared.Messaging" Version="1.0.0" />
```

### 2. Configure Services

```csharp
// Program.cs
services.AddConvexLogging("UserService", "1.0.0");
services.AddConvexSecurity(options =>
{
    options.JwtSecret = "your-jwt-secret";
    options.JwtIssuer = "Convex";
});
services.AddConvexValidation();
services.AddConvexRedisCache("localhost:6379");
services.AddConvexMessaging(options =>
{
    options.BootstrapServers = "localhost:9092";
    options.ConsumerGroup = "convex-group";
});
```

### 3. Use in Your Services

```csharp
public class UserService
{
    private readonly IConvexLogger _logger;
    private readonly IConvexCache _cache;
    private readonly IConvexMessageBus _messageBus;
    private readonly IConvexSecurityService _security;

    public UserService(
        IConvexLogger logger,
        IConvexCache cache,
        IConvexMessageBus messageBus,
        IConvexSecurityService security)
    {
        _logger = logger;
        _cache = cache;
        _messageBus = messageBus;
        _security = security;
    }

    public async Task<User> CreateUserAsync(CreateUserRequest request)
    {
        _logger.LogInformation("Creating user {Email}", request.Email);
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = _security.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        // Cache user
        await _cache.SetAsync($"user:{user.Id}", user, TimeSpan.FromMinutes(30));
        
        // Publish event
        await _messageBus.PublishAsync("user.created", new UserCreatedEvent
        {
            UserId = user.Id,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        });

        _logger.LogInformation("User {UserId} created successfully", user.Id);
        return user;
    }
}
```

## 🔧 Configuration

### appsettings.json

```json
{
  "ConvexLogging": {
    "MinimumLevel": "Information",
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {ServiceName} {Message:lj}"
        }
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/convex-.txt",
          "rollingInterval": "Day"
        }
      }
    ]
  },
  "ConvexSecurity": {
    "JwtSecret": "your-jwt-secret-key",
    "JwtIssuer": "Convex",
    "JwtAudience": "ConvexUsers",
    "JwtExpirationMinutes": 60
  },
  "ConvexMessaging": {
    "BootstrapServers": "localhost:9092",
    "SecurityProtocol": "PLAINTEXT",
    "ConsumerGroup": "convex-group",
    "TopicPrefix": "convex"
  },
  "ConnectionStrings": {
    "Redis": "localhost:6379"
  }
}
```

## 🏭 Production Features

### Security
- ✅ JWT token generation and validation
- ✅ API key management
- ✅ Password hashing with salt
- ✅ CORS configuration
- ✅ Rate limiting support

### Logging
- ✅ Structured logging with Serilog
- ✅ Performance metrics
- ✅ Business event logging
- ✅ Correlation ID tracking
- ✅ Multiple sinks (Console, File, Seq)

### Caching
- ✅ Memory caching
- ✅ Redis distributed caching
- ✅ JSON serialization
- ✅ GetOrSet pattern
- ✅ Bulk operations

### Messaging
- ✅ Apache Kafka integration
- ✅ Topic publishing/subscribing
- ✅ Consumer groups
- ✅ Message headers
- ✅ Error handling and retry

### Validation
- ✅ FluentValidation integration
- ✅ Email, phone, password validation
- ✅ Custom validators
- ✅ Async validation support

## 📚 Documentation

Each library includes comprehensive documentation:

- [Convex.Shared.Common](src/Convex.Shared.Common/README.md) - Base models and utilities
- [Convex.Shared.Http](src/Convex.Shared.Http/README.md) - HTTP client utilities
- [Convex.Shared.Logging](src/Convex.Shared.Logging/README.md) - Structured logging
- [Convex.Shared.Security](src/Convex.Shared.Security/README.md) - Security and authentication
- [Convex.Shared.Validation](src/Convex.Shared.Validation/README.md) - Validation utilities
- [Convex.Shared.Caching](src/Convex.Shared.Caching/README.md) - Caching utilities
- [Convex.Shared.Messaging](src/Convex.Shared.Messaging/README.md) - Kafka messaging

## 🚀 Building and Publishing

### Build All Libraries

```bash
dotnet build Convex.Shared.sln
```

### Publish NuGet Packages

```bash
dotnet pack Convex.Shared.sln --configuration Release
```

### Run Tests

```bash
dotnet test Convex.Shared.sln
```

## 🏗️ Development

### Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022 or VS Code
- Docker (for Redis and Kafka)

### Local Development Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/convex/shared-libraries.git
   cd shared-libraries
   ```

2. **Start dependencies**
   ```bash
   docker-compose up -d redis kafka
   ```

3. **Build and test**
   ```bash
   dotnet build
   dotnet test
   ```

## 📋 Roadmap

- [ ] **Convex.Shared.Metrics** - Prometheus metrics
- [ ] **Convex.Shared.Tracing** - OpenTelemetry tracing
- [ ] **Convex.Shared.Configuration** - Configuration management
- [ ] **Convex.Shared.HealthChecks** - Health check utilities
- [ ] **Convex.Shared.Monitoring** - Application monitoring

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

- 📧 Email: support@convex.com
- 💬 Discord: [Convex Community](https://discord.gg/convex)
- 📖 Documentation: [docs.convex.com](https://docs.convex.com)

---

**Built with ❤️ by the Convex Team**"# con-shred-lib" 
