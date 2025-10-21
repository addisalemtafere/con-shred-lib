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
    /// Enable TLS encryption (disabled for local development, enabled for production)
    /// </summary>
    public bool EnableTls { get; set; } = false;

    /// <summary>
    /// Enable authentication for gRPC calls (disabled for local development, enabled for production)
    /// </summary>
    public bool EnableAuthentication { get; set; } = false;

    /// <summary>
    /// JWT token for authentication (used when EnableAuthentication is true)
    /// </summary>
    public string? JwtToken { get; set; }

    /// <summary>
    /// API key for service-to-service authentication
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Service-specific API key for this service (used when calling other services)
    /// </summary>
    public string? ServiceApiKey { get; set; }

    /// <summary>
    /// Valid API keys for incoming requests (used to validate other services calling this service)
    /// </summary>
    public List<string> ValidApiKeys { get; set; } = new();

    /// <summary>
    /// Service discovery configuration
    /// </summary>
    public List<GrpcServiceConfig> Services { get; set; } = new();

    /// <summary>
    /// Server configuration
    /// </summary>
    public string? ServiceName { get; set; }

    /// <summary>
    /// Server port number for gRPC services (auto-discovered if not set)
    /// </summary>
    public int ServerPort { get; set; } = 0; // 0 = auto-discover
    
    /// <summary>
    /// Enable or disable the gRPC server
    /// </summary>
    public bool EnableServer { get; set; } = true;

    /// <summary>
    /// Maximum number of concurrent connections per service
    /// </summary>
    public int MaxConcurrentConnections { get; set; } = 100;

    /// <summary>
    /// Connection pool size for high-performance scenarios
    /// </summary>
    public int ConnectionPoolSize { get; set; } = 10;

    /// <summary>
    /// Enable connection pooling for better performance
    /// </summary>
    public bool EnableConnectionPooling { get; set; } = true;

    /// <summary>
    /// Enable load balancing (handled by external load balancer)
    /// </summary>
    public bool EnableLoadBalancing { get; set; } = false;

    /// <summary>
    /// Circuit breaker threshold for fault tolerance
    /// </summary>
    public int CircuitBreakerThreshold { get; set; } = 5;

    /// <summary>
    /// Circuit breaker timeout in seconds
    /// </summary>
    public int CircuitBreakerTimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Enable metrics collection for monitoring
    /// </summary>
    public bool EnableMetrics { get; set; } = true;

    /// <summary>
    /// Enable distributed tracing
    /// </summary>
    public bool EnableDistributedTracing { get; set; } = true;

    /// <summary>
    /// Request timeout in seconds
    /// </summary>
    public int RequestTimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Enable request/response caching
    /// </summary>
    public bool EnableCaching { get; set; } = false;

    /// <summary>
    /// Cache TTL in seconds
    /// </summary>
    public int CacheTtlSeconds { get; set; } = 300;

    // Kubernetes integration
    /// <summary>
    /// Enable service discovery for Kubernetes
    /// </summary>
    public bool EnableServiceDiscovery { get; set; } = true;

    /// <summary>
    /// Enable client-side load balancing (solves Kubernetes L4 load balancing issue)
    /// </summary>
    public bool EnableClientSideLoadBalancing { get; set; } = true;
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

    /// <summary>
    /// API key for this specific service (used when other services call this service)
    /// </summary>
    public string? ServiceApiKey { get; set; }
}