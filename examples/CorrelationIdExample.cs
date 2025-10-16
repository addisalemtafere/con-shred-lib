using Convex.Shared.Common.Services;
using Convex.Shared.Logging.Interfaces;
using Convex.Shared.Messaging.Interfaces;
using Convex.Shared.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Convex.Examples;

/// <summary>
/// Example demonstrating correlation ID usage across all Convex shared libraries
/// </summary>
public class CorrelationIdExample
{
    private readonly ICorrelationIdService _correlationIdService;
    private readonly IConvexLogger _logger;
    private readonly IConvexMessageBus _messageBus;
    private readonly IConvexHttpClient _httpClient;

    public CorrelationIdExample(
        ICorrelationIdService correlationIdService,
        IConvexLogger logger,
        IConvexMessageBus messageBus,
        IConvexHttpClient httpClient)
    {
        _correlationIdService = correlationIdService;
        _logger = logger;
        _messageBus = messageBus;
        _httpClient = httpClient;
    }

    /// <summary>
    /// Demonstrates correlation ID flow across all libraries
    /// </summary>
    public async Task DemonstrateCorrelationFlow()
    {
        // 1. Create a new correlation ID scope
        using var scope = _correlationIdService.CreateScope();
        var correlationId = _correlationIdService.GetCorrelationId();
        
        _logger.LogInformation("Starting correlation flow with ID: {CorrelationId}", correlationId);

        // 2. Logging automatically includes correlation ID
        _logger.LogInformation("Processing user request");
        _logger.LogBusinessEvent("UserLogin", new { UserId = "12345", Timestamp = DateTime.UtcNow });
        _logger.LogPerformance("DatabaseQuery", TimeSpan.FromMilliseconds(150));

        // 3. HTTP requests automatically include correlation ID in headers
        try
        {
            var userData = await _httpClient.GetAsync<UserDto>("/api/users/12345");
            _logger.LogInformation("Retrieved user data: {UserId}", userData?.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve user data");
        }

        // 4. Messaging automatically includes correlation ID in message headers
        var userEvent = new UserCreatedEvent
        {
            UserId = Guid.NewGuid(),
            Email = "user@example.com",
            CreatedAt = DateTime.UtcNow
        };

        var published = await _messageBus.PublishAsync("user.created", userEvent);
        if (published)
        {
            _logger.LogInformation("Published user created event");
        }

        // 5. Subscribe to messages (correlation ID will be automatically set from message headers)
        var subscriptionId = await _messageBus.SubscribeAsync<UserCreatedEvent>("user.created", async userEvent =>
        {
            _logger.LogInformation("Received user created event: {UserId}", userEvent.UserId);
            // Correlation ID is automatically set from message headers
            var currentCorrelationId = _correlationIdService.GetCorrelationId();
            _logger.LogInformation("Processing with correlation ID: {CorrelationId}", currentCorrelationId);
        });

        // 6. Demonstrate nested correlation scopes
        await DemonstrateNestedCorrelation();

        // 7. Clean up subscription
        await _messageBus.UnsubscribeAsync(subscriptionId);

        _logger.LogInformation("Correlation flow completed");
    }

    /// <summary>
    /// Demonstrates nested correlation ID scopes
    /// </summary>
    private async Task DemonstrateNestedCorrelation()
    {
        _logger.LogInformation("Starting nested correlation scope");

        // Create a nested correlation scope
        using var nestedScope = _correlationIdService.CreateScope("nested-correlation-123");
        var nestedCorrelationId = _correlationIdService.GetCorrelationId();
        
        _logger.LogInformation("Nested scope correlation ID: {CorrelationId}", nestedCorrelationId);

        // All operations in this scope will use the nested correlation ID
        await _messageBus.PublishAsync("nested.event", new { Message = "From nested scope" });
        
        _logger.LogInformation("Nested scope completed");
        // Correlation ID automatically restored to parent scope when disposed
    }
}

/// <summary>
/// Example of setting up all Convex services with correlation ID support
/// </summary>
public static class ServiceSetupExample
{
    public static IServiceCollection AddConvexServices(this IServiceCollection services)
    {
        // 1. Add Common services (includes correlation ID service)
        services.AddConvexCommon();

        // 2. Add Logging with correlation ID support
        services.AddConvexLogging("UserService", "1.0.0");

        // 3. Add Messaging with correlation ID support
        services.AddConvexMessaging(options =>
        {
            options.BootstrapServers = "localhost:9092";
            options.ConsumerGroup = "user-service";
            options.TopicPrefix = "convex";
        });

        // 4. Add HTTP client with correlation ID support
        services.AddConvexHttpClient(options =>
        {
            options.BaseAddress = new Uri("https://api.convex.com");
            options.Timeout = TimeSpan.FromSeconds(30);
        });

        // 5. Add other services
        services.AddConvexSecurity();
        services.AddConvexRedisCache("localhost:6379");
        services.AddConvexValidation();

        return services;
    }
}

/// <summary>
/// Example DTOs for the correlation flow
/// </summary>
public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}

public class UserCreatedEvent
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Example of using correlation ID in a web API controller
/// </summary>
public class UserController
{
    private readonly ICorrelationIdService _correlationIdService;
    private readonly IConvexLogger _logger;

    public UserController(ICorrelationIdService correlationIdService, IConvexLogger logger)
    {
        _correlationIdService = correlationIdService;
        _logger = logger;
    }

    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        // Correlation ID is automatically set from HTTP headers (X-Correlation-ID)
        var correlationId = _correlationIdService.GetCorrelationId();
        _logger.LogInformation("Creating user with correlation ID: {CorrelationId}", correlationId);

        // All subsequent operations will use this correlation ID
        // - Logging automatically includes it
        // - HTTP requests automatically include it in headers
        // - Message publishing automatically includes it in headers

        return Ok();
    }
}

public class CreateUserRequest
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}
