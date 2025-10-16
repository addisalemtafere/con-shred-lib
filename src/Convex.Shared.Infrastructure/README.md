# Convex.Shared.Infrastructure

## Overview
Infrastructure components for Convex microservices (messaging, caching, etc.) following SOLID principles.

## Features
- **KafkaFlow Integration**: Professional Kafka messaging
- **Generic Message Handlers**: Type-safe message processing
- **Background Services**: Reliable message consumption
- **Configuration**: Flexible Kafka setup

## SOLID Principles
- **Single Responsibility**: Each component has one clear purpose
- **Open/Closed**: Extensible through configuration and middleware
- **Liskov Substitution**: Proper inheritance hierarchy
- **Interface Segregation**: Focused interfaces
- **Dependency Inversion**: Depends on abstractions

## SOAR Compliance
- **Scalable**: Configurable buffer sizes and workers
- **Observable**: Comprehensive logging and monitoring
- **Available**: Health checks and fault tolerance
- **Reliable**: Retry logic and error handling

## Usage
```csharp
// Register KafkaFlow services
services.AddKafkaFlowConsumers(configuration);
services.AddKafkaFlowProducers(configuration);
services.AddKafkaFlowBackgroundServices();

// Create message handlers
public class OrderEventHandler : IMessageHandler<OrderEvent>
{
    public async Task HandleAsync(OrderEvent message)
    {
        // Handle the message
    }
}

// Register handlers
services.AddScoped<IMessageHandler<OrderEvent>, OrderEventHandler>();
```

## Configuration
```json
{
  "Kafka": {
    "BootstrapServers": "localhost:9092",
    "GroupId": "my-service-group",
    "DefaultTopic": "my-topic",
    "BufferSize": 100,
    "WorkersCount": 2,
    "AutoOffsetReset": "Earliest"
  }
}
```

## Dependencies
- KafkaFlow
- Microsoft.Extensions.DependencyInjection.Abstractions
- Microsoft.Extensions.Hosting
- Microsoft.Extensions.Logging.Abstractions
