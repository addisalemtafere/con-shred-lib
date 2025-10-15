namespace Convex.Shared.Messaging.Configuration;

/// <summary>
/// Configuration options for Convex messaging services
/// </summary>
public class ConvexMessagingOptions
{
    /// <summary>
    /// Kafka bootstrap servers
    /// </summary>
    public string BootstrapServers { get; set; } = "localhost:9092";

    /// <summary>
    /// Kafka security protocol
    /// </summary>
    public string SecurityProtocol { get; set; } = "PLAINTEXT";

    /// <summary>
    /// Kafka SASL mechanism
    /// </summary>
    public string SaslMechanism { get; set; } = "PLAIN";

    /// <summary>
    /// Kafka SASL username
    /// </summary>
    public string SaslUsername { get; set; } = string.Empty;

    /// <summary>
    /// Kafka SASL password
    /// </summary>
    public string SaslPassword { get; set; } = string.Empty;

    /// <summary>
    /// Kafka SSL CA location
    /// </summary>
    public string SslCaLocation { get; set; } = string.Empty;

    /// <summary>
    /// Default topic prefix
    /// </summary>
    public string TopicPrefix { get; set; } = "convex";

    /// <summary>
    /// Default consumer group
    /// </summary>
    public string ConsumerGroup { get; set; } = "convex-group";

    /// <summary>
    /// Auto offset reset policy
    /// </summary>
    public string AutoOffsetReset { get; set; } = "earliest";

    /// <summary>
    /// Enable auto commit
    /// </summary>
    public bool EnableAutoCommit { get; set; } = true;

    /// <summary>
    /// Auto commit interval in milliseconds
    /// </summary>
    public int AutoCommitIntervalMs { get; set; } = 5000;

    /// <summary>
    /// Session timeout in milliseconds
    /// </summary>
    public int SessionTimeoutMs { get; set; } = 30000;

    /// <summary>
    /// Request timeout in milliseconds
    /// </summary>
    public int RequestTimeoutMs { get; set; } = 30000;

    /// <summary>
    /// Maximum retry attempts
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Retry delay in seconds
    /// </summary>
    public int RetryDelaySeconds { get; set; } = 5;

    /// <summary>
    /// Message retention in milliseconds
    /// </summary>
    public long MessageRetentionMs { get; set; } = 604800000; // 7 days
}
