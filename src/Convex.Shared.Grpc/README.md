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

```csharp
// In your microservice Startup.cs
services.AddConvexGrpc();
```

### 2. Use gRPC Client

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
        var client = _grpcClientFactory.CreateClient<UserService.UserServiceClient>();
        return await client.GetUserAsync(new UserRequest { UserId = userId });
    }
}
```

## Performance Benefits

- **7-10x faster** than HTTP/JSON
- **Binary serialization** with Protocol Buffers
- **HTTP/2 multiplexing** for concurrent requests
- **Built-in compression** for large payloads
- **Connection pooling** for efficiency

## Service Communication

Perfect for:
- User Service ↔ Auth Service
- Betting Service ↔ Payment Service  
- Game Service ↔ Notification Service
- All microservice-to-microservice communication
