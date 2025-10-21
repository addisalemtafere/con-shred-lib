# Convex.Shared.Http

Professional HTTP client library for Convex microservices.

## Features

- **Clean API**: Simple and intuitive interface
- **JSON Serialization**: Automatic JSON serialization/deserialization
- **Multiple Authentication Types**: API Key, Bearer Token, Basic Auth, OAuth2, Custom
- **Automatic Token Management**: OAuth2 tokens cached and refreshed automatically
- **Multiple HTTP Clients**: Support for different services with different authentication
- **Resilience Patterns**: Built-in retry, circuit breaker, and timeout policies
- **Polly Integration**: Advanced resilience policies with exponential backoff and jitter
- **Flexible Configuration**: String-based configuration from JSON, environment variables, or code
- **Production Ready**: Comprehensive error handling and logging
- **Debugging & Monitoring**: Request tracking, performance metrics, health checks
- **Dependency Injection**: Easy integration with .NET DI container

## Installation

```xml
<PackageReference Include="Convex.Shared.Http" Version="1.0.0" />
```

## Quick Start

### 1. Register the Service

```csharp
// In Program.cs
services.AddConvexHttpClient(options =>
{
    options.BaseAddress = new Uri("https://api.example.com");
    options.Authentication.Type = AuthenticationTypes.ApiKey;
    options.Authentication.ApiKey = "your-api-key";
    options.Timeout = TimeSpan.FromSeconds(30);
});
```

### 2. Use in Your Service

```csharp
public class UserService
{
    private readonly IConvexHttpClient _httpClient;

    public UserService(IConvexHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<User> GetUserAsync(int userId)
    {
        return await _httpClient.GetAsync<User>($"/users/{userId}");
    }

    public async Task<User> CreateUserAsync(CreateUserRequest request)
    {
        return await _httpClient.PostAsync<CreateUserRequest, User>("/users", request);
    }
}
```

### 3. Multiple Services with Different Authentication

```csharp
// User Service with API Key
services.AddConvexHttpClient("UserService", options =>
{
    options.BaseAddress = new Uri("https://user-service.example.com");
    options.Authentication.Type = AuthenticationTypes.ApiKey;
    options.Authentication.ApiKey = "user-service-key";
});

// Payment Service with OAuth2
services.AddConvexHttpClient("PaymentService", options =>
{
    options.BaseAddress = new Uri("https://payment-service.example.com");
    options.Authentication.Type = AuthenticationTypes.OAuth2ClientCredentials;
    options.Authentication.EnableTokenManagement = true;
    options.Authentication.TokenEndpoint = "https://auth.example.com/oauth/token";
    options.Authentication.ClientId = "payment-client-id";
    options.Authentication.ClientSecret = "payment-client-secret";
});
```

## Configuration

### Programmatic Configuration

```csharp
services.AddConvexHttpClient(options =>
{
    options.BaseAddress = new Uri("https://api.example.com");
    options.Timeout = TimeSpan.FromSeconds(30);
    options.Authentication.Type = AuthenticationTypes.ApiKey;
    options.Authentication.ApiKey = "your-api-key";
    options.EnableLogging = true;
    options.DefaultHeaders.Add("X-Custom-Header", "value");
});
```

### Configuration from appsettings.json

```json
{
  "ConvexHttpClient": {
    "BaseAddress": "https://api.example.com",
    "Timeout": "00:00:30",
    "EnableLogging": true,
    "Authentication": {
      "Type": "ApiKey",
      "ApiKey": "your-api-key",
      "ApiKeyHeaderName": "X-API-Key"
    },
    "DefaultHeaders": {
      "X-Custom-Header": "value"
    }
  }
}
```

```csharp
services.AddConvexHttpClient(configuration);
```

### Multiple Clients from Configuration

