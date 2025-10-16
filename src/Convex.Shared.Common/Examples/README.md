# Convex Exception Handling Examples

This directory contains examples of how to implement comprehensive exception handling in your Convex microservices using the shared libraries.

## 📋 **Overview**

The Convex shared libraries provide a complete exception handling framework that includes:

- ✅ **Structured Exception Types** - Domain-specific exceptions with error codes
- ✅ **Structured Error Responses** - Consistent JSON error format
- ✅ **Correlation ID Tracking** - Request tracing across services
- ✅ **Security-Conscious Logging** - Never log sensitive data
- ✅ **Performance Optimized** - Efficient for high-volume scenarios

## 🚀 **Quick Start**

### 1. **Use Exception Classes in Your Microservice**

```csharp
// The exception classes are in Convex.Shared.Common
// No dependencies between shared libraries!

// In your microservice, use the exception classes:
throw new ValidationException("PAY_001", "Invalid amount", "Amount must be positive");
throw new BusinessException("PAY_002", "Daily limit exceeded", "You've reached your limit");
throw new NotFoundException("User", userId, "User not found", "User could not be found");

// Register your services independently:
services.AddConvexLogging("YourService", "1.0.0");
services.AddConvexCommon();
```

### 2. **Use Structured Exceptions in Your Code**

```csharp
public class PaymentService
{
    private readonly IConvexLogger _logger;

    public async Task ProcessPaymentAsync(PaymentRequest request)
    {
        try
        {
            // Your business logic here
            if (request.Amount <= 0)
            {
                throw new ValidationException(
                    "PAY_001",
                    "Invalid payment amount",
                    "Payment amount must be greater than zero",
                    new[] { new ValidationError { Field = "Amount", Code = "MIN_VALUE" } }
                );
            }

            // Process payment...
        }
        catch (ConvexException ex)
        {
            _logger.LogBusinessException(ex, correlationId, request.UserId);
            throw; // Re-throw to be handled by middleware
        }
    }
}
```

## 📊 **Exception Types**

### **ValidationException**
For input validation errors with detailed field-level errors.

```csharp
throw new ValidationException(
    "PAY_001",
    "Invalid payment amount",
    "Payment amount must be greater than zero",
    new[]
    {
        new ValidationError
        {
            Field = "Amount",
            Code = "MIN_VALUE",
            Message = "Amount must be greater than 0",
            AttemptedValue = -100,
            Constraint = new { Minimum = 0.01m }
        }
    }
);
```

### **BusinessException**
For business logic violations with user-friendly messages.

```csharp
throw new BusinessException(
    "PAY_002",
    "Daily limit exceeded",
    "You have reached your daily payment limit",
    new { DailyLimit = 10000m, CurrentTotal = 10000m },
    new[] { "Try again tomorrow", "Contact support for limit increase" }
);
```

### **NotFoundException**
For resource not found scenarios.

```csharp
throw new NotFoundException(
    "User",
    userId,
    $"User {userId} not found",
    "The requested user could not be found"
);
```

### **UnauthorizedException**
For authentication and authorization failures.

```csharp
throw new UnauthorizedException(
    "Authentication required",
    "Your session has expired. Please log in again.",
    new { Reason = "TOKEN_EXPIRED" },
    new[] { "Log in to continue", "Refresh your token" }
);
```

### **RateLimitException**
For rate limiting scenarios.

```csharp
throw new RateLimitException(
    limit: 100,
    remaining: 0,
    resetTime: DateTime.UtcNow.AddMinutes(1),
    retryAfter: 60
);
```

## 📝 **Error Response Format**

All exceptions are automatically converted to structured JSON responses:

```json
{
  "success": false,
  "timestamp": "2024-01-20T10:30:45.123Z",
  "correlationId": "corr-abc123-def456",
  "error": {
    "code": "PAY_001",
    "type": "ValidationException",
    "message": "Invalid payment amount",
    "userMessage": "Payment amount must be greater than zero",
    "validationErrors": [
      {
        "field": "Amount",
        "code": "MIN_VALUE",
        "message": "Amount must be greater than 0",
        "attemptedValue": -100,
        "constraint": { "minimum": 0.01 }
      }
    ]
  },
  "metadata": {
    "requestId": "req-789012",
    "service": "PaymentService",
    "version": "1.0.0",
    "environment": "Production"
  }
}
```

