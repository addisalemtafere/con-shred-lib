using Convex.Shared.Grpc.Configuration;
using Convex.Shared.Grpc.Interfaces;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Convex.Shared.Grpc.Services;

/// <summary>
/// High-performance gRPC client factory with connection pooling and service discovery
/// </summary>
public class ConvexGrpcClientFactory : IGrpcClientFactory, IDisposable
{
    private readonly ILogger<ConvexGrpcClientFactory> _logger;
    private readonly ConvexGrpcOptions _options;
    private readonly Dictionary<string, GrpcChannel> _channels = new();
    private readonly object _lock = new();
    private bool _disposed = false;

    public ConvexGrpcClientFactory(
        ILogger<ConvexGrpcClientFactory> logger,
        IOptions<ConvexGrpcOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public TClient CreateClient<TClient>(string serviceName) where TClient : ClientBase<TClient>
    {
        var endpoint = ResolveServiceEndpoint(serviceName);
        return CreateClient<TClient>(endpoint);
    }

    public TClient CreateClient<TClient>(string serviceName, GrpcChannelOptions options) where TClient : ClientBase<TClient>
    {
        var endpoint = ResolveServiceEndpoint(serviceName);
        return CreateClientWithOptions<TClient>(endpoint, options);
    }

    public TClient CreateClient<TClient>(Uri endpoint) where TClient : ClientBase<TClient>
    {
        var channel = GetOrCreateChannel(endpoint);
        return (TClient)Activator.CreateInstance(typeof(TClient), channel)!;
    }

    private TClient CreateClientWithOptions<TClient>(Uri endpoint, GrpcChannelOptions options) where TClient : ClientBase<TClient>
    {
        var channel = GrpcChannel.ForAddress(endpoint, options);
        return (TClient)Activator.CreateInstance(typeof(TClient), channel)!;
    }

    private GrpcChannel GetOrCreateChannel(Uri endpoint)
    {
        var key = endpoint.ToString();

        lock (_lock)
        {
            if (_channels.TryGetValue(key, out var existingChannel) && existingChannel.State != ConnectivityState.Shutdown)
            {
                return existingChannel;
            }

            var channelOptions = new GrpcChannelOptions
            {
                MaxReceiveMessageSize = _options.MaxReceiveMessageSize,
                MaxSendMessageSize = _options.MaxSendMessageSize,
                Credentials = _options.EnableTls ? ChannelCredentials.SecureSsl : ChannelCredentials.Insecure
            };

            var channel = GrpcChannel.ForAddress(endpoint, channelOptions);
            _channels[key] = channel;

            _logger.LogInformation("Created gRPC channel for {Endpoint}", endpoint);
            return channel;
        }
    }

    private Uri ResolveServiceEndpoint(string serviceName)
    {
        // First try to get from configuration
        var serviceConfig = _options.Services.FirstOrDefault(s => s.Name == serviceName);
        if (serviceConfig != null)
        {
            return new Uri(serviceConfig.Endpoint);
        }

        // Fallback to default service discovery
        var serviceEndpoint = GetDefaultServiceEndpoint(serviceName);
        if (serviceEndpoint != null)
        {
            return new Uri(serviceEndpoint);
        }

        throw new InvalidOperationException($"Service '{serviceName}' not found. Available services: {string.Join(", ", _options.Services.Select(s => s.Name))}");
    }

    private string? GetDefaultServiceEndpoint(string serviceName)
    {
        // Default service endpoints for development
        return serviceName switch
        {
            "UserService" => "http://localhost:50052",
            "PaymentService" => "http://localhost:50054",
            "BettingService" => "http://localhost:50053",
            "GameService" => "http://localhost:50055",
            "NotificationService" => "http://localhost:50056",
            "AdminService" => "http://localhost:50057",
            "AuthService" => "http://localhost:50051",
            _ => null
        };
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            lock (_lock)
            {
                foreach (var channel in _channels.Values)
                {
                    try
                    {
                        channel.ShutdownAsync().Wait(TimeSpan.FromSeconds(5));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error shutting down gRPC channel");
                    }
                }
                _channels.Clear();
            }
            _disposed = true;
        }
    }
}