```json
{
  "HttpClients": {
    "UserService": {
      "BaseAddress": "https://user-service.example.com",
      "Authentication": {
        "Type": "ApiKey",
        "ApiKey": "user-service-key"
      }
    },
    "PaymentService": {
      "BaseAddress": "https://payment-service.example.com",
      "Authentication": {
        "Type": "OAuth2ClientCredentials",
        "EnableTokenManagement": true,
        "TokenEndpoint": "https://auth.example.com/oauth/token",
        "ClientId": "payment-client-id",
        "ClientSecret": "payment-client-secret"
      }
    }
  }
}
```

```csharp
var httpClients = configuration.GetSection("HttpClients")
    .Get<Dictionary<string, ConvexHttpClientOptions>>();
services.AddConvexHttpClients(httpClients);
```

## Production Debugging & Monitoring

The HTTP client includes comprehensive debugging and monitoring capabilities for production environments:

### Debugging Configuration

```csharp
services.AddConvexHttpClient(options =>
{
    // Basic request/response logging
    options.EnableLogging = true;
    
    // Detailed debug logging (development only)
    options.EnableDebugLogging = true;
    
    // Content logging (be careful with sensitive data)
    options.EnableContentLogging = false;
    options.MaxContentLogLength = 1024;
    
    // Performance metrics
    options.EnableMetrics = true;
});
```

### Request Tracking

Every request gets a unique ID for easy debugging:

```csharp
// Logs include request IDs for tracing
[abc12345] Starting GET request to /api/users/123
[abc12345] GET /api/users/123 completed in 245ms with status 200
[abc12345] Response content length: 1024
```

### Performance Metrics

Built-in metrics collection:

```csharp
// Get current metrics
var diagnostics = serviceProvider.GetRequiredService<IHttpClientDiagnostics>();
var metrics = diagnostics.GetMetrics();

Console.WriteLine($"Total Requests: {metrics.TotalRequests}");
Console.WriteLine($"Success Rate: {metrics.SuccessRate:F2}%");
Console.WriteLine($"Average Response Time: {metrics.AverageResponseTime:F2}ms");
Console.WriteLine($"Requests Per Minute: {metrics.RequestsPerMinute:F2}");
```

### Health Checks

Automatic health monitoring:

```csharp
// Health check endpoint: /health
// Returns detailed metrics and health status
{
  "status": "Healthy",
  "description": "Success rate: 98.5%, Average response time: 245ms",
  "data": {
    "TotalRequests": 1250,
    "SuccessfulRequests": 1232,
    "FailedRequests": 18,
    "SuccessRate": "98.56%",
    "AverageResponseTime": "245.32ms",
    "RequestsPerMinute": "45.2",
    "LastRequestTime": "2024-01-15T10:30:45.123Z"
  }
}
```

### Logging Examples

**Success Logs:**
```
[INFO] [abc12345] GET /api/users/123 completed in 245ms with status 200
[DEBUG] [abc12345] Response content length: 1024
```

**Error Logs:**
```
[ERROR] [abc12345] GET /api/users/123 failed after 5000ms
System.HttpRequestException: Request failed with status 500
```

**Debug Logs:**
```
[DEBUG] [abc12345] Starting GET request to /api/users/123
[DEBUG] [abc12345] POST /api/users content: {"name":"John","email":"john@example.com"}
```

### Production Configuration

```json
{
  "ConvexHttpClient": {
    "BaseAddress": "https://api.example.com",
    "EnableLogging": true,
    "EnableDebugLogging": false,
    "EnableContentLogging": false,
    "EnableMetrics": true,
    "MaxContentLogLength": 1024,
    "Authentication": {
      "Type": "ApiKey",
      "ApiKey": "your-api-key"
    }
  }
}
```

## Resilience Configuration

The HTTP client includes built-in resilience patterns using Polly:

### Retry Policy

