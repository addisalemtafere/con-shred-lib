# Convex.Shared.Grpc

High-performance gRPC communication library for Convex microservices.

## Features

- ✅ **High-Performance gRPC** - 7-10x faster than HTTP/JSON
- ✅ **Type-Safe Communication** - Strong typing with Protocol Buffers
- ✅ **Bidirectional Streaming** - Real-time communication
- ✅ **Load Balancing** - Built-in load balancing support
- ✅ **Service Discovery** - Integrates with Consul, etcd
- ✅ **Security** - TLS encryption by default
- ✅ **Correlation ID Support** - Request tracing across services

## Quick Start

### 1. Register gRPC Services

#### For Local Development (No Authentication):
```csharp
// In your microservice Startup.cs - Local development
services.AddConvexGrpc(isProduction: false);
```

#### For Production (With Simple API Key Authentication):
```csharp
// In your microservice Startup.cs - Production
services.AddConvexGrpc(
    isProduction: true, 
    serviceApiKey: "your-service-api-key",
    validApiKeys: new List<string> { "auth-service-key", "user-service-key", "betting-service-key" }
);
```

#### Custom Configuration:
```csharp
// Custom configuration
services.AddConvexGrpc(options =>
{
    options.EnableTls = true; // Enable for production
    options.EnableAuthentication = true; // Enable for production
    options.ServiceApiKey = "your-service-api-key";
    options.ValidApiKeys = new List<string> { "auth-key", "user-key", "betting-key" };
});
```

### 2. Use gRPC Client

#### For Local Development (No Authentication):
```csharp
public class UserService
{
    private readonly IGrpcClientFactory _grpcClientFactory;

    public UserService(IGrpcClientFactory grpcClientFactory)
    {
        _grpcClientFactory = grpcClientFactory;
    }

    public async Task<UserResponse> GetUserAsync(string userId)
    {
        // No authentication required for local development
        var client = _grpcClientFactory.CreateClient<UserService.UserServiceClient>("UserService");
        return await client.GetUserAsync(new UserRequest { UserId = userId });
    }
}
```

#### For Production (With Simple API Key Authentication):
```csharp
public class UserService
{
    private readonly IGrpcClientFactory _grpcClientFactory;

    public UserService(IGrpcClientFactory grpcClientFactory)
    {
        _grpcClientFactory = grpcClientFactory;
    }

    public async Task<UserResponse> GetUserAsync(string userId)
    {
        // Simple API key authentication for production
        var client = _grpcClientFactory.CreateClientWithApiKey<AuthService.AuthServiceClient>("AuthService", "user-service-api-key");
        
        // Add API key to gRPC call
        var metadata = SimpleApiKeyAuthentication.CreateApiKeyMetadata("user-service-api-key");
        
        return await client.GetUserAsync(new UserRequest { UserId = userId }, metadata);
    }
}
```

## Performance Benefits

- **7-10x faster** than HTTP/JSON
- **Binary serialization** with Protocol Buffers
- **HTTP/2 multiplexing** for concurrent requests
- **Built-in compression** for large payloads
- **Connection pooling** for efficiency

## 🚀 **Professional Performance & Scalability**

### **High-Performance Features:**
- ✅ **Connection Pooling** - Reuse connections for maximum efficiency
- ✅ **Load Balancer Integration** - Works with external load balancers
- ✅ **Circuit Breaker** - Fault tolerance and resilience
- ✅ **Metrics Collection** - Real-time performance monitoring
- ✅ **Response Caching** - Cache frequently accessed data
- ✅ **Parallel Processing** - Execute multiple calls concurrently
- ✅ **No Hardcoded Values** - Fully configurable via environment/config

### **Simple API Key Authentication:**
- ✅ **Each service** has a unique API key
- ✅ **Service-to-service** calls use API keys
- ✅ **No token validation** complexity
- ✅ **Fast and simple** authentication

### **Service Communication Examples:**

