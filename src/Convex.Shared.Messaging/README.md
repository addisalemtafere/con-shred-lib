📨 Convex.Shared.Messaging

A shared .NET library for easily integrating Kafka messaging (producers, consumers, and serializers) into Convex-based services.

This package provides ready-to-use dependency injection extensions for Kafka setup, configuration binding, and strongly-typed producers/consumers.

📦 Installation

Add the NuGet package reference (once you publish or share it):

dotnet add package Convex.Shared.Messaging


⚙️ If you’re referencing it locally in your solution, add a Project Reference instead:

dotnet add <YourProjectName> reference ../Convex.Shared.Messaging/Convex.Shared.Messaging.csproj

⚙️ Prerequisites

Make sure your project references these common dependencies (most are already included transitively):

dotnet add package Microsoft.Extensions.Options.ConfigurationExtensions
dotnet add package Microsoft.Extensions.DependencyInjection.Abstractions
dotnet add package Microsoft.Extensions.Configuration.Abstractions

🧩 Configuration

Add a Kafka section in your appsettings.json (or environment variables):

{
  "Kafka": {
    "BootstrapServers": "localhost:9092",
    "GroupId": "convex-app",
    "EnableAutoCommit": false,
    "AutoOffsetReset": "Earliest",
    "MessageTimeoutMs": 5000,
    "RequestTimeoutMs": 3000,
    "SessionTimeoutMs": 10000,
    "Producer": {
      "Acks": "all"
    },
    "Consumer": {
      "EnableAutoCommit": false
    }
  }
}

🚀 Usage in a Consumer Project
1️⃣ Register Services in Program.cs
using Convex.Shared.Messaging.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Kafka messaging services
builder.Services.AddKafkaMessaging(builder.Configuration);

var app = builder.Build();
app.Run();


That’s all you need — the library automatically:

Binds configuration from the "Kafka" section to KafkaConfig

Registers generic Kafka producers/consumers:

IKafkaProducer<T>

IKafkaProducer<TKey, TValue>

IKafkaConsumer<T>

IKafkaConsumer<TKey, TValue>

2️⃣ Inject and Use Producers / Consumers

Example: Producing messages

public class UserCreatedProducer
{
    private readonly IKafkaProducer<string, UserCreatedEvent> _producer;

    public UserCreatedProducer(IKafkaProducer<string, UserCreatedEvent> producer)
    {
        _producer = producer;
    }

    public async Task PublishAsync(UserCreatedEvent evt)
    {
        await _producer.ProduceAsync("user-created-topic", "user-key", evt);
    }
}


Example: Consuming messages

public class UserCreatedConsumer : IKafkaConsumer<string, UserCreatedEvent>
{
    public Task ConsumeAsync(string key, UserCreatedEvent message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Received event for user {message.UserId}");
        return Task.CompletedTask;
    }
}

🧠 Advanced: Custom Message Serializer

You can register a custom serializer per type:

builder.Services.AddMessageSerializer<MyCustomMessage>(sp =>
    new JsonMessageSerializer<MyCustomMessage>());


Then your Kafka producer/consumer will automatically use it.