```csharp
services.AddConvexHttpClient(options =>
{
    options.RetryPolicy.MaxRetryAttempts = 3;
    options.RetryPolicy.BaseDelay = TimeSpan.FromSeconds(1);
    options.RetryPolicy.MaxDelay = TimeSpan.FromSeconds(30);
    options.RetryPolicy.UseExponentialBackoff = true;
    options.RetryPolicy.JitterFactor = 0.1;
    options.RetryPolicy.RetryableStatusCodes = new[] { 408, 429, 500, 502, 503, 504 };
});
```

### Circuit Breaker

```csharp
services.AddConvexHttpClient(options =>
{
    options.CircuitBreaker.Enabled = true;
    options.CircuitBreaker.FailureThreshold = 5;
    options.CircuitBreaker.DurationOfBreak = TimeSpan.FromSeconds(30);
    options.CircuitBreaker.MinimumThroughput = 10;
});
```

### Timeout Policy

```csharp
services.AddConvexHttpClient(options =>
{
    options.TimeoutPolicy.Enabled = true;
    options.TimeoutPolicy.Timeout = TimeSpan.FromSeconds(30);
});
```

### Complete Resilience Configuration

```json
{
  "ConvexHttpClient": {
    "BaseAddress": "https://api.example.com",
    "Timeout": "00:00:30",
    "EnableLogging": true,
    "Authentication": {
      "Type": "OAuth2ClientCredentials",
      "EnableTokenManagement": true,
      "TokenEndpoint": "https://auth.example.com/oauth/token",
      "ClientId": "your-client-id",
      "ClientSecret": "your-client-secret",
      "Scope": "api.read api.write",
      "TokenCacheDuration": "00:50:00",
      "TokenRefreshBuffer": "00:05:00"
    },
    "RetryPolicy": {
      "MaxRetryAttempts": 3,
      "BaseDelay": "00:00:01",
      "MaxDelay": "00:00:30",
      "UseExponentialBackoff": true,
      "JitterFactor": 0.1,
      "RetryableStatusCodes": [408, 429, 500, 502, 503, 504]
    },
    "CircuitBreaker": {
      "Enabled": true,
      "FailureThreshold": 5,
      "DurationOfBreak": "00:00:30",
      "MinimumThroughput": 10
    },
    "TimeoutPolicy": {
      "Enabled": true,
      "Timeout": "00:00:30"
    }
  }
}
```

## Authentication Types

The HTTP client supports multiple authentication types with string-based configuration:

### Available Authentication Types

- **`"None"`** - No authentication
- **`"ApiKey"`** - API Key in custom header
- **`"BearerToken"`** - Bearer token authentication
- **`"Basic"`** - Basic username/password authentication
- **`"OAuth2ClientCredentials"`** - OAuth2 with automatic token management
- **`"Custom"`** - Custom header authentication

### 1. API Key Authentication

```csharp
services.AddConvexHttpClient(options =>
{
    options.BaseAddress = new Uri("https://api.example.com");
    options.Authentication.Type = AuthenticationTypes.ApiKey;
    options.Authentication.ApiKey = "your-api-key";
    options.Authentication.ApiKeyHeaderName = "X-API-Key"; // Optional, defaults to X-API-Key
});
```

### 2. Bearer Token Authentication

```csharp
services.AddConvexHttpClient(options =>
{
    options.BaseAddress = new Uri("https://api.example.com");
    options.Authentication.Type = AuthenticationTypes.BearerToken;
    options.Authentication.BearerToken = "your-bearer-token";
});
```

### 3. Basic Authentication

```csharp
services.AddConvexHttpClient(options =>
{
    options.BaseAddress = new Uri("https://api.example.com");
    options.Authentication.Type = AuthenticationTypes.Basic;
    options.Authentication.Username = "username";
    options.Authentication.Password = "password";
});
```

### 4. OAuth2 Client Credentials (with automatic token management)

