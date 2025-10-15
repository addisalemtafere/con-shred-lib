# Convex.Shared.Http

Professional HTTP client library for Convex microservices.

## Features

- **Clean API**: Simple and intuitive interface
- **JSON Serialization**: Automatic JSON serialization/deserialization
- **Authentication**: API Key and Bearer Token support
- **Logging**: Built-in request/response logging
- **Configuration**: Flexible configuration options
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
    options.ApiKey = "your-api-key";
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

## Configuration

### Programmatic Configuration

```csharp
services.AddConvexHttpClient(options =>
{
    options.BaseAddress = new Uri("https://api.example.com");
    options.Timeout = TimeSpan.FromSeconds(30);
    options.ApiKey = "your-api-key";
    options.BearerToken = "your-bearer-token";
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
    "ApiKey": "your-api-key",
    "BearerToken": "your-bearer-token",
    "EnableLogging": true,
    "DefaultHeaders": {
      "X-Custom-Header": "value"
    }
  }
}
```

```csharp
services.AddConvexHttpClient(configuration);
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

The client automatically handles common HTTP errors:

```csharp
try
{
    var user = await _httpClient.GetAsync<User>("/users/123");
    return user;
}
catch (HttpRequestException ex)
{
    // Handle HTTP errors
    _logger.LogError(ex, "HTTP request failed");
    throw;
}
catch (TaskCanceledException ex)
{
    // Handle timeout
    _logger.LogError(ex, "Request timed out");
    throw;
}
```

## Logging

Enable logging to see request details:

```csharp
services.AddConvexHttpClient(options =>
{
    options.EnableLogging = true;
});
```

## Best Practices

1. **Use Dependency Injection**: Always inject `IConvexHttpClient` in your services
2. **Configure Timeouts**: Set appropriate timeouts based on your service requirements
3. **Handle Errors**: Implement proper error handling in your services
4. **Use Logging**: Enable logging in development, disable in production for performance
5. **Configure Authentication**: Use API keys or Bearer tokens for service authentication

## License

This project is licensed under the MIT License.