## 🔧 **Logging Integration**

### **Structured Exception Logging**

```csharp
// Log business exceptions with full context
_logger.LogBusinessException(exception, correlationId, userId, requestId);

// Log validation errors
_logger.LogValidationErrors(validationErrors, correlationId, userId);

// Log general exceptions with context
_logger.LogException(exception, "Payment processing failed", 
    "CorrelationId", correlationId,
    "UserId", userId,
    "Amount", amount);
```

### **Sample Log Output**

```json
{
  "Timestamp": "2024-01-20T10:30:45.123Z",
  "Level": "Error",
  "MessageTemplate": "Business exception occurred: {ExceptionType}",
  "Properties": {
    "ServiceName": "PaymentService",
    "Version": "1.0.0",
    "MachineName": "PROD-SERVER-01",
    "ProcessId": 1234,
    "CorrelationId": "corr-abc123-def456",
    "UserId": "user_12345",
    "RequestId": "req-789012",
    "ExceptionType": "BusinessException"
  },
  "Exception": "Convex.Shared.Common.Exceptions.BusinessException: Daily limit exceeded..."
}
```

## 🛡️ **Security Best Practices**

### **Never Log Sensitive Data**

```csharp
// ❌ WRONG - Never log passwords, tokens, or PII
_logger.LogInformation("User {UserId} logged in with password {Password}", userId, password);

// ✅ CORRECT - Log only safe identifiers
_logger.LogInformation("User {UserId} logged in successfully", userId);
```

### **Structured Context Logging**

```csharp
// ✅ CORRECT - Use structured logging with safe properties
_logger.LogBusinessException(exception, correlationId, userId, requestId);
```

## 🚀 **Performance Considerations**

### **High-Volume Exception Handling**

The exception handling is optimized for billion-record scenarios:

- ✅ **Efficient JSON Serialization** - Minimal allocations
- ✅ **Structured Logging** - Pre-allocated arrays
- ✅ **Correlation ID Caching** - Thread-safe AsyncLocal
- ✅ **Batch Logging Support** - For high-volume scenarios

### **Example: Batch Exception Logging**

```csharp
// For high-volume scenarios, use batch logging
var exceptions = new List<(string message, object[] properties)>();
foreach (var item in items)
{
    try
    {
        ProcessItem(item);
    }
    catch (Exception ex)
    {
        exceptions.Add(("Item processing failed", new object[] { "ItemId", item.Id, "Error", ex.Message }));
    }
}

// Log all exceptions at once
_logger.LogBatch(exceptions.ToArray());
```

## 📚 **Complete Example**

See `ExceptionHandlingExample.cs` for a complete working example that demonstrates:

- ✅ **Payment Processing** with validation and business rules
- ✅ **User Lookup** with proper not-found handling
- ✅ **Rate Limiting** with structured responses
- ✅ **Authentication** with security-conscious logging
- ✅ **Error Response Generation** with all exception types

## 🔗 **Integration with Other Libraries**

### **Independent Usage**

```csharp
// Each shared library is independent - no cross-dependencies!

// Use Convex.Shared.Logging independently:
services.AddConvexLogging("PaymentService", "1.0.0");

// Use Convex.Shared.Common independently:
services.AddConvexCommon();

// Use Convex.Shared.Caching independently:
services.AddConvexRedisCache("localhost:6379");

// Use Convex.Shared.Messaging independently:
services.AddConvexMessaging(options => { ... });
```

## 🎯 **Best Practices Summary**

1. **Use Structured Exceptions** - Always use Convex exception types
2. **Include Correlation IDs** - For request tracing
3. **Log with Context** - Include user ID, request ID, correlation ID
4. **Never Log Sensitive Data** - No passwords, tokens, or PII
5. **Use Appropriate Exception Types** - Match the business scenario
6. **Include User-Friendly Messages** - Help users understand the error
7. **Provide Actionable Suggestions** - Tell users what they can do
8. **Test Exception Scenarios** - Ensure proper error responses
9. **Monitor Exception Rates** - Track and alert on high error rates
10. **Use Performance Logging** - For high-volume scenarios

---

**🚀 Your Convex microservices now have enterprise-grade exception handling!**
