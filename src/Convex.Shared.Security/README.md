# Convex.Shared.Security

Security and authentication utilities for Convex microservices.

## Features

- **JWT Authentication**: JWT token generation and validation
- **API Key Management**: Service-to-service authentication
- **Password Hashing**: Secure password hashing with salt
- **CORS Configuration**: Cross-origin resource sharing setup
- **Rate Limiting**: Built-in rate limiting support
- **Secure Random**: Cryptographically secure random string generation

## Installation

```xml
<PackageReference Include="Convex.Shared.Security" Version="1.0.0" />
```

## Quick Start

### 1. Register Services

```csharp
// In Program.cs
services.AddConvexSecurity(options =>
{
    options.JwtSecret = "your-jwt-secret-key";
    options.JwtIssuer = "Convex";
    options.JwtAudience = "ConvexUsers";
});
```

### 2. Use in Your Service

```csharp
public class AuthService
{
    private readonly IConvexSecurityService _securityService;

    public AuthService(IConvexSecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        // Validate user credentials
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null || !_securityService.VerifyPassword(password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        // Generate JWT token
        var token = _securityService.GenerateJwtToken(
            user.Id.ToString(), 
            user.Email, 
            user.Roles.ToArray());

        return token;
    }
}
```

## JWT Authentication

### Generate JWT Token

```csharp
var token = _securityService.GenerateJwtToken(
    userId: "123",
    email: "user@example.com",
    roles: new[] { "User", "Admin" },
    expiresInMinutes: 60);
```

### Validate JWT Token

```csharp
if (_securityService.ValidateJwtToken(token))
{
    var claims = _securityService.GetClaimsFromToken(token);
    var userId = claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}
```

### Configure JWT Authentication

```csharp
services.AddConvexJwtAuthentication(options =>
{
    options.JwtSecret = "your-jwt-secret-key";
    options.JwtIssuer = "Convex";
    options.JwtAudience = "ConvexUsers";
});
```

## API Key Authentication

### Generate API Key

```csharp
var apiKey = _securityService.GenerateApiKey(
    serviceName: "UserService",
    expiresAt: DateTime.UtcNow.AddDays(30));
```

### Validate API Key

```csharp
if (_securityService.ValidateApiKey(apiKey))
{
    var keyInfo = _securityService.GetApiKeyInfo(apiKey);
    var serviceName = keyInfo?.ServiceName;
}
```

### Configure API Key Authentication

```csharp
services.AddConvexApiKeyAuthentication(options =>
{
    options.ApiKeySecret = "your-api-key-secret";
});
```

## Password Security

### Hash Password

```csharp
var hashedPassword = _securityService.HashPassword("userPassword123");
```

### Verify Password

```csharp
if (_securityService.VerifyPassword("userPassword123", hashedPassword))
{
    // Password is correct
}
```

## CORS Configuration

```csharp
services.AddConvexCors(options =>
{
    options.AllowedOrigins = new[] { "https://app.convex.com", "https://admin.convex.com" };
});
```

## Configuration

### appsettings.json

```json
{
  "ConvexSecurity": {
    "JwtSecret": "your-jwt-secret-key-here",
    "JwtIssuer": "Convex",
    "JwtAudience": "ConvexUsers",
    "JwtExpirationMinutes": 60,
    "ApiKeySecret": "your-api-key-secret",
    "PasswordHashIterations": 100000,
    "RequireHttps": true,
    "AllowedOrigins": [
      "https://app.convex.com",
      "https://admin.convex.com"
    ],
    "RateLimit": {
      "Enabled": true,
      "MaxRequestsPerMinute": 100,
      "MaxRequestsPerHour": 1000,
      "MaxRequestsPerDay": 10000
    }
  }
}
```

### Environment Variables

```bash
export CONVEX_JWT_SECRET="your-jwt-secret-key"
export CONVEX_JWT_ISSUER="Convex"
export CONVEX_JWT_AUDIENCE="ConvexUsers"
export CONVEX_API_KEY_SECRET="your-api-key-secret"
```

## Security Best Practices

### JWT Security
1. **Use Strong Secrets**: Use cryptographically strong secrets
2. **Short Expiration**: Use short token expiration times
3. **HTTPS Only**: Always use HTTPS in production
4. **Validate Claims**: Always validate token claims

### API Key Security
1. **Rotate Keys**: Regularly rotate API keys
2. **Monitor Usage**: Monitor API key usage
3. **Set Expiration**: Set expiration dates for API keys
4. **Secure Storage**: Store API keys securely

### Password Security
1. **Strong Hashing**: Use strong hashing algorithms
2. **Salt**: Always use salt for password hashing
3. **Iterations**: Use sufficient iterations for PBKDF2
4. **Validation**: Implement password strength validation

## Rate Limiting

```csharp
services.AddConvexSecurity(options =>
{
    options.RateLimit.Enabled = true;
    options.RateLimit.MaxRequestsPerMinute = 100;
    options.RateLimit.MaxRequestsPerHour = 1000;
    options.RateLimit.MaxRequestsPerDay = 10000;
});
```

## Secure Random Generation

```csharp
// Generate secure random string
var randomString = _securityService.GenerateSecureRandomString(32);

// Generate secure random bytes
var randomBytes = new byte[32];
using var rng = RandomNumberGenerator.Create();
rng.GetBytes(randomBytes);
```

## License

This project is licensed under the MIT License.
