using KafkaFlow;
using KafkaFlow.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Convex.Shared.Infrastructure.Messaging;
using Convex.Shared.Infrastructure.Services;

namespace Convex.Shared.Infrastructure.Extensions;

/// <summary>
/// Kafka service collection extensions for generic message processing
/// </summary>
public static class KafkaServiceCollectionExtensions
{
    /// <summary>
    /// Add KafkaFlow consumer services
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    /// <returns>Service collection</returns>
    /// <exception cref="ArgumentNullException">Thrown when services or configuration is null</exception>
    public static IServiceCollection AddKafkaFlowConsumers(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var kafkaConfig = GetKafkaConfiguration(configuration);
        services.AddSingleton(kafkaConfig);
        
        // Register KafkaFlow with proper error handling and observability
        services.AddKafka(kafka => ConfigureKafkaCluster(kafka, kafkaConfig));
        
        return services;
    }

    /// <summary>
    /// Add KafkaFlow producer services
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddKafkaFlowProducers(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var kafkaConfig = GetKafkaConfiguration(configuration);
        services.AddSingleton(kafkaConfig);
        
        // Add KafkaFlow with producer
        services.AddKafka(kafka => kafka
            .UseConsoleLog()
            .AddCluster(cluster => cluster
                .WithBrokers(kafkaConfig.BootstrapServers)
                .AddProducer(producer => producer
                    .DefaultTopic(kafkaConfig.DefaultTopic)
                    .AddMiddlewares(middlewares => middlewares
                        .AddSerializer<JsonCoreSerializer>()
                    )
                )
            )
        );
        
        return services;
    }

    /// <summary>
    /// Add KafkaFlow background services
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddKafkaFlowBackgroundServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        
        services.AddHostedService<KafkaFlowBackgroundService>();
        return services;
    }

    /// <summary>
    /// Gets Kafka configuration with proper validation and defaults.
    /// Single Responsibility: Only handles configuration retrieval and validation.
    /// </summary>
    /// <param name="configuration">Configuration source</param>
    /// <returns>Validated Kafka configuration</returns>
    private static KafkaFlowConfiguration GetKafkaConfiguration(IConfiguration configuration)
    {
        var kafkaConfig = configuration.GetSection("Kafka").Get<KafkaFlowConfiguration>() ?? new KafkaFlowConfiguration();
        
        // Validate required configuration
        if (string.IsNullOrWhiteSpace(kafkaConfig.BootstrapServers))
        {
            throw new InvalidOperationException("Kafka BootstrapServers configuration is required");
        }

        if (string.IsNullOrWhiteSpace(kafkaConfig.GroupId))
        {
            throw new InvalidOperationException("Kafka GroupId configuration is required");
        }

        return kafkaConfig;
    }

    /// <summary>
    /// Configures Kafka cluster with proper error handling and observability.
    /// Single Responsibility: Only handles cluster configuration.
    /// Open/Closed: Extensible through configuration parameters.
    /// </summary>
    /// <param name="kafka">Kafka configuration builder</param>
    /// <param name="config">Kafka configuration</param>
    private static void ConfigureKafkaCluster(IKafkaConfigurationBuilder kafka, KafkaFlowConfiguration config)
    {
        kafka.UseConsoleLog()
            .AddCluster(cluster => cluster
                .WithBrokers(config.BootstrapServers)
                .AddConsumer(consumer => ConfigureConsumer(consumer, config))
            );
    }

    /// <summary>
    /// Configures Kafka consumer with middleware and error handling.
    /// Single Responsibility: Only handles consumer configuration.
    /// </summary>
    /// <param name="consumer">Consumer configuration builder</param>
    /// <param name="config">Kafka configuration</param>
    private static void ConfigureConsumer(IConsumerConfigurationBuilder consumer, KafkaFlowConfiguration config)
    {
        consumer.Topic(config.DefaultTopic)
            .WithGroupId(config.GroupId)
            .WithBufferSize(config.BufferSize)
            .WithWorkersCount(config.WorkersCount)
            .WithAutoOffsetReset(config.AutoOffsetReset)
            .AddMiddlewares(middlewares => middlewares
                .AddSerializer<JsonCoreSerializer>()
                .Add<GenericMessageHandlerMiddleware>()
            );
    }
}