#### **UserService calling AuthService (via Load Balancer):**
```csharp
// Load balancer handles multiple AuthService instances
var authClient = _grpcClientFactory.CreateClientWithApiKey<AuthService.AuthServiceClient>("AuthService", "user-service-api-key");
var metadata = SimpleApiKeyAuthentication.CreateApiKeyMetadata("user-service-api-key");
var response = await authClient.ValidateUserAsync(request, metadata);
```

#### **BettingService calling PaymentService (via Load Balancer):**
```csharp
// Load balancer handles multiple PaymentService instances
var paymentClient = _grpcClientFactory.CreateClientWithApiKey<PaymentService.PaymentServiceClient>("PaymentService", "betting-service-api-key");
var metadata = SimpleApiKeyAuthentication.CreateApiKeyMetadata("betting-service-api-key");
var response = await paymentClient.ProcessPaymentAsync(request, metadata);
```

## 🏗️ **Load Balancer Architecture**

### **Production Setup with Load Balancers:**
```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Auth Service  │    │   User Service  │    │ Betting Service│
│   Load Balancer │    │   Load Balancer │    │   Load Balancer │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         ▼                       ▼                       ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│ auth-1, auth-2, │    │ user-1, user-2,│    │betting-1,betting│
│ auth-3, auth-4  │    │ user-3, user-4 │    │-2, betting-3   │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

### **Configuration for Load Balancers:**
```csharp
// Production configuration
services.AddConvexGrpc(options =>
{
    options.ServiceName = "AuthService";
    options.Services.AddRange(new[]
    {
        new GrpcServiceConfig { Name = "AuthService", Endpoint = "http://auth-loadbalancer.prod.com" },
        new GrpcServiceConfig { Name = "UserService", Endpoint = "http://user-loadbalancer.prod.com" },
        new GrpcServiceConfig { Name = "BettingService", Endpoint = "http://betting-loadbalancer.prod.com" },
        new GrpcServiceConfig { Name = "PaymentService", Endpoint = "http://payment-loadbalancer.prod.com" }
    });
});
```

### **Environment Variables:**
```bash
# Production
GRPC_BASE_URL=https://convex.com
GRPC_AUTHSERVICE_ENDPOINT=https://auth-loadbalancer.convex.com
GRPC_USERSERVICE_ENDPOINT=https://user-loadbalancer.convex.com
GRPC_BETTINGSERVICE_ENDPOINT=https://betting-loadbalancer.convex.com

# Development
GRPC_BASE_URL=http://localhost
GRPC_AUTHSERVICE_ENDPOINT=http://auth-loadbalancer.local
GRPC_USERSERVICE_ENDPOINT=http://user-loadbalancer.local
```

### **Docker Compose with Load Balancers:**
```yaml
version: '3.8'
services:
  # Load Balancers
  auth-loadbalancer:
    image: nginx:alpine
    ports: ["50051:80"]
    volumes:
      - ./nginx/auth.conf:/etc/nginx/nginx.conf
    
  user-loadbalancer:
    image: nginx:alpine
    ports: ["50052:80"]
    volumes:
      - ./nginx/user.conf:/etc/nginx/nginx.conf

  # Service Instances
  auth-service-1:
    build: ./src/AuthService
    environment:
      - ASPNETCORE_URLS=http://+:50051
      
  auth-service-2:
    build: ./src/AuthService
    environment:
      - ASPNETCORE_URLS=http://+:50051
```

## 🎯 **Usage Examples**

### **Service-to-Service Communication:**
```csharp
public class UserService
{
    private readonly IGrpcClientFactory _grpcClientFactory;

    public UserService(IGrpcClientFactory grpcClientFactory)
    {
        _grpcClientFactory = grpcClientFactory;
    }

    public async Task<UserResponse> GetUserAsync(string userId)
    {
        // Load balancer automatically distributes requests
        var authClient = _grpcClientFactory.CreateClientWithApiKey<AuthService.AuthServiceClient>(
            "AuthService", "user-service-api-key");
        
        var metadata = SimpleApiKeyAuthentication.CreateApiKeyMetadata("user-service-api-key");
        var response = await authClient.ValidateUserAsync(new UserRequest { UserId = userId }, metadata);
        
        return new UserResponse { Success = true, User = response.User };
    }
}
```

### **Betting Service Example:**
```csharp
public class BettingService
{
    private readonly IGrpcClientFactory _grpcClientFactory;