```csharp
services.AddConvexHttpClient(options =>
{
    options.BaseAddress = new Uri("https://api.example.com");
    options.Authentication.Type = AuthenticationTypes.OAuth2ClientCredentials;
    options.Authentication.EnableTokenManagement = true;
    options.Authentication.TokenEndpoint = "https://auth.example.com/oauth/token";
    options.Authentication.ClientId = "your-client-id";
    options.Authentication.ClientSecret = "your-client-secret";
    options.Authentication.Scope = "api.read api.write";
    options.Authentication.TokenCacheDuration = TimeSpan.FromMinutes(50);
    options.Authentication.TokenRefreshBuffer = TimeSpan.FromMinutes(5);
});
```

### 5. Custom Authentication

```csharp
services.AddConvexHttpClient(options =>
{
    options.BaseAddress = new Uri("https://api.example.com");
    options.Authentication.Type = AuthenticationTypes.Custom;
    options.Authentication.CustomHeaderName = "X-Custom-Auth";
    options.Authentication.CustomHeaderValue = "your-custom-value";
});
```

## Multiple HTTP Clients

For different applications/services with different authentication types:

### 1. Individual Named Clients

```csharp
// User Service with API Key
services.AddConvexHttpClient("UserService", options =>
{
    options.BaseAddress = new Uri("https://user-service.example.com");
    options.Authentication.Type = AuthenticationTypes.ApiKey;
    options.Authentication.ApiKey = "user-service-key";
});

// Payment Service with OAuth2
services.AddConvexHttpClient("PaymentService", options =>
{
    options.BaseAddress = new Uri("https://payment-service.example.com");
    options.Authentication.Type = AuthenticationTypes.OAuth2ClientCredentials;
    options.Authentication.EnableTokenManagement = true;
    options.Authentication.TokenEndpoint = "https://auth.example.com/oauth/token";
    options.Authentication.ClientId = "payment-client-id";
    options.Authentication.ClientSecret = "payment-client-secret";
});

// Notification Service with Basic Auth
services.AddConvexHttpClient("NotificationService", options =>
{
    options.BaseAddress = new Uri("https://notification-service.example.com");
    options.Authentication.Type = AuthenticationTypes.Basic;
    options.Authentication.Username = "notification-user";
    options.Authentication.Password = "notification-password";
});
```

### 2. Batch Configuration

```csharp
var httpClients = new Dictionary<string, ConvexHttpClientOptions>
{
    ["UserService"] = new ConvexHttpClientOptions
    {
        BaseAddress = new Uri("https://user-service.example.com"),
        Authentication = new AuthenticationOptions
        {
            Type = AuthenticationTypes.ApiKey,
            ApiKey = "user-service-key"
        }
    },
    ["PaymentService"] = new ConvexHttpClientOptions
    {
        BaseAddress = new Uri("https://payment-service.example.com"),
        Authentication = new AuthenticationOptions
        {
            Type = AuthenticationTypes.OAuth2ClientCredentials,
            EnableTokenManagement = true,
            TokenEndpoint = "https://auth.example.com/oauth/token",
            ClientId = "payment-client-id",
            ClientSecret = "payment-client-secret"
        }
    },
    ["NotificationService"] = new ConvexHttpClientOptions
    {
        BaseAddress = new Uri("https://notification-service.example.com"),
        Authentication = new AuthenticationOptions
        {
            Type = AuthenticationTypes.Basic,
            Username = "notification-user",
            Password = "notification-password"
        }
    }
};

services.AddConvexHttpClients(httpClients);
```

### 3. Using Named Clients in Services

