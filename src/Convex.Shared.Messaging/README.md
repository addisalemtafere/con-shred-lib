# Convex.Shared.Messaging

Messaging utilities for Convex microservices using Apache Kafka.

## Features

- **Kafka Integration**: Full Apache Kafka support
- **Topic Publishing**: Publish messages to Kafka topics
- **Consumer Groups**: Kafka consumer group support
- **Automatic Recovery**: Built-in connection recovery
- **JSON Serialization**: Automatic message serialization
- **Error Handling**: Robust error handling and retry logic

## Installation

```xml
<PackageReference Include="Convex.Shared.Messaging" Version="1.0.0" />
```

## Quick Start

### 1. Register Services

```csharp
// In Program.cs
services.AddConvexMessaging(options =>
{
    options.BootstrapServers = "localhost:9092";
    options.SecurityProtocol = "PLAINTEXT";
    options.ConsumerGroup = "convex-group";
});
```

### 2. Use in Your Service

```csharp
public class UserService
{
    private readonly IConvexMessageBus _messageBus;

    public UserService(IConvexMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        // Create user in database
        var createdUser = await _userRepository.CreateAsync(user);
        
        // Publish user created event
        await _messageBus.PublishAsync("user.created", new UserCreatedEvent
        {
            UserId = createdUser.Id,
            Email = createdUser.Email,
            CreatedAt = DateTime.UtcNow
        });
        
        return createdUser;
    }
}
```

## Topic Publishing

### Publish Messages
```csharp
// Publish to topic
await _messageBus.PublishAsync("user.created", new UserCreatedEvent
{
    UserId = 123,
    Email = "user@example.com",
    CreatedAt = DateTime.UtcNow
});

// Publish to multiple topics
await _messageBus.PublishAsync("bet.placed", new BetPlacedEvent
{
    BetId = 456,
    UserId = 123,
    Amount = 100.00,
    Odds = 2.5
});
```

### Subscribe to Topics
```csharp
// Subscribe to topic
var subscriptionId = await _messageBus.SubscribeAsync<UserCreatedEvent>(
    "user.created", 
    async (message) =>
    {
        Console.WriteLine($"User {message.UserId} created with email {message.Email}");
        // Handle user created event
    });

// Unsubscribe
await _messageBus.UnsubscribeAsync(subscriptionId);
```

## Queue Management

### Send Messages
```csharp
// Send to queue
await _messageBus.SendAsync("user.notifications", new NotificationMessage
{
    UserId = 123,
    Type = "Welcome",
    Content = "Welcome to Convex!"
});
```

### Receive Messages
```csharp
// Receive from queue
await _messageBus.ReceiveAsync<NotificationMessage>(
    "user.notifications",
    async (message) =>
    {
        await _notificationService.SendAsync(message);
    });
```

## Message Types

### Event Messages
```csharp
public class UserCreatedEvent
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class BetPlacedEvent
{
    public int BetId { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public decimal Odds { get; set; }
    public DateTime PlacedAt { get; set; }
}
```

### Command Messages
```csharp
public class SendEmailCommand
{
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}

public class ProcessPaymentCommand
{
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
}
```

## Configuration

### appsettings.json
```json
{
  "ConvexMessaging": {
    "BootstrapServers": "localhost:9092",
    "SecurityProtocol": "PLAINTEXT",
    "SaslMechanism": "PLAIN",
    "SaslUsername": "",
    "SaslPassword": "",
    "SslCaLocation": "",
    "TopicPrefix": "convex",
    "ConsumerGroup": "convex-group",
    "AutoOffsetReset": "earliest",
    "EnableAutoCommit": true,
    "AutoCommitIntervalMs": 5000,
    "SessionTimeoutMs": 30000,
    "RequestTimeoutMs": 30000,
    "MaxRetryAttempts": 3,
    "RetryDelaySeconds": 5,
    "MessageRetentionMs": 604800000
  }
}
```

### Environment Variables
```bash
export KAFKA_BOOTSTRAP_SERVERS=localhost:9092
export KAFKA_SECURITY_PROTOCOL=PLAINTEXT
export KAFKA_CONSUMER_GROUP=convex-group
export KAFKA_TOPIC_PREFIX=convex
```

## Error Handling

### Retry Logic
```csharp
public async Task<bool> PublishWithRetryAsync<T>(string topic, T message)
{
    for (int attempt = 1; attempt <= 3; attempt++)
    {
        try
        {
            return await _messageBus.PublishAsync(topic, message);
        }
        catch (Exception ex)
        {
            if (attempt == 3)
                throw;
                
            await Task.Delay(TimeSpan.FromSeconds(attempt * 2));
        }
    }
    return false;
}
```

### Dead Letter Topic
```csharp
// Configure dead letter topic for failed messages
var deadLetterTopic = "convex.dlq";

// Publish failed messages to dead letter topic
await _messageBus.PublishAsync(deadLetterTopic, new DeadLetterMessage
{
    OriginalTopic = originalTopic,
    OriginalMessage = originalMessage,
    ErrorMessage = ex.Message,
    FailedAt = DateTime.UtcNow
});
```

## Best Practices

1. **Use Topics for Events**: Use Kafka topics for event-driven communication
2. **Consumer Groups**: Use consumer groups for load balancing
3. **Handle Errors**: Implement proper error handling and retry logic
4. **Monitor Messages**: Monitor message flow and performance
5. **Use Dead Letter Topics**: Handle failed messages appropriately
6. **Partitioning**: Consider message partitioning for scalability

## License

This project is licensed under the MIT License.
