namespace Convex.Shared.Grpc.Configuration;

/// <summary>
/// Configuration options for Convex gRPC services
/// </summary>
public class ConvexGrpcOptions
{
    /// <summary>
    /// Maximum message size for receiving (in bytes)
    /// </summary>
    public int MaxReceiveMessageSize { get; set; } = 4 * 1024 * 1024; // 4MB

    /// <summary>
    /// Maximum message size for sending (in bytes)
    /// </summary>
    public int MaxSendMessageSize { get; set; } = 4 * 1024 * 1024; // 4MB

    /// <summary>
    /// Maximum number of retry attempts
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Initial backoff delay in milliseconds
    /// </summary>
    public int InitialBackoffMs { get; set; } = 100;

    /// <summary>
    /// Maximum backoff delay in milliseconds
    /// </summary>
    public int MaxBackoffMs { get; set; } = 5000;

    /// <summary>
    /// Backoff multiplier for exponential backoff
    /// </summary>
    public double BackoffMultiplier { get; set; } = 1.5;

    /// <summary>
    /// Connection timeout in seconds
    /// </summary>
    public int ConnectionTimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Keep-alive ping interval in seconds
    /// </summary>
    public int KeepAlivePingIntervalSeconds { get; set; } = 30;

    /// <summary>
    /// Keep-alive timeout in seconds
    /// </summary>
    public int KeepAliveTimeoutSeconds { get; set; } = 5;

    /// <summary>
    /// Enable compression
    /// </summary>
    public bool EnableCompression { get; set; } = true;

    /// <summary>
    /// Enable TLS encryption
    /// </summary>
    public bool EnableTls { get; set; } = false;

    /// <summary>
    /// Service discovery configuration
    /// </summary>
    public List<GrpcServiceConfig> Services { get; set; } = new();

    /// <summary>
    /// Server configuration
    /// </summary>
    public string? ServiceName { get; set; }

    public int ServerPort { get; set; } = 50051;
    public bool EnableServer { get; set; } = true;
}

/// <summary>
/// Configuration for individual gRPC services
/// </summary>
public class GrpcServiceConfig
{
    /// <summary>
    /// Service name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Service endpoint
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Service port
    /// </summary>
    public int Port { get; set; } = 50051;

    /// <summary>
    /// Enable health checks
    /// </summary>
    public bool EnableHealthChecks { get; set; } = true;

    /// <summary>
    /// Service tags for discovery
    /// </summary>
    public List<string> Tags { get; set; } = new();
}