```csharp
public class UserService
{
    private readonly HttpClient _userServiceClient;
    private readonly HttpClient _paymentServiceClient;
    private readonly HttpClient _notificationServiceClient;

    public UserService(
        IHttpClientFactory httpClientFactory,
        ILogger<UserService> logger)
    {
        _userServiceClient = httpClientFactory.CreateClient("UserService");
        _paymentServiceClient = httpClientFactory.CreateClient("PaymentService");
        _notificationServiceClient = httpClientFactory.CreateClient("NotificationService");
    }

    public async Task<User> GetUserAsync(int userId)
    {
        // Uses API Key authentication automatically
        var response = await _userServiceClient.GetAsync($"/users/{userId}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<User>(content);
    }

    public async Task<Payment> ProcessPaymentAsync(PaymentRequest request)
    {
        // Uses OAuth2 with automatic token management
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _paymentServiceClient.PostAsync("/payments", content);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Payment>(responseContent);
    }

    public async Task SendNotificationAsync(NotificationRequest request)
    {
        // Uses Basic authentication automatically
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _notificationServiceClient.PostAsync("/notifications", content);
        response.EnsureSuccessStatusCode();
    }
}
```

### 4. Configuration from appsettings.json

```json
{
  "HttpClients": {
    "UserService": {
      "BaseAddress": "https://user-service.example.com",
      "Authentication": {
        "Type": "ApiKey",
        "ApiKey": "user-service-key"
      }
    },
    "PaymentService": {
      "BaseAddress": "https://payment-service.example.com",
      "Authentication": {
        "Type": "OAuth2ClientCredentials",
        "EnableTokenManagement": true,
        "TokenEndpoint": "https://auth.example.com/oauth/token",
        "ClientId": "payment-client-id",
        "ClientSecret": "payment-client-secret"
      }
    },
    "NotificationService": {
      "BaseAddress": "https://notification-service.example.com",
      "Authentication": {
        "Type": "Basic",
        "Username": "notification-user",
        "Password": "notification-password"
      }
    }
  }
}
```

```csharp
// Register from configuration
var httpClients = configuration.GetSection("HttpClients")
    .Get<Dictionary<string, ConvexHttpClientOptions>>();
services.AddConvexHttpClients(httpClients);
```

## Service-to-Service Communication

### Simple Setup

```csharp
services.AddConvexHttpClient(
    baseAddress: "https://user-service.example.com",
    apiKey: "user-service-key",
    timeout: TimeSpan.FromSeconds(15));
```

### Multiple Services

```csharp
// User Service
services.AddConvexServiceClient(
    serviceName: "UserService",
    baseAddress: "https://user-service.example.com",
    apiKey: "user-service-key");

// Order Service
services.AddConvexServiceClient(
    serviceName: "OrderService", 
    baseAddress: "https://order-service.example.com",
    apiKey: "order-service-key");
```

## Error Handling

The client follows library best practices - exceptions bubble up naturally for consumer applications to handle:

```csharp
try
{
    var user = await _httpClient.GetAsync<User>("/users/123");
    return user;
}
catch (ArgumentException ex)
{
    // Handle invalid parameters
    _logger.LogError(ex, "Invalid parameter: {Message}", ex.Message);
    throw;
}
catch (HttpRequestException ex)
{
    // Handle HTTP errors (4xx, 5xx status codes)
    _logger.LogError(ex, "HTTP request failed: {StatusCode}", ex.Data["StatusCode"]);
    throw;
}
catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
{
    // Handle timeout
    _logger.LogError(ex, "Request timed out");
    throw new TimeoutException("Request timed out", ex);
}
catch (JsonException ex)
{
    // Handle JSON deserialization errors
    _logger.LogError(ex, "Failed to deserialize response");
    throw;
}
```

## Logging

The library follows best practices for NuGet packages - logging is handled by the consuming application:

```csharp
// The library doesn't log internally - consumers handle logging
services.AddConvexHttpClient(options =>
{
    // Library focuses on HTTP operations, not logging
    options.BaseAddress = new Uri("https://api.example.com");
    options.ApiKey = "your-api-key";
});

// Consumer application handles logging
public class UserService
{
    private readonly IConvexHttpClient _httpClient;
    private readonly ILogger<UserService> _logger;

    public async Task<User?> GetUserAsync(int userId)
    {
        try
        {
            _logger.LogInformation("Fetching user {UserId}", userId);
            var user = await _httpClient.GetAsync<User>($"/users/{userId}");
            _logger.LogInformation("Successfully fetched user {UserId}", userId);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch user {UserId}", userId);
            throw;
        }
    }
}
```

