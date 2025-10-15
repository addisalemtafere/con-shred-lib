# Convex.Shared.Common

Common models, DTOs, and utilities for Convex microservices.

## Features

- **Base Models**: BaseEntity, ApiResponse, and common DTOs
- **Enums**: Standard result status and common enumerations
- **Constants**: API constants, headers, and cache keys
- **Extensions**: String and other utility extensions
- **Utilities**: Common helper methods and utilities

## Installation

```xml
<PackageReference Include="Convex.Shared.Common" Version="1.0.0" />
```

## Quick Start

### Base Entity
```csharp
public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
```

### API Response
```csharp
// Success response
var response = ApiResponse<User>.SuccessResult(user);

// Error response
var errorResponse = ApiResponse<User>.ErrorResult("User not found");
```

### String Extensions
```csharp
var email = "user@example.com";
if (email.IsValidEmail())
{
    // Valid email
}

var title = "hello world";
var titleCase = title.ToTitleCase(); // "Hello World"
```

### Constants
```csharp
// Use API constants
var pageSize = ApiConstants.DefaultPageSize;
var correlationId = HeaderConstants.CorrelationId;
var cacheKey = CacheConstants.UserCachePrefix + userId;
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

## Best Practices

1. **Use BaseEntity**: Inherit from BaseEntity for all domain models
2. **Use ApiResponse**: Wrap all API responses with ApiResponse<T>
3. **Use Constants**: Use predefined constants instead of magic strings
4. **Use Extensions**: Leverage extension methods for common operations
5. **Use Enums**: Use ResultStatus for consistent error handling

## License

This project is licensed under the MIT License.
