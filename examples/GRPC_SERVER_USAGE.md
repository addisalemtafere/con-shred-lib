# gRPC Server Usage Guide

## 🚀 **Complete gRPC Server + Client Implementation**

### **✅ What We Now Have:**

#### **1. gRPC CLIENT (Already implemented)**
- ✅ **IGrpcClientFactory** - Create clients to call other services
- ✅ **ConvexGrpcClientFactory** - High-performance client factory
- ✅ **Connection pooling** - Reuses connections
- ✅ **Service discovery** - Automatic endpoint resolution

#### **2. gRPC SERVER (Newly added)**
- ✅ **IGrpcServerService** - Base interface for gRPC servers
- ✅ **ConvexGrpcServer** - High-performance gRPC server
- ✅ **Automatic registration** - Easy server setup
- ✅ **Port configuration** - Configurable server ports

### **🎯 How to Use gRPC Server:**

#### **Step 1: Add gRPC to Your Microservice**
```csharp
// In your Program.cs
var builder = WebApplication.CreateBuilder();

// ✅ Add both gRPC client and server
builder.Services.AddConvexGrpc();

var app = builder.Build();
app.Run();
```

#### **Step 2: Create gRPC Server Service**
```csharp
// Create your gRPC server service
public class PaymentGrpcService : PaymentService.PaymentServiceBase
{
    public override async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request, ServerCallContext context)
    {
        // Your payment processing logic here
        return new PaymentResponse { Success = true };
    }
}
```

#### **Step 3: Register gRPC Server**
```csharp
// In your Program.cs
var app = builder.Build();

// ✅ Map your gRPC service
app.MapGrpcService<PaymentGrpcService>();

app.Run();
```

#### **Step 4: Use gRPC Client (Already implemented)**
```csharp
// In another service
public class BettingService
{
    private readonly IGrpcClientFactory _grpcClientFactory;

    public async Task<bool> PlaceBetAsync(string userId, decimal amount)
    {
        // ✅ Call PaymentService via gRPC CLIENT
        var paymentClient = _grpcClientFactory.CreateClient<PaymentService.PaymentServiceClient>("PaymentService");
        var result = await paymentClient.ProcessPaymentAsync(new PaymentRequest { UserId = userId, Amount = amount });
        return result.Success;
    }
}
```

### **🏆 Complete Architecture:**

```
┌─────────────────────────────────────────────────────────────┐
│                    gRPC Server + Client                   │
├─────────────────────────────────────────────────────────────┤
│  PaymentService:                                          │
│  ✅ SERVER - Handles payment requests (gRPC Server)      │
│  ✅ CLIENT - Calls UserService for user info (gRPC Client)│
│                                                             │
│  BettingService:                                           │
│  ✅ SERVER - Handles betting requests (gRPC Server)       │
│  ✅ CLIENT - Calls PaymentService for payments (gRPC Client)│
│                                                             │
│  UserService:                                              │
│  ✅ SERVER - Handles user requests (gRPC Server)          │
│  ✅ CLIENT - Calls AuthService for auth (gRPC Client)     │
└─────────────────────────────────────────────────────────────┘
```

### **🎯 Service Ports:**

```csharp
// Default gRPC server ports:
AuthService        → Port 50051
UserService        → Port 50052
BettingService     → Port 50053
PaymentService     → Port 50054
GameService        → Port 50055
NotificationService → Port 50056
AdminService       → Port 50057
```

### **🚀 Benefits:**

- ✅ **Complete gRPC Solution** - Both server and client
- ✅ **High Performance** - 7-10x faster than HTTP/JSON
- ✅ **Type Safety** - Strong typing with Protocol Buffers
- ✅ **Connection Pooling** - Efficient resource usage
- ✅ **Service Discovery** - Automatic endpoint resolution
- ✅ **Production Ready** - TLS support, proper disposal

### **📝 Summary:**

**Now you have BOTH:**
1. **✅ gRPC CLIENT** - Call other services (using IGrpcClientFactory)
2. **✅ gRPC SERVER** - Handle requests from other services (using MapGrpcService)

**Each microservice can be both a SERVER (receives requests) and a CLIENT (makes requests) using gRPC!** 🎉