## Best Practices

### For Library Consumers:

1. **Use Dependency Injection**: Always inject `IConvexHttpClient` in your services
2. **Handle Exceptions**: Implement proper error handling in your services
3. **Log in Your Application**: Handle logging at the application level, not in the library
4. **Configure Resilience**: Use the built-in retry, circuit breaker, and timeout policies
5. **Validate Inputs**: The library validates parameters and throws appropriate exceptions

### Library Design Principles:

- **✅ Input Validation**: Library validates required parameters
- **✅ Exception Propagation**: Exceptions bubble up naturally
- **✅ Performance Focused**: Minimal overhead, fast execution
- **✅ Consumer Responsibility**: Applications handle logging and business logic
- **✅ Resilience Built-in**: Polly policies handle retry/circuit breaker automatically

### Example Service Implementation:

```csharp
public class UserService
{
    private readonly IConvexHttpClient _httpClient;
    private readonly ILogger<UserService> _logger;

    public UserService(IConvexHttpClient httpClient, ILogger<UserService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<User?> GetUserAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be positive", nameof(userId));

        try
        {
            _logger.LogInformation("Fetching user {UserId}", userId);
            var user = await _httpClient.GetAsync<User>($"/users/{userId}");
            
            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found", userId);
                return null;
            }

            _logger.LogInformation("Successfully fetched user {UserId}", userId);
            return user;
        }
        catch (ArgumentException)
        {
            // Re-throw validation errors
            throw;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error fetching user {UserId}", userId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching user {UserId}", userId);
            throw;
        }
    }
}
```

## Summary

The `Convex.Shared.Http` library provides a comprehensive HTTP client solution for .NET microservices with:

### ✅ **Key Features**
- **Multiple Authentication Types**: API Key, Bearer Token, Basic Auth, OAuth2, Custom
- **Automatic Token Management**: OAuth2 tokens cached and refreshed automatically
- **Multiple HTTP Clients**: Different services with different authentication
- **Resilience Patterns**: Retry, circuit breaker, and timeout policies
- **Flexible Configuration**: String-based configuration from JSON or code
- **Production Ready**: Comprehensive error handling and logging
- **Debugging & Monitoring**: Request tracking, performance metrics, health checks
- **Easy Debugging**: Unique request IDs, structured logging, content logging

### 🚀 **Quick Setup**
```csharp
// Single client
services.AddConvexHttpClient(options => {
    options.Authentication.Type = AuthenticationTypes.ApiKey;
    options.Authentication.ApiKey = "your-key";
});

// Multiple clients
services.AddConvexHttpClient("UserService", options => {
    options.Authentication.Type = AuthenticationTypes.ApiKey;
});
services.AddConvexHttpClient("PaymentService", options => {
    options.Authentication.Type = AuthenticationTypes.OAuth2ClientCredentials;
});
```

### 📋 **Configuration Options**
- **JSON Configuration**: Easy setup from appsettings.json
- **Environment Variables**: Support for environment-based config
- **Code Configuration**: Programmatic setup with IntelliSense
- **String Constants**: No hardcoded enums, flexible configuration

### 🔍 **Debugging & Monitoring**
- **Request Tracking**: Unique IDs for tracing requests
- **Performance Metrics**: Success rates, response times, throughput
- **Health Checks**: Automatic health monitoring endpoints
- **Structured Logging**: Consistent, searchable log format
- **Content Logging**: Optional request/response content logging

### 🎯 **Perfect For**
- Microservices communication
- API integrations with different authentication
- Production applications requiring resilience
- Multi-tenant applications with different auth types
- Applications requiring comprehensive debugging and monitoring

## License

This project is licensed under the MIT License.
