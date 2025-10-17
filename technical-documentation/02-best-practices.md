# C# .NET 9 Microservices Best Practices

## Table of Contents
1. [Microservices Repository Strategy](#microservices-repository-strategy)
2. [Local NuGet Package Management](#local-nuget-package-management)
3. [Shared Libraries Strategy](#shared-libraries-strategy)
4. [Professional Shared Libraries as Separate NuGet Packages](#professional-shared-libraries-as-separate-nuget-packages)
5. [Asynchronous Programming Best Practices](#asynchronous-programming-best-practices)
6. [Logging & Exception Handling](#logging--exception-handling)
7. [Security Best Practices](#security-best-practices)
8. [API Design & Generic Response Patterns](#api-design--generic-response-patterns)
9. [ORM Best Practices](#orm-best-practices)
10. [CQRS Implementation](#cqrs-implementation)
11. [Validation Libraries](#validation-libraries)
12. [Localization & Error Handling](#localization--error-handling)
13. [Kafka Implementation](#kafka-implementation)
14. [gRPC Communication](#grpc-communication)
15. [Load Balancing & Scalability](#load-balancing--scalability)
16. [Clean Architecture Principles](#clean-architecture-principles)
17. [Database Design Patterns](#database-design-patterns)
18. [Caching Strategies](#caching-strategies)
19. [Message Queue Patterns](#message-queue-patterns)
20. [Monitoring & Observability](#monitoring--observability)
21. [Performance Optimization](#performance-optimization)
22. [Testing Strategies](#testing-strategies)
23. [Deployment Patterns](#deployment-patterns)
24. [Configuration Management](#configuration-management)

---

## Microservices Repository Strategy

### Convex Repository Structure for Large Teams (15+ Developers)

#### Shared Libraries Repository (`convex-shared`)
```
convex-shared/
├── Convex.Shared.sln                 # Shared libraries solution
├── src/
│   ├── Convex.Shared.Domain/         # Domain entities and interfaces
│   ├── Convex.Shared.Models/         # DTOs, enums, value objects
│   ├── Convex.Shared.Common/         # Common utilities
│   ├── Convex.Shared.Http/           # HTTP client utilities
│   ├── Convex.Shared.Logging/        # Structured logging
│   ├── Convex.Shared.Security/       # JWT, authentication
│   ├── Convex.Shared.Validation/     # Input validation
│   ├── Convex.Shared.Caching/        # Memory and Redis caching
│   ├── Convex.Shared.Messaging/      # Kafka messaging
│   ├── Convex.Shared.Utilities/     # Helper utilities
│   ├── Convex.Shared.Business/       # Business logic services
│   └── Convex.Shared.Grpc/           # gRPC client + server
├── tests/
│   ├── Convex.Shared.Domain.Tests/
│   ├── Convex.Shared.Models.Tests/
│   └── Convex.Shared.Validation.Tests/
├── .github/workflows/
│   ├── build-and-publish.yml
│   └── security-scan.yml
├── Directory.Packages.props
├── Directory.Build.props
└── README.md
```

#### Payment Service Repository (`convex-payment-service`)
```
convex-payment-service/
├── Convex.Payment.sln                # Payment service solution
├── src/
│   ├── Convex.Payment.API/           # Web API project
│   │   ├── Controllers/
│   │   │   ├── PaymentController.cs
│   │   │   ├── WalletController.cs
│   │   │   └── TransactionController.cs
│   │   ├── Middleware/
│   │   │   ├── ErrorHandlingMiddleware.cs
│   │   │   └── AuthenticationMiddleware.cs
│   │   ├── Filters/
│   │   │   ├── ValidationFilter.cs
│   │   │   └── AuthorizationFilter.cs
│   │   ├── Program.cs
│   │   └── appsettings.json
│   ├── Convex.Payment.Application/   # Application layer
│   │   ├── Commands/
│   │   ├── Queries/
│   │   ├── Handlers/
│   │   ├── Services/
│   │   └── DTOs/
│   ├── Convex.Payment.Domain/        # Domain layer
│   │   ├── Entities/
│   │   ├── ValueObjects/
│   │   ├── Interfaces/
│   │   └── Events/
│   └── Convex.Payment.Infrastructure/ # Infrastructure layer
│       ├── Data/
│       ├── Repositories/
│       ├── ExternalServices/
│       ├── Kafka/
│       ├── gRPC/
│       └── Services/
├── tests/
│   ├── Convex.Payment.UnitTests/
│   ├── Convex.Payment.IntegrationTests/
│   └── Convex.Payment.ContractTests/
├── docker/
│   ├── Dockerfile
│   ├── docker-compose.yml
│   └── docker-compose.override.yml
├── .github/workflows/
│   ├── build-and-test.yml
│   ├── deploy-staging.yml
│   └── deploy-production.yml
├── Directory.Packages.props
├── Directory.Build.props
└── README.md
```

### Repository Management Strategy
- **Separate Repositories**: Each microservice has its own repository
- **Shared Dependencies**: Use shared NuGet packages for common functionality
- **Version Control**: Independent versioning for each service
- **CI/CD**: Separate pipelines for each repository
- **Security**: Repository-level access controls

### Benefits for Large Teams
- **Independent Development**: Teams work on different services simultaneously
- **Technology Flexibility**: Different technologies per service
- **Focused Codebase**: Smaller, focused codebases
- **Faster CI/CD**: Build and test only what changed
- **Independent Deployment**: Deploy services separately
- **Scalability**: Scale services independently
- **Team Autonomy**: Clear service ownership

---

## Asynchronous Programming Best Practices

### Async/Await Patterns
- **Non-Blocking Operations**: Use async/await to prevent thread blocking during I/O operations
- **Database Calls**: Always use async methods for database operations (EF Core, Dapper)
- **HTTP Requests**: Use HttpClientFactory with async methods for external API calls
- **File I/O**: Use async file operations for reading/writing large files
- **Avoid Async Void**: Use async Task instead of async void (except for event handlers)
- **ConfigureAwait**: Use ConfigureAwait(false) in library code to avoid deadlocks

### Task Parallelism
- **Parallel Operations**: Use Task.WhenAll for independent parallel operations
- **Concurrent Processing**: Process multiple items concurrently when order doesn't matter
- **Max Parallelism**: Limit concurrent operations to avoid resource exhaustion
- **Cancellation Tokens**: Support cancellation for long-running async operations
- **Timeout Handling**: Implement timeouts for external service calls

### Thread Safety
- **Immutable Data**: Prefer immutable data structures for concurrent access
- **Thread-Safe Collections**: Use ConcurrentDictionary, ConcurrentQueue for shared data
- **Lock-Free Operations**: Use Interlocked operations for simple atomic updates
- **Async Locking**: Use SemaphoreSlim for async-compatible locking
- **Avoid Blocking**: Never use .Result or .Wait() on async tasks in async methods

### Performance Considerations
- **ValueTask**: Use ValueTask<T> for hot paths with frequent synchronous completion
- **Memory Allocation**: Minimize allocations in async hot paths
- **Thread Pool**: Don't exhaust thread pool with long-running CPU operations
- **Async All The Way**: Maintain async throughout the call stack
- **Background Tasks**: Use IHostedService or BackgroundService for background processing

### Common Async Pitfalls to Avoid
- **Deadlocks**: Avoid mixing sync and async code (no .Result/.Wait in async context)
- **Fire and Forget**: Always await or properly handle async tasks
- **Capturing Context**: Understand SynchronizationContext behavior
- **Exception Handling**: Use try-catch in async methods, exceptions on await
- **Async Over-Usage**: Don't use async for CPU-bound operations

---

## Logging & Exception Handling

### Structured Logging
- **Serilog**: Structured logging with correlation IDs
- **Log Levels**: Trace, Debug, Information, Warning, Error, Fatal
- **Security**: Never log passwords, tokens, or PII
- **Environment-Specific**: Different log levels per environment
- **Log Aggregation**: ELK stack for centralized logging
- **Correlation IDs**: Track requests across microservices

### Exception Handling Patterns
- **Global Exception Middleware**: Centralized error handling
- **Custom Exceptions**: Domain-specific exception types
- **Pattern Matching**: Use C# switch expressions for exception type handling
- **HTTP Status Mapping**: Map exception types to appropriate HTTP status codes
- **Exception Hierarchy**: ValidationException, NotFoundException, BusinessException, UnauthorizedException
- **Error Responses**: Consistent error response format with error codes
- **Logging Context**: Include correlation ID, user ID, request details in logs

### Custom Logging Format Examples

#### Structured Log Entry Format
```json
{
  "timestamp": "2024-01-20T10:30:45.123Z",
  "level": "Error",
  "correlationId": "corr-abc123-def456",
  "requestId": "req-789012",
  "userId": "user_12345",
  "service": "PaymentService",
  "environment": "Production",
  "message": "Payment processing failed",
  "exception": {
    "type": "PaymentException",
    "message": "Insufficient funds in wallet",
    "stackTrace": "...",
    "innerException": null
  },
  "context": {
    "paymentId": "pay_67890",
    "amount": 500.00,
    "currency": "ETB",
    "paymentMethod": "M-Pesa",
    "walletBalance": 250.00
  },
  "performance": {
    "duration": 1250,
    "endpoint": "/api/v1/payments/process"
  }
}
```

#### Error Response Format
```json
{
  "success": false,
  "timestamp": "2024-01-20T10:30:45.123Z",
  "correlationId": "corr-abc123-def456",
  "error": {
    "code": "PAY_001",
    "type": "PaymentException",
    "message": "Payment processing failed",
    "userMessage": "Insufficient funds. Please top up your wallet.",
    "details": {
      "paymentId": "pay_67890",
      "requiredAmount": 500.00,
      "availableBalance": 250.00,
      "currency": "ETB",
      "shortfall": 250.00
    },
    "suggestions": [
      "Top up your wallet with at least 250.00 ETB",
      "Use a different payment method",
      "Contact support if you believe this is an error"
    ]
  },
  "metadata": {
    "requestId": "req-789012",
    "service": "PaymentService",
    "version": "1.0",
    "environment": "Production"
  }
}
```

#### Validation Error Response Format
```json
{
  "success": false,
  "timestamp": "2024-01-20T10:30:45.123Z",
  "correlationId": "corr-abc123-def456",
  "error": {
    "code": "VALIDATION_ERROR",
    "type": "ValidationException",
    "message": "Request validation failed",
    "validationErrors": [
      {
        "field": "betAmount",
        "code": "MIN_VALUE",
        "message": "Bet amount must be at least 5.00 ETB",
        "attemptedValue": 2.00,
        "constraint": {
          "minimum": 5.00,
          "currency": "ETB"
        }
      },
      {
        "field": "gameId",
        "code": "INVALID_STATE",
        "message": "Game is not available for betting",
        "attemptedValue": "game_456",
        "constraint": {
          "reason": "Game has already started",
          "gameStatus": "IN_PROGRESS",
          "startTime": "2024-01-20T10:00:00Z"
        }
      },
      {
        "field": "userId",
        "code": "ACCOUNT_SUSPENDED",
        "message": "User account is suspended",
        "attemptedValue": "user_789",
        "constraint": {
          "reason": "Multiple failed payment attempts",
          "suspendedUntil": "2024-01-25T00:00:00Z",
          "contactSupport": true
        }
      }
    ]
  }
}
```

#### Business Exception Response Format
```json
{
  "success": false,
  "timestamp": "2024-01-20T10:30:45.123Z",
  "correlationId": "corr-abc123-def456",
  "error": {
    "code": "BET_002",
    "type": "BusinessException",
    "message": "Daily betting limit exceeded",
    "userMessage": "You have reached your daily betting limit of 10,000.00 ETB",
    "details": {
      "userId": "user_12345",
      "dailyLimit": 10000.00,
      "currentDailyTotal": 10000.00,
      "attemptedBet": 500.00,
      "currency": "ETB",
      "limitResetTime": "2024-01-21T00:00:00Z",
      "hoursUntilReset": 13.5
    },
    "actions": [
      {
        "type": "WAIT",
        "message": "Wait until limit resets at midnight",
        "availableAt": "2024-01-21T00:00:00Z"
      },
      {
        "type": "REQUEST_INCREASE",
        "message": "Request a limit increase",
        "url": "/api/v1/users/limits/increase"
      }
    ]
  }
}
```

#### Not Found Error Response Format
```json
{
  "success": false,
  "timestamp": "2024-01-20T10:30:45.123Z",
  "correlationId": "corr-abc123-def456",
  "error": {
    "code": "NOT_FOUND",
    "type": "NotFoundException",
    "message": "Resource not found",
    "userMessage": "The requested game could not be found",
    "details": {
      "resourceType": "Game",
      "resourceId": "game_999",
      "searchedAt": "2024-01-20T10:30:45.123Z"
    },
    "suggestions": [
      "Verify the game ID is correct",
      "Check if the game has been cancelled",
      "Browse available games at /api/v1/games"
    ]
  }
}
```

#### Unauthorized Error Response Format
```json
{
  "success": false,
  "timestamp": "2024-01-20T10:30:45.123Z",
  "correlationId": "corr-abc123-def456",
  "error": {
    "code": "AUTH_001",
    "type": "UnauthorizedException",
    "message": "Authentication required",
    "userMessage": "Your session has expired. Please log in again.",
    "details": {
      "reason": "TOKEN_EXPIRED",
      "expiredAt": "2024-01-20T09:30:45.123Z",
      "currentTime": "2024-01-20T10:30:45.123Z"
    },
    "actions": [
      {
        "type": "LOGIN",
        "message": "Log in to continue",
        "url": "/api/v1/auth/login"
      },
      {
        "type": "REFRESH_TOKEN",
        "message": "Refresh your access token",
        "url": "/api/v1/auth/refresh"
      }
    ]
  }
}
```

#### Rate Limit Error Response Format
```json
{
  "success": false,
  "timestamp": "2024-01-20T10:30:45.123Z",
  "correlationId": "corr-abc123-def456",
  "error": {
    "code": "RATE_LIMIT",
    "type": "RateLimitException",
    "message": "Rate limit exceeded",
    "userMessage": "Too many requests. Please try again later.",
    "details": {
      "limit": 100,
      "remaining": 0,
      "resetTime": "2024-01-20T10:31:00.000Z",
      "retryAfter": 15,
      "window": "1 minute"
    }
  },
  "headers": {
    "X-RateLimit-Limit": "100",
    "X-RateLimit-Remaining": "0",
    "X-RateLimit-Reset": "1705748460",
    "Retry-After": "15"
  }
}
```

---

## Security Best Practices
- **Authentication**: JWT tokens, OAuth 2.0/OpenID Connect, Multi-Factor Authentication
- **Authorization**: Role-Based Access Control (RBAC), API Keys for service-to-service
- **Data Protection**: Encryption at rest and in transit, Secrets management (Azure Key Vault)
- **Input Validation**: Validate all inputs, prevent SQL injection with parameterized queries
- **Security Headers**: CORS, HSTS, CSP, Rate limiting, Request validation
- **Compliance**: GDPR, PCI DSS, Audit logging, Data retention policies

---

## API Design & Generic Response Patterns

### RESTful API Design Principles
- **Resource-Based URLs**: Use nouns, not verbs in URLs
  - ✅ Good: `/api/v1/users`, `/api/v1/users/123`
  - ❌ Bad: `/api/v1/getUser`, `/api/v1/createUser`
- **HTTP Methods**: Use appropriate HTTP methods
  - `GET`: Retrieve resources
  - `POST`: Create new resources
  - `PUT`: Update entire resources
  - `PATCH`: Partial updates
  - `DELETE`: Remove resources
- **Status Codes**: Use proper HTTP status codes
  - `200 OK`: Successful GET, PUT, PATCH
  - `201 Created`: Successful POST
  - `204 No Content`: Successful DELETE
  - `400 Bad Request`: Client error
  - `401 Unauthorized`: Authentication required
  - `403 Forbidden`: Access denied
  - `404 Not Found`: Resource not found
  - `500 Internal Server Error`: Server error

### Generic Response Pattern

#### Success Response Structure
```json
{
  "success": true,
  "data": {
    "betId": "bet_123456",
    "userId": "user_789",
    "gameId": "game_456",
    "betAmount": 100.00,
    "currency": "ETB",
    "odds": 2.50,
    "potentialWin": 250.00,
    "status": "PENDING",
    "placedAt": "2024-01-20T10:30:00Z"
  },
  "message": "Bet placed successfully",
  "metadata": {
    "timestamp": "2024-01-20T10:30:00Z",
    "requestId": "req-123456",
    "version": "1.0",
    "correlationId": "corr-789012",
    "pagination": {
      "page": 1,
      "pageSize": 20,
      "totalPages": 5,
      "totalCount": 100,
      "hasNext": true,
      "hasPrevious": false
    }
  }
}
```

#### Error Response Structure
```json
{
  "success": false,
  "error": {
    "code": "BET_001",
    "message": "Insufficient funds for bet placement",
    "details": {
      "field": "betAmount",
      "reason": "Available balance: 50.00 ETB, Required: 100.00 ETB",
      "availableBalance": 50.00,
      "requiredAmount": 100.00,
      "currency": "ETB"
    },
    "timestamp": "2024-01-20T10:30:00Z",
    "requestId": "req-123456",
    "correlationId": "corr-789012"
  }
}
```

---

## Clean Architecture Principles

### Architecture Layers
- **Domain Layer**: Core business logic, entities, value objects
- **Application Layer**: Use cases, application services, DTOs
- **Infrastructure Layer**: Data access, external services, messaging
- **Presentation Layer**: Controllers, middleware, filters

### SOLID Principles
- **Single Responsibility**: Each class has one reason to change
- **Open/Closed**: Open for extension, closed for modification
- **Liskov Substitution**: Subtypes must be substitutable for base types
- **Interface Segregation**: Clients shouldn't depend on unused interfaces
- **Dependency Inversion**: Depend on abstractions, not concretions

### Dependency Injection Best Practices
- **Interface Segregation**: Define focused interfaces for abstraction
- **Constructor Injection**: Prefer constructor injection for required dependencies
- **Service Lifetimes**: Use appropriate lifetimes (Transient, Scoped, Singleton)
- **Avoid Service Locator**: Don't use service locator anti-pattern
- **Testability**: Design for easy unit testing with mocked dependencies

### Design Patterns
- **Repository Pattern**: Abstract data access
- **Unit of Work**: Manage transactions
- **Factory Pattern**: Create objects without specifying classes
- **Strategy Pattern**: Define family of algorithms
- **Observer Pattern**: Implement event-driven architecture

---

## Conclusion

This comprehensive guide provides all the essential best practices for building robust, scalable, and maintainable C# .NET 9 microservices for large development teams (15+ developers).

**Key Benefits for Large Teams:**
- **Independent Development**: Teams can work on different services simultaneously
- **Technology Flexibility**: Each service can use different technologies
- **Focused Codebase**: Smaller, more focused codebases
- **Faster CI/CD**: Only build and test what changed
- **Independent Deployment**: Deploy services independently
- **Scalability**: Scale services independently
- **Security**: Repository-level access controls
- **Team Autonomy**: Clear ownership of each service

**Implementation Strategy:**
- Use separate repositories for true microservices independence
- Implement shared libraries through NuGet packages
- Follow clean architecture principles with auditable entities
- Use hybrid data access (EF Core + Dapper) for optimal performance
- Implement asynchronous programming throughout the application stack
- Implement comprehensive logging with pattern matching exception handling
- Focus on security throughout the development process
- Use connection factory pattern for database management
- Implement database seeding for consistent development environments
- Design for horizontal scalability with load balancing strategies
- Implement resilience patterns (circuit breaker, retry, timeout)
- Use distributed caching for multi-server environments
- Use appropriate patterns for your specific use cases
- Continuously refactor and improve your codebase
- Keep documentation up to date
- Implement comprehensive testing strategies

These practices will help you build enterprise-grade .NET 9 microservices that are maintainable, scalable, secure, and performant for your large development team, capable of handling millions of users efficiently.

---

## Database Design Patterns

### Database Per Service Pattern
- **Service Isolation**: Each microservice owns its database
- **Technology Flexibility**: Different databases per service (PostgreSQL, MongoDB, Redis)
- **Data Consistency**: Use eventual consistency patterns
- **Schema Evolution**: Independent database schema changes
- **Backup Strategy**: Service-specific backup and recovery

### Database Connection Management
```csharp
// Connection Factory Pattern
public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
    Task<IDbConnection> CreateConnectionAsync();
}

public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    
    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    public IDbConnection CreateConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
    
    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}
```

### Database Seeding Pattern
```csharp
// Startup Database Seeding
public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // Seed roles
    if (!await context.Roles.AnyAsync())
    {
        var roles = new[]
        {
            new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Name = "User", NormalizedName = "USER" },
            new IdentityRole { Name = "Moderator", NormalizedName = "MODERATOR" }
        };
        
        context.Roles.AddRange(roles);
        await context.SaveChangesAsync();
    }
    
    // Seed admin user
    if (!await context.Users.AnyAsync(u => u.UserName == "admin"))
    {
        var adminUser = new ApplicationUser
        {
            UserName = "admin",
            Email = "admin@convex.com",
            EmailConfirmed = true
        };
        
        await userManager.CreateAsync(adminUser, "AdminPassword123!");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}
```

### Hybrid Data Access Pattern (EF Core + Dapper)
```csharp
// Commands (Write) - Use EF Core
public class PaymentCommandRepository : IPaymentCommandRepository
{
    private readonly PaymentDbContext _context;
    
    public async Task<Payment> CreatePaymentAsync(CreatePaymentCommand command)
    {
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            Amount = command.Amount,
            Currency = command.Currency,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }
}

// Queries (Read) - Use Dapper
public class PaymentQueryRepository : IPaymentQueryRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    
    public async Task<IEnumerable<PaymentDto>> GetPaymentsByUserIdAsync(Guid userId)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        
        const string sql = @"
            SELECT p.Id, p.Amount, p.Currency, p.Status, p.CreatedAt
            FROM Payments p
            WHERE p.UserId = @UserId
            ORDER BY p.CreatedAt DESC";
            
        return await connection.QueryAsync<PaymentDto>(sql, new { UserId = userId });
    }
}
```

---

## Caching Strategies

### Multi-Level Caching Architecture
```csharp
// Cache Service Interface
public interface ICacheService
{
    Task<T> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    Task RemoveAsync(string key);
    Task RemoveByPatternAsync(string pattern);
}

// Redis Cache Implementation
public class RedisCacheService : ICacheService
{
    private readonly IDatabase _database;
    private readonly IJsonSerializer _serializer;
    
    public async Task<T> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        return value.HasValue ? _serializer.Deserialize<T>(value) : default(T);
    }
    
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var serializedValue = _serializer.Serialize(value);
        await _database.StringSetAsync(key, serializedValue, expiration);
    }
}

// Memory Cache Implementation
public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly IJsonSerializer _serializer;
    
    public async Task<T> GetAsync<T>(string key)
    {
        _cache.TryGetValue(key, out var value);
        return value != null ? _serializer.Deserialize<T>((string)value) : default(T);
    }
    
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var serializedValue = _serializer.Serialize(value);
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(15)
        };
        
        _cache.Set(key, serializedValue, cacheOptions);
    }
}
```

### Cache-Aside Pattern Implementation
```csharp
public class GameService : IGameService
{
    private readonly IGameRepository _repository;
    private readonly ICacheService _cache;
    private readonly ILogger<GameService> _logger;
    
    public async Task<GameDto> GetGameAsync(Guid gameId)
    {
        var cacheKey = $"game:{gameId}";
        
        // Try to get from cache first
        var cachedGame = await _cache.GetAsync<GameDto>(cacheKey);
        if (cachedGame != null)
        {
            _logger.LogInformation("Game {GameId} retrieved from cache", gameId);
            return cachedGame;
        }
        
        // If not in cache, get from database
        var game = await _repository.GetByIdAsync(gameId);
        if (game == null)
        {
            throw new NotFoundException($"Game with ID {gameId} not found");
        }
        
        var gameDto = MapToDto(game);
        
        // Store in cache for future requests
        await _cache.SetAsync(cacheKey, gameDto, TimeSpan.FromMinutes(30));
        
        _logger.LogInformation("Game {GameId} retrieved from database and cached", gameId);
        return gameDto;
    }
}
```

### Cache Invalidation Strategies
```csharp
// Event-Driven Cache Invalidation
public class GameUpdatedEventHandler : INotificationHandler<GameUpdatedEvent>
{
    private readonly ICacheService _cache;
    private readonly ILogger<GameUpdatedEventHandler> _logger;
    
    public async Task Handle(GameUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var cacheKey = $"game:{notification.GameId}";
        await _cache.RemoveAsync(cacheKey);
        
        // Also remove related cache entries
        await _cache.RemoveByPatternAsync($"games:*");
        await _cache.RemoveByPatternAsync($"odds:{notification.GameId}:*");
        
        _logger.LogInformation("Cache invalidated for game {GameId}", notification.GameId);
    }
}
```

---

## Message Queue Patterns

### Event-Driven Architecture with Kafka
```csharp
// Event Publisher
public class KafkaEventPublisher : IEventPublisher
{
    private readonly IProducer<string, string> _producer;
    private readonly IJsonSerializer _serializer;
    private readonly ILogger<KafkaEventPublisher> _logger;
    
    public async Task PublishAsync<T>(T @event, string topic) where T : class
    {
        try
        {
            var message = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = _serializer.Serialize(@event)
            };
            
            await _producer.ProduceAsync(topic, message);
            _logger.LogInformation("Event {EventType} published to topic {Topic}", typeof(T).Name, topic);
        }
        catch (ProduceException<string, string> ex)
        {
            _logger.LogError(ex, "Failed to publish event {EventType} to topic {Topic}", typeof(T).Name, topic);
            throw;
        }
    }
}

// Event Consumer
public class PaymentEventConsumer : IConsumer<string, string>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PaymentEventConsumer> _logger;
    
    public async Task ConsumeAsync(ConsumeResult<string, string> result)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var eventHandler = scope.ServiceProvider.GetRequiredService<IPaymentEventHandler>();
            
            var eventData = JsonSerializer.Deserialize<PaymentProcessedEvent>(result.Message.Value);
            await eventHandler.HandleAsync(eventData);
            
            _logger.LogInformation("Payment event processed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process payment event");
            // Implement dead letter queue logic here
        }
    }
}
```

### Saga Pattern for Distributed Transactions
```csharp
// Payment Saga Implementation
public class PaymentSaga : ISaga<PaymentSagaData>
{
    public Guid Id => Data.PaymentId;
    public PaymentSagaData Data { get; set; }
    
    public async Task HandleAsync(PaymentInitiatedEvent @event)
    {
        // Step 1: Reserve funds in wallet
        var reserveCommand = new ReserveFundsCommand
        {
            UserId = @event.UserId,
            Amount = @event.Amount,
            PaymentId = @event.PaymentId
        };
        
        await _commandBus.SendAsync(reserveCommand);
        
        // Step 2: Process payment with external provider
        var processCommand = new ProcessPaymentCommand
        {
            PaymentId = @event.PaymentId,
            Amount = @event.Amount,
            PaymentMethod = @event.PaymentMethod
        };
        
        await _commandBus.SendAsync(processCommand);
    }
    
    public async Task HandleAsync(PaymentProcessedEvent @event)
    {
        // Step 3: Update payment status
        var updateCommand = new UpdatePaymentStatusCommand
        {
            PaymentId = @event.PaymentId,
            Status = PaymentStatus.Completed
        };
        
        await _commandBus.SendAsync(updateCommand);
        
        // Mark saga as completed
        MarkAsComplete();
    }
    
    public async Task HandleAsync(PaymentFailedEvent @event)
    {
        // Compensating action: Release reserved funds
        var releaseCommand = new ReleaseFundsCommand
        {
            UserId = @event.UserId,
            Amount = @event.Amount,
            PaymentId = @event.PaymentId
        };
        
        await _commandBus.SendAsync(releaseCommand);
        
        // Mark saga as completed
        MarkAsComplete();
    }
}
```

---

## Monitoring & Observability

### Structured Logging with Serilog
```csharp
// Logging Configuration
public static class LoggingConfiguration
{
    public static void ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "Convex.Payment")
            .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
            .WriteTo.Console(new JsonFormatter())
            .WriteTo.File(new JsonFormatter(), "logs/payment-service-.json", 
                rollingInterval: RollingInterval.Day)
            .WriteTo.Seq("http://localhost:5341")
            .CreateLogger();
    }
}

// Correlation ID Middleware
public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string CorrelationIdHeaderName = "X-Correlation-ID";
    
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[CorrelationIdHeaderName].FirstOrDefault() 
                           ?? Guid.NewGuid().ToString();
        
        context.Items["CorrelationId"] = correlationId;
        context.Response.Headers[CorrelationIdHeaderName] = correlationId;
        
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await _next(context);
        }
    }
}
```

### Health Checks Implementation
```csharp
// Custom Health Checks
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IDbConnectionFactory _connectionFactory;
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.QueryAsync("SELECT 1");
            return HealthCheckResult.Healthy("Database is accessible");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database is not accessible", ex);
        }
    }
}

// Redis Health Check
public class RedisHealthCheck : IHealthCheck
{
    private readonly IDatabase _database;
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _database.PingAsync();
            return HealthCheckResult.Healthy("Redis is accessible");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Redis is not accessible", ex);
        }
    }
}
```

### Metrics Collection
```csharp
// Custom Metrics
public class PaymentMetrics
{
    private readonly IMetrics _metrics;
    
    public void RecordPaymentProcessed(decimal amount, string currency)
    {
        _metrics.Measure.Counter.Increment("payments.processed.total");
        _metrics.Measure.Histogram.Update("payments.amount", amount);
        _metrics.Measure.Gauge.SetValue("payments.currency", currency);
    }
    
    public void RecordPaymentFailed(string reason)
    {
        _metrics.Measure.Counter.Increment("payments.failed.total");
        _metrics.Measure.Counter.Increment($"payments.failed.{reason.ToLowerInvariant()}");
    }
    
    public void RecordProcessingTime(TimeSpan duration)
    {
        _metrics.Measure.Timer.Time("payments.processing.duration", duration);
    }
}
```

---

## Performance Optimization

### Connection Pooling Configuration
```csharp
// Database Connection Pooling
public static class DatabaseConfiguration
{
    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.CommandTimeout(30);
                sqlOptions.EnableRetryOnFailure(3);
                sqlOptions.EnableSensitiveDataLogging(false);
            });
            
            options.EnableSensitiveDataLogging(false);
            options.EnableServiceProviderCaching();
            options.EnableDetailedErrors(false);
        });
        
        // Configure connection pooling
        services.AddDbContextPool<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.CommandTimeout(30);
                sqlOptions.EnableRetryOnFailure(3);
            });
        }, poolSize: 100);
    }
}
```

### Query Optimization Patterns
```csharp
// Optimized Query with Projection
public async Task<IEnumerable<GameSummaryDto>> GetActiveGamesAsync(int page, int pageSize)
{
    return await _context.Games
        .Where(g => g.Status == GameStatus.Active)
        .Select(g => new GameSummaryDto
        {
            Id = g.Id,
            Name = g.Name,
            StartTime = g.StartTime,
            HomeTeam = g.HomeTeam.Name,
            AwayTeam = g.AwayTeam.Name
        })
        .OrderBy(g => g.StartTime)
        .Skip(page * pageSize)
        .Take(pageSize)
        .ToListAsync();
}

// Bulk Operations
public async Task BulkUpdateGameStatusAsync(IEnumerable<Guid> gameIds, GameStatus status)
{
    await _context.Games
        .Where(g => gameIds.Contains(g.Id))
        .ExecuteUpdateAsync(g => g.SetProperty(x => x.Status, status));
}
```

### Memory Management
```csharp
// Object Pooling for High-Frequency Objects
public class PaymentProcessorPool
{
    private readonly ObjectPool<PaymentProcessor> _pool;
    
    public PaymentProcessorPool()
    {
        var provider = new DefaultObjectPoolProvider();
        _pool = provider.Create(new PaymentProcessorPooledObjectPolicy());
    }
    
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        var processor = _pool.Get();
        try
        {
            return await processor.ProcessAsync(request);
        }
        finally
        {
            _pool.Return(processor);
        }
    }
}

// Streaming for Large Data Sets
public async IAsyncEnumerable<PaymentDto> StreamPaymentsAsync(Guid userId)
{
    await foreach (var payment in _context.Payments
        .Where(p => p.UserId == userId)
        .AsAsyncEnumerable())
    {
        yield return MapToDto(payment);
    }
}
```

---

## Testing Strategies

### Unit Testing with xUnit
```csharp
// Payment Service Unit Tests
public class PaymentServiceTests
{
    private readonly Mock<IPaymentRepository> _mockRepository;
    private readonly Mock<ICacheService> _mockCache;
    private readonly Mock<ILogger<PaymentService>> _mockLogger;
    private readonly PaymentService _service;
    
    public PaymentServiceTests()
    {
        _mockRepository = new Mock<IPaymentRepository>();
        _mockCache = new Mock<ICacheService>();
        _mockLogger = new Mock<ILogger<PaymentService>>();
        _service = new PaymentService(_mockRepository.Object, _mockCache.Object, _mockLogger.Object);
    }
    
    [Fact]
    public async Task ProcessPaymentAsync_ValidRequest_ReturnsSuccess()
    {
        // Arrange
        var request = new ProcessPaymentRequest
        {
            Amount = 100.00m,
            Currency = "ETB",
            PaymentMethod = "M-Pesa"
        };
        
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Payment>()))
            .ReturnsAsync(new Payment { Id = Guid.NewGuid() });
        
        // Act
        var result = await _service.ProcessPaymentAsync(request);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(PaymentStatus.Pending, result.Payment.Status);
        _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Payment>()), Times.Once);
    }
    
    [Fact]
    public async Task ProcessPaymentAsync_InsufficientFunds_ReturnsFailure()
    {
        // Arrange
        var request = new ProcessPaymentRequest
        {
            Amount = 1000.00m,
            Currency = "ETB",
            PaymentMethod = "M-Pesa"
        };
        
        _mockRepository.Setup(r => r.GetUserBalanceAsync(It.IsAny<Guid>()))
            .ReturnsAsync(500.00m);
        
        // Act
        var result = await _service.ProcessPaymentAsync(request);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Insufficient funds", result.ErrorMessage);
    }
}
```

### Integration Testing
```csharp
// Payment API Integration Tests
public class PaymentControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    
    public PaymentControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace with test database
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            });
        });
        
        _client = _factory.CreateClient();
    }
    
    [Fact]
    public async Task ProcessPayment_ValidRequest_ReturnsCreated()
    {
        // Arrange
        var request = new
        {
            Amount = 100.00m,
            Currency = "ETB",
            PaymentMethod = "M-Pesa"
        };
        
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        // Act
        var response = await _client.PostAsync("/api/v1/payments", content);
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PaymentResponse>(responseContent);
        Assert.True(result.Success);
    }
}
```

---

## Deployment Patterns

### Docker Configuration
```dockerfile
# Multi-stage Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Convex.Payment.API/Convex.Payment.API.csproj", "Convex.Payment.API/"]
COPY ["Convex.Payment.Application/Convex.Payment.Application.csproj", "Convex.Payment.Application/"]
COPY ["Convex.Payment.Domain/Convex.Payment.Domain.csproj", "Convex.Payment.Domain/"]
COPY ["Convex.Payment.Infrastructure/Convex.Payment.Infrastructure.csproj", "Convex.Payment.Infrastructure/"]
RUN dotnet restore "Convex.Payment.API/Convex.Payment.API.csproj"
COPY . .
WORKDIR "/src/Convex.Payment.API"
RUN dotnet build "Convex.Payment.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Convex.Payment.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Convex.Payment.API.dll"]
```

### Kubernetes Deployment
```yaml
# payment-service-deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: payment-service
  labels:
    app: payment-service
spec:
  replicas: 3
  selector:
    matchLabels:
      app: payment-service
  template:
    metadata:
      labels:
        app: payment-service
    spec:
      containers:
      - name: payment-service
        image: convex/payment-service:latest
        ports:
        - containerPort: 80
        env:
        - name: ASPNETCORE_ENVIRONMENT
          value: "Production"
        - name: ConnectionStrings__DefaultConnection
          valueFrom:
            secretKeyRef:
              name: payment-db-secret
              key: connection-string
        resources:
          requests:
            memory: "256Mi"
            cpu: "250m"
          limits:
            memory: "512Mi"
            cpu: "500m"
        livenessProbe:
          httpGet:
            path: /health
            port: 80
          initialDelaySeconds: 30
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: /health/ready
            port: 80
          initialDelaySeconds: 5
          periodSeconds: 5
```

---

## Configuration Management

### Environment-Specific Configuration
```csharp
// Configuration Builder
public static class ConfigurationBuilder
{
    public static IConfiguration BuildConfiguration(string environment)
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddAzureKeyVault() // For production secrets
            .Build();
    }
}

// Configuration Options Pattern
public class PaymentOptions
{
    public const string SectionName = "Payment";
    
    public decimal MaxPaymentAmount { get; set; } = 10000.00m;
    public decimal MinPaymentAmount { get; set; } = 5.00m;
    public int PaymentTimeoutMinutes { get; set; } = 30;
    public string[] AllowedCurrencies { get; set; } = { "ETB", "USD", "EUR" };
    public PaymentProviderOptions[] Providers { get; set; } = Array.Empty<PaymentProviderOptions>();
}

public class PaymentProviderOptions
{
    public string Name { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
}

// Options Registration
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPaymentOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PaymentOptions>(configuration.GetSection(PaymentOptions.SectionName));
        return services;
    }
}
```

### Secrets Management
```csharp
// Azure Key Vault Integration
public static class KeyVaultConfiguration
{
    public static IConfigurationBuilder AddAzureKeyVault(this IConfigurationBuilder builder)
    {
        var keyVaultUrl = Environment.GetEnvironmentVariable("KEY_VAULT_URL");
        if (!string.IsNullOrEmpty(keyVaultUrl))
        {
            var credential = new DefaultAzureCredential();
            builder.AddAzureKeyVault(keyVaultUrl, credential);
        }
        return builder;
    }
}

// Local Development Secrets
public static class UserSecretsConfiguration
{
    public static IConfigurationBuilder AddUserSecrets(this IConfigurationBuilder builder, string environment)
    {
        if (environment == "Development")
        {
            builder.AddUserSecrets<Program>();
        }
        return builder;
    }
}
```

This comprehensive guide now includes detailed implementations, code examples, and best practices for building enterprise-grade .NET 9 microservices. Each section provides practical, production-ready patterns that can be immediately implemented in your Convex shared libraries project.
