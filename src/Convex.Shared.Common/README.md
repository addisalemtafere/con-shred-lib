# Convex.Shared.Common

**Foundation library for Convex microservices** - Provides essential base models, DTOs, utilities, and constants for consistent API development across all Convex services.

## 🚀 Key Features

- **🏗️ Base Models**: BaseEntity with common properties (Id, CreatedAt, UpdatedAt, IsDeleted)
- **📦 API Responses**: Standardized ApiResponse<T> wrapper with success/error handling
- **📊 Status Enums**: ResultStatus enum for consistent error categorization
- **🔧 Constants**: Predefined API, header, and cache constants
- **⚡ Extensions**: Rich string extension methods for common operations
- **🛠️ Utilities**: Helper methods and common functionality

## Installation

```xml
<PackageReference Include="Convex.Shared.Common" Version="1.0.0" />
```

## 🚀 Quick Start

### Base Entity
```csharp
public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

// Automatic properties from BaseEntity:
// - Id (Guid) - Auto-generated unique identifier
// - CreatedAt (DateTime) - Set to current UTC time
// - UpdatedAt (DateTime) - Set to current UTC time  
// - IsDeleted (bool) - Soft delete flag (default: false)
```

### API Response
```csharp
// Success response with data
var user = new User { Name = "John Doe", Email = "john@example.com" };
var successResponse = ApiResponse<User>.SuccessResult(user);

// Error response with message
var errorResponse = ApiResponse<User>.ErrorResult("User not found");

// Error response with multiple validation errors
var validationErrors = new Dictionary<string, string>
{
    { "Email", "Email is required" },
    { "Name", "Name must be at least 2 characters" }
};
var validationResponse = ApiResponse<User>.ErrorResult(validationErrors);

// Usage in controllers
[HttpGet("{id}")]
public async Task<ApiResponse<User>> GetUser(Guid id)
{
    var user = await _userService.GetByIdAsync(id);
    if (user == null)
        return ApiResponse<User>.ErrorResult("User not found");
    
    return ApiResponse<User>.SuccessResult(user);
}
```

### String Extensions
```csharp
// Email validation
var email = "user@example.com";
if (email.IsValidEmail())
{
    // Valid email format
}

// Text formatting
var title = "hello world";
var titleCase = title.ToTitleCase(); // "Hello World"

var slug = "My Blog Post Title";
var urlSlug = slug.ToSlug(); // "my-blog-post-title"

// String manipulation
var longText = "This is a very long text that needs to be truncated";
var shortText = longText.Truncate(20); // "This is a very lo..."

// Null/empty checks
var text = "";
if (text.IsNullOrEmpty()) { /* handle empty */ }
if (text.IsNullOrWhiteSpace()) { /* handle whitespace */ }
```

### Constants Usage
```csharp
// API pagination
var pageSize = ApiConstants.DefaultPageSize; // 20
var maxPageSize = ApiConstants.MaxPageSize; // 100

// HTTP headers
var correlationId = HeaderConstants.CorrelationId; // "X-Correlation-ID"
var requestId = HeaderConstants.RequestId; // "X-Request-ID"
var apiKey = HeaderConstants.ApiKey; // "X-API-Key"

// Cache keys
var userCacheKey = CacheConstants.UserCachePrefix + userId; // "user:12345"
var sessionCacheKey = CacheConstants.SessionCachePrefix + sessionId; // "session:abc123"

// Timeouts
var timeout = TimeSpan.FromSeconds(ApiConstants.DefaultTimeoutSeconds); // 30 seconds
```

## Models

### BaseEntity
Base class for all domain entities with common properties:
- `Id`: Unique identifier
- `CreatedAt`: Creation timestamp
- `UpdatedAt`: Last update timestamp
- `IsDeleted`: Soft delete flag

### ApiResponse<T>
Standard API response wrapper:
- `Success`: Operation success flag
- `Data`: Response data
- `Error`: Error message
- `Errors`: Additional error details
- `Timestamp`: Response timestamp

## Enums

### ResultStatus
Standard result status enumeration:
- `Success`: Operation successful
- `ValidationError`: Validation failed
- `BusinessError`: Business logic error
- `SystemError`: System error
- `AuthenticationError`: Authentication failed
- `AuthorizationError`: Authorization failed
- `NotFound`: Resource not found
- `Timeout`: Operation timeout

## Constants

