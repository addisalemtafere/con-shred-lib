# Convex.Shared.Caching

Caching utilities for Convex microservices.

## Features

- **Memory Caching**: In-memory caching for fast access
- **Redis Caching**: Distributed caching with Redis
- **JSON Serialization**: Automatic JSON serialization/deserialization
- **Expiration Support**: Configurable cache expiration
- **Bulk Operations**: Get/set multiple items at once
- **GetOrSet Pattern**: Lazy loading with caching

## Installation

```xml
<PackageReference Include="Convex.Shared.Caching" Version="1.0.0" />
```

## Quick Start

### 1. Register Services

```csharp
// Memory caching
services.AddConvexMemoryCache();

// Redis caching
services.AddConvexRedisCache("localhost:6379");
```

### 2. Use in Your Service

```csharp
public class UserService
{
    private readonly IConvexCache _cache;

    public UserService(IConvexCache cache)
    {
        _cache = cache;
    }

    public async Task<User?> GetUserAsync(int userId)
    {
        var cacheKey = $"user:{userId}";
        
        return await _cache.GetOrSetAsync(cacheKey, async () =>
        {
            return await _userRepository.GetByIdAsync(userId);
        }, TimeSpan.FromMinutes(15));
    }
}
```

## Memory Caching

### Basic Setup
```csharp
services.AddConvexMemoryCache();
```

### Usage
```csharp
// Set value
await _cache.SetAsync("key", "value", TimeSpan.FromMinutes(5));

// Get value
var value = await _cache.GetAsync<string>("key");

// Check if exists
var exists = await _cache.ExistsAsync("key");

// Remove value
await _cache.RemoveAsync("key");
```

## Redis Caching

### Basic Setup
```csharp
services.AddConvexRedisCache("localhost:6379");
```

### Advanced Configuration
```csharp
services.AddConvexRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "Convex";
});
```

## GetOrSet Pattern

### Lazy Loading
```csharp
public async Task<List<User>> GetUsersAsync()
{
    return await _cache.GetOrSetAsync("users:all", async () =>
    {
        return await _userRepository.GetAllAsync();
    }, TimeSpan.FromHours(1));
}
```

### With Factory Function
```csharp
public async Task<User> GetUserAsync(int userId)
{
    var cacheKey = $"user:{userId}";
    
    return await _cache.GetOrSetAsync(cacheKey, async () =>
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException($"User {userId} not found");
        return user;
    }, TimeSpan.FromMinutes(30));
}
```

## Bulk Operations

### Get Multiple Items
```csharp
var keys = new[] { "user:1", "user:2", "user:3" };
var users = await _cache.GetManyAsync<User>(keys);
```

### Set Multiple Items
```csharp
var users = new Dictionary<string, User>
{
    ["user:1"] = user1,
    ["user:2"] = user2,
    ["user:3"] = user3
};

await _cache.SetManyAsync(users, TimeSpan.FromMinutes(15));
```

### Remove Multiple Items
```csharp
var keys = new[] { "user:1", "user:2", "user:3" };
var removedCount = await _cache.RemoveManyAsync(keys);
```

## Cache Patterns

### Cache-Aside Pattern
```csharp
public async Task<User?> GetUserAsync(int userId)
{
    var cacheKey = $"user:{userId}";
    var user = await _cache.GetAsync<User>(cacheKey);
    
    if (user == null)
    {
        user = await _userRepository.GetByIdAsync(userId);
        if (user != null)
        {
            await _cache.SetAsync(cacheKey, user, TimeSpan.FromMinutes(30));
        }
    }
    
    return user;
}
```

### Write-Through Pattern
```csharp
public async Task<User> CreateUserAsync(User user)
{
    // Save to database
    var createdUser = await _userRepository.CreateAsync(user);
    
    // Update cache
    var cacheKey = $"user:{createdUser.Id}";
    await _cache.SetAsync(cacheKey, createdUser, TimeSpan.FromMinutes(30));
    
    return createdUser;
}
```

### Write-Behind Pattern
```csharp
public async Task UpdateUserAsync(User user)
{
    // Update cache immediately
    var cacheKey = $"user:{user.Id}";
    await _cache.SetAsync(cacheKey, user, TimeSpan.FromMinutes(30));
    
    // Queue for database update
    await _updateQueue.EnqueueAsync(user);
}
```

## Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "Redis": "localhost:6379"
  },
  "Cache": {
    "DefaultExpiration": "00:15:00",
    "UserExpiration": "00:30:00",
    "SessionExpiration": "01:00:00"
  }
}
```

### Environment Variables
```bash
export REDIS_CONNECTION_STRING="localhost:6379"
export CACHE_DEFAULT_EXPIRATION="00:15:00"
```

## Best Practices

1. **Use Appropriate Expiration**: Set reasonable expiration times
2. **Cache Keys**: Use consistent, descriptive cache keys
3. **Error Handling**: Handle cache failures gracefully
4. **Serialization**: Ensure objects are serializable
5. **Memory Usage**: Monitor memory usage for in-memory caching

## License

This project is licensed under the MIT License.