/// <summary>
/// KafkaFlow configuration following SOLID principles.
/// Single Responsibility: Only handles Kafka configuration.
/// Open/Closed: Extensible through additional properties.
/// Interface Segregation: Implements only necessary configuration.
/// </summary>
public sealed class KafkaFlowConfiguration
{
    /// <summary>
    /// Kafka bootstrap servers (required)
    /// </summary>
    public string BootstrapServers { get; set; } = "localhost:9092";

    /// <summary>
    /// Consumer group ID (required)
    /// </summary>
    public string GroupId { get; set; } = "application-group";

    /// <summary>
    /// Default topic for messages
    /// </summary>
    public string DefaultTopic { get; set; } = "default-topic";

    /// <summary>
    /// Buffer size for message processing (SOAR: Scalable)
    /// </summary>
    public int BufferSize { get; set; } = 100;

    /// <summary>
    /// Number of worker threads (SOAR: Scalable)
    /// </summary>
    public int WorkersCount { get; set; } = 1;

    /// <summary>
    /// Auto offset reset strategy (SOAR: Reliable)
    /// </summary>
    public AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Earliest;

    /// <summary>
    /// Security protocol (SOAR: Available, Reliable)
    /// </summary>
    public string SecurityProtocol { get; set; } = "PLAINTEXT";

    /// <summary>
    /// SASL mechanism for authentication
    /// </summary>
    public string SaslMechanism { get; set; } = "PLAIN";

    /// <summary>
    /// SASL username for authentication
    /// </summary>
    public string SaslUsername { get; set; } = string.Empty;

    /// <summary>
    /// SASL password for authentication
    /// </summary>
    public string SaslPassword { get; set; } = string.Empty;

    /// <summary>
    /// Session timeout in milliseconds (SOAR: Reliable)
    /// </summary>
    public int SessionTimeoutMs { get; set; } = 30000;

    /// <summary>
    /// Maximum poll interval in milliseconds (SOAR: Reliable)
    /// </summary>
    public int MaxPollIntervalMs { get; set; } = 300000;

    /// <summary>
    /// Enable auto commit (SOAR: Reliable)
    /// </summary>
    public bool EnableAutoCommit { get; set; } = false;

    /// <summary>
    /// Retry configuration for failed messages (SOAR: Reliable)
    /// </summary>
    public RetryConfiguration RetryConfiguration { get; set; } = new();

    /// <summary>
    /// Observability configuration (SOAR: Observable)
    /// </summary>
    public ObservabilityConfiguration Observability { get; set; } = new();
}

/// <summary>
/// Retry configuration for message processing failures (SOAR: Reliable)
/// </summary>
public sealed class RetryConfiguration
{
    /// <summary>
    /// Maximum number of retry attempts
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Base delay between retries in milliseconds
    /// </summary>
    public int BaseDelayMs { get; set; } = 1000;

    /// <summary>
    /// Maximum delay between retries in milliseconds
    /// </summary>
    public int MaxDelayMs { get; set; } = 30000;

    /// <summary>
    /// Exponential backoff multiplier
    /// </summary>
    public double BackoffMultiplier { get; set; } = 2.0;
}

/// <summary>
/// Observability configuration (SOAR: Observable)
/// </summary>
public sealed class ObservabilityConfiguration
{
    /// <summary>
    /// Enable detailed logging
    /// </summary>
    public bool EnableDetailedLogging { get; set; } = true;

    /// <summary>
    /// Enable metrics collection
    /// </summary>
    public bool EnableMetrics { get; set; } = true;

    /// <summary>
    /// Enable health checks
    /// </summary>
    public bool EnableHealthChecks { get; set; } = true;

    /// <summary>
    /// Log level for Kafka operations
    /// </summary>
    public string LogLevel { get; set; } = "Information";
}
