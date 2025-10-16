# How to Use gRPC in Your Convex Project

## 🚀 **Super Simple gRPC Usage**

### **Step 1: Add gRPC to Your Microservice**

```csharp
// In your Program.cs
var builder = WebApplication.CreateBuilder();

// ✅ Add gRPC
builder.Services.AddConvexGrpc();

var app = builder.Build();
app.Run();
```

### **Step 2: Use gRPC in Your Services**

```csharp
public class BettingService
{
    private readonly IGrpcClientFactory _grpcClientFactory;

    public BettingService(IGrpcClientFactory grpcClientFactory)
    {
        _grpcClientFactory = grpcClientFactory;
    }

    public async Task<bool> PlaceBetAsync(string userId, decimal amount)
    {
        // ✅ Call PaymentService via gRPC
        var paymentClient = _grpcClientFactory.CreateClient<PaymentService.PaymentServiceClient>("PaymentService");
        
        var result = await paymentClient.ProcessPaymentAsync(new PaymentRequest 
        { 
            UserId = userId,
            Amount = amount 
        });
        
        return result.Success;
    }
}
```

### **Step 3: Call Multiple Services**

```csharp
public class UserService
{
    private readonly IGrpcClientFactory _grpcClientFactory;

    public UserService(IGrpcClientFactory grpcClientFactory)
    {
        _grpcClientFactory = grpcClientFactory;
    }

    public async Task<string> GetUserAsync(string userId)
    {
        // ✅ Call UserService via gRPC
        var userClient = _grpcClientFactory.CreateClient<UserService.UserServiceClient>("UserService");
        
        var result = await userClient.GetUserAsync(new UserRequest { UserId = userId });
        
        return result.Name;
    }
}
```

## 🎯 **That's It! 3 Simple Steps:**

1. **Add gRPC**: `services.AddConvexGrpc()`
2. **Inject factory**: `IGrpcClientFactory _grpcClientFactory`
3. **Use client**: `_grpcClientFactory.CreateClient<YourClient>("ServiceName")`

## ✅ **Benefits:**

- **7x faster** than HTTP/JSON
- **Type-safe** communication
- **Simple** to use
- **Automatic** connection pooling

## 🔧 **Service Names:**

```csharp
// Use these service names:
"UserService"        // For user operations
"PaymentService"     // For payment operations
"BettingService"     // For betting operations
"GameService"        // For game operations
"NotificationService" // For notifications
"AdminService"       // For admin operations
```

## 📝 **Example Usage:**

```csharp
// ✅ Get user info
var userClient = _grpcClientFactory.CreateClient<UserService.UserServiceClient>("UserService");
var user = await userClient.GetUserAsync(new UserRequest { UserId = "123" });

// ✅ Process payment
var paymentClient = _grpcClientFactory.CreateClient<PaymentService.PaymentServiceClient>("PaymentService");
var payment = await paymentClient.ProcessPaymentAsync(new PaymentRequest { UserId = "123", Amount = 100 });

// ✅ Place bet
var bettingClient = _grpcClientFactory.CreateClient<BettingService.BettingServiceClient>("BettingService");
var bet = await bettingClient.PlaceBetAsync(new BetRequest { UserId = "123", Amount = 50 });
```

**That's it! Super simple gRPC usage in your Convex project!** 🚀
