# Convex.Shared.Logging

Structured logging utilities for Convex microservices.

## Features

- **Structured Logging**: JSON-formatted logs with Serilog
- **Performance Metrics**: Built-in performance logging
- **Business Events**: Business event logging
- **API Request Logging**: HTTP request/response logging
- **Correlation IDs**: Request correlation tracking
- **Multiple Sinks**: Console and file logging
- **Enrichment**: Automatic property enrichment
- **High-Performance**: Optimized for billion-record scenarios
- **Object Pooling**: Memory-efficient object reuse
- **Parallel Processing**: Optimized batch logging

## Installation

```xml
<PackageReference Include="Convex.Shared.Logging" Version="1.0.0" />
```

## Quick Start

### 1. Register Services

```csharp
// In Program.cs
services.AddConvexLogging("UserService", "1.0.0");
```

### 2. Use in Your Service

```csharp
public class UserService
{
    private readonly IConvexLogger _logger;

    public UserService(IConvexLogger logger)
    {
        _logger = logger;
    }

    public async Task<User> GetUserAsync(int userId)
    {
        _logger.LogInformation("Getting user {UserId}", userId);
        
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            _logger.LogInformation("User {UserId} retrieved successfully", userId);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to get user {UserId}", ex, userId);
            throw;
        }
    }
}
```

## Advanced Usage

### Performance Logging

```csharp
public async Task<User> CreateUserAsync(CreateUserRequest request)
{
    var stopwatch = Stopwatch.StartNew();
    
    try
    {
        var user = await _userRepository.CreateAsync(request);
        _logger.LogPerformance("CreateUser", stopwatch.Elapsed, 
            new { UserId = user.Id, Email = user.Email });
        return user;
    }
    catch (Exception ex)
    {
        _logger.LogError("Failed to create user", ex);
        throw;
    }
}
```

### Business Event Logging

```csharp
public async Task<Bet> PlaceBetAsync(PlaceBetRequest request)
{
    var bet = await _betRepository.CreateAsync(request);
    
    _logger.LogBusinessEvent("BetPlaced", new
    {
        BetId = bet.Id,
        UserId = bet.UserId,
        Amount = bet.Amount,
        Odds = bet.Odds
    });
    
    return bet;
}
```

### API Request Logging

```csharp
public async Task<HttpResponseMessage> CallExternalApiAsync(string url)
{
    var stopwatch = Stopwatch.StartNew();
    
    try
    {
        var response = await _httpClient.GetAsync(url);
        _logger.LogApiRequest("GET", url, (int)response.StatusCode, stopwatch.Elapsed);
        return response;
    }
    catch (Exception ex)
    {
        _logger.LogError("API call failed", ex, new { Url = url });
        throw;
    }
}
```

### Correlation ID Logging

```csharp
public async Task ProcessRequestAsync(string correlationId, RequestData data)
{
    _logger.LogWithCorrelation(correlationId, "Processing request", 
        new { RequestId = data.Id, Type = data.Type });
    
    // Process request...
}
```

### High-Performance Batch Logging

```csharp
// Standard batch logging
var messages = new (string message, object[] properties)[]
{
    ("User created", new object[] { "UserId", 123 }),
    ("Order placed", new object[] { "OrderId", 456, "Amount", 99.99m })
};
_logger.LogBatch(messages);

// Ultra-high-performance batch logging with object pooling
_logger.LogBatchOptimized(messages);
```

## Configuration

### Basic Configuration

```csharp
services.AddConvexLogging("UserService", "1.0.0");
```

### Custom Configuration

```csharp
services.AddConvexLogging(config =>
{
    config.MinimumLevel.Information()
        .Enrich.WithProperty("ServiceName", "UserService")
        .Enrich.WithProperty("Version", "1.0.0")
        .Enrich.WithProperty("Environment", "Production")
        .WriteTo.Console()
        .WriteTo.File("logs/user-service-.txt", rollingInterval: RollingInterval.Day)
        .WriteTo.Seq("http://seq-server:5341");
});
```

### appsettings.json Configuration

```json
{
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {ServiceName} {Message:lj} {Properties:j}{NewLine}{Exception}"
        }
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/convex-.txt",
          "rollingInterval": "Day"
        }
      }
    ],
    "Enrich": ["FromLogContext", "WithMachineName", "WithProcessId"]
  }
}
```

## Log Levels

- **Trace**: Detailed diagnostic information
- **Debug**: Diagnostic information for debugging
- **Information**: General information about application flow
- **Warning**: Warning messages for potential issues
- **Error**: Error messages for exceptions and failures
- **Fatal**: Critical errors that may cause application failure

## Log Formats

### Console Output
```
[14:30:15 INF] UserService Getting user 123
[14:30:15 INF] UserService User 123 retrieved successfully
```

### File Output (JSON)
```json
{
  "Timestamp": "2024-01-15T14:30:15.123Z",
  "Level": "Information",
  "MessageTemplate": "Getting user {UserId}",
  "Properties": {
    "ServiceName": "UserService",
    "Version": "1.0.0",
    "MachineName": "SERVER-01",
    "ProcessId": 1234,
    "UserId": 123
  }
}
```

## Performance Optimizations

### Object Pooling
- **Memory Efficient**: Reuses object arrays to reduce GC pressure
- **High Volume**: Optimized for billion-record scenarios
- **Automatic Management**: Buffers are automatically returned to pool

### Parallel Processing
- **Batch Logging**: Parallel processing for large batches (>100 messages)
- **Ultra-High Performance**: Optimized batch logging for very large batches (>1000 messages)
- **Smart Thresholds**: Uses sequential processing for small batches to avoid overhead

### Memory Management
- **Pre-allocated Arrays**: Avoids dynamic List growth
- **Efficient Copying**: Uses Array.Copy for better performance
- **Reduced Allocations**: Eliminates anonymous object creation

## Best Practices

1. **Use Structured Logging**: Always use structured logging with properties
2. **Log Performance**: Log performance metrics for critical operations
3. **Log Business Events**: Log important business events
4. **Use Correlation IDs**: Track requests across services
5. **Log Errors Properly**: Always include exception details
6. **Use Appropriate Levels**: Use the correct log level for each message
7. **Use Batch Logging**: For high-volume scenarios, use LogBatch or LogBatchOptimized
8. **Return Buffers**: When using object pooling, return buffers to pool

## License

This project is licensed under the MIT License.