### ApiConstants
- `DefaultPageSize`: Default pagination size (20)
- `MaxPageSize`: Maximum pagination size (100)
- `DefaultTimeoutSeconds`: Default timeout (30s)
- `MaxTimeoutSeconds`: Maximum timeout (300s)
- `DefaultRetryAttempts`: Default retry attempts (3)
- `MaxRetryAttempts`: Maximum retry attempts (10)

### HeaderConstants
- `CorrelationId`: "X-Correlation-ID"
- `RequestId`: "X-Request-ID"
- `ApiKey`: "X-API-Key"
- `UserId`: "X-User-ID"
- `TenantId`: "X-Tenant-ID"

### CacheConstants
- `DefaultExpirationMinutes`: Default cache expiration (15m)
- `ShortExpirationMinutes`: Short cache expiration (5m)
- `LongExpirationMinutes`: Long cache expiration (60m)
- `UserCachePrefix`: "user:"
- `SessionCachePrefix`: "session:"

## Extensions

### StringExtensions
- `IsNullOrEmpty()`: Check if null or empty
- `IsNullOrWhiteSpace()`: Check if null, empty, or whitespace
- `ToTitleCase()`: Convert to title case
- `Truncate()`: Truncate to specified length
- `IsValidEmail()`: Validate email format
- `ToSlug()`: Convert to slug format

## 🎯 Real-World Use Cases

### Microservice API Development
```csharp
// Consistent API responses across all services
[HttpGet]
public async Task<ApiResponse<List<User>>> GetUsers(int page = 1, int size = ApiConstants.DefaultPageSize)
{
    var users = await _userService.GetUsersAsync(page, size);
    return ApiResponse<List<User>>.SuccessResult(users);
}

[HttpPost]
public async Task<ApiResponse<User>> CreateUser(CreateUserRequest request)
{
    // Validation using string extensions
    if (request.Email.IsNullOrEmpty() || !request.Email.IsValidEmail())
    {
        return ApiResponse<User>.ErrorResult("Invalid email format");
    }
    
    var user = await _userService.CreateAsync(request);
    return ApiResponse<User>.SuccessResult(user);
}
```

### Domain Entity Modeling
```csharp
public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    
    // Inherits: Id, CreatedAt, UpdatedAt, IsDeleted
}

public class Order : BaseEntity
{
    public Guid CustomerId { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    
    // Inherits: Id, CreatedAt, UpdatedAt, IsDeleted
}
```

### Error Handling and Logging
```csharp
public async Task<ApiResponse<Order>> ProcessOrderAsync(OrderRequest request)
{
    try
    {
        // Business logic
        var order = await _orderService.CreateOrderAsync(request);
        
        // Log success with correlation ID
        _logger.LogInformation("Order {OrderId} created successfully", order.Id);
        
        return ApiResponse<Order>.SuccessResult(order);
    }
    catch (ValidationException ex)
    {
        _logger.LogWarning("Validation failed for order: {Errors}", ex.Message);
        return ApiResponse<Order>.ErrorResult(ex.Message);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to process order");
        return ApiResponse<Order>.ErrorResult("Internal server error");
    }
}
```

### Cache Key Management
```csharp
public class UserService
{
    public async Task<User?> GetUserAsync(Guid userId)
    {
        // Consistent cache key generation
        var cacheKey = $"{CacheConstants.UserCachePrefix}{userId}";
        
        var cachedUser = await _cache.GetAsync<User>(cacheKey);
        if (cachedUser != null)
            return cachedUser;
            
        var user = await _repository.GetByIdAsync(userId);
        if (user != null)
        {
            await _cache.SetAsync(cacheKey, user, 
                TimeSpan.FromMinutes(CacheConstants.DefaultExpirationMinutes));
        }
        
        return user;
    }
}
```

## 📋 Best Practices

1. **🏗️ Use BaseEntity**: Inherit from BaseEntity for all domain models to ensure consistency
2. **📦 Use ApiResponse**: Wrap all API responses with ApiResponse<T> for standardized responses
3. **🔧 Use Constants**: Use predefined constants instead of magic strings for maintainability
4. **⚡ Use Extensions**: Leverage extension methods for common string operations
5. **📊 Use Enums**: Use ResultStatus for consistent error categorization
6. **🔄 Update Timestamps**: Always update UpdatedAt when modifying entities
7. **🗑️ Soft Delete**: Use IsDeleted flag instead of hard deletes for audit trails
8. **📝 Log Consistently**: Use correlation IDs and structured logging
9. **🔑 Cache Keys**: Use consistent cache key patterns with constants
10. **✅ Validate Early**: Use string extensions for input validation

## License

This project is licensed under the MIT License.