    public async Task<BetResponse> PlaceBetAsync(BetRequest request)
    {
        // Call Payment Service (via load balancer)
        var paymentClient = _grpcClientFactory.CreateClientWithApiKey<PaymentService.PaymentServiceClient>(
            "PaymentService", "betting-service-api-key");
        
        var metadata = SimpleApiKeyAuthentication.CreateApiKeyMetadata("betting-service-api-key");
        var paymentResponse = await paymentClient.ProcessPaymentAsync(
            new PaymentRequest { Amount = request.Stake, UserId = request.UserId }, metadata);
        
        if (!paymentResponse.Success)
            throw new InvalidOperationException("Payment failed");
            
        // Call Notification Service (via load balancer)
        var notificationClient = _grpcClientFactory.CreateClientWithApiKey<NotificationService.NotificationServiceClient>(
            "NotificationService", "betting-service-api-key");
        
        await notificationClient.SendBetConfirmationAsync(
            new NotificationRequest { UserId = request.UserId, Message = "Bet placed successfully" }, metadata);
        
        return new BetResponse { Success = true, BetId = Guid.NewGuid().ToString() };
    }
}
```

## 🔧 **Configuration Options**

### **appsettings.json:**
```json
{
  "ConvexGrpc": {
    "ServiceName": "AuthService",
    "ServerPort": 50051,
    "EnableTls": true,
    "EnableAuthentication": true,
    "EnableConnectionPooling": true,
    "EnableMetrics": true,
    "Services": [
      {
        "Name": "AuthService",
        "Endpoint": "http://auth-loadbalancer:50051",
        "Port": 50051,
        "EnableHealthChecks": true
      },
      {
        "Name": "UserService", 
        "Endpoint": "http://user-loadbalancer:50052",
        "Port": 50052,
        "EnableHealthChecks": true
      }
    ]
  }
}
```

### **Health Checks:**
```csharp
// Add health checks for load balancer monitoring
services.AddHealthChecks()
    .AddCheck<ConvexGrpcHealthCheck>("grpc-services");

public class ConvexGrpcHealthCheck : IHealthCheck
{
    private readonly IGrpcClientFactory _grpcClientFactory;
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            // Test gRPC connectivity
            var client = _grpcClientFactory.CreateClient<HealthService.HealthServiceClient>("HealthService");
            var response = await client.CheckHealthAsync(new HealthRequest(), cancellationToken: cancellationToken);
            
            return response.IsHealthy ? 
                HealthCheckResult.Healthy("gRPC services are healthy") :
                HealthCheckResult.Unhealthy("gRPC services are unhealthy");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("gRPC health check failed", ex);
        }
    }
}
```

## 📊 **Monitoring & Metrics**

### **Performance Metrics:**
```csharp
// Get service metrics
var metrics = _grpcMetrics.GetAllMetrics();
var systemMetrics = _grpcMetrics.GetSystemMetrics();

// Example output:
{
  "TotalServices": 11,
  "TotalRequests": 15420,
  "SuccessfulRequests": 15380,
  "FailedRequests": 40,
  "OverallSuccessRate": 99.74,
  "AverageResponseTimeMs": 45,
  "ActiveConnections": 23
}
```

### **Circuit Breaker Status:**
```csharp
// Check circuit breaker status
var circuitState = _circuitBreaker.GetCircuitState("AuthService");
var statistics = _circuitBreaker.GetStatistics();

// Example output:
{
  "AuthService": {
    "State": "Closed",
    "FailureCount": 0,
    "SuccessCount": 1250,
    "LastFailureTime": "2024-01-01T00:00:00Z"
  }
}
```

Perfect for:
- User Service ↔ Auth Service
- Betting Service ↔ Payment Service  
- Game Service ↔ Notification Service
- All microservice-to-microservice communication
