using Convex.Shared.Grpc.Configuration;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Convex.Shared.Grpc.Services;

/// <summary>
/// Simple gRPC client factory with client-side load balancing for Kubernetes
/// </summary>
public class ConvexGrpcLoadBalancedClientFactory : IDisposable
{
    private readonly ILogger<ConvexGrpcLoadBalancedClientFactory> _logger;
    private readonly ConvexGrpcOptions _options;
    private readonly ConvexGrpcLoadBalancer _loadBalancer;
    private readonly ConcurrentDictionary<string, GrpcChannel> _channels = new();
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of the ConvexGrpcLoadBalancedClientFactory
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="options">gRPC configuration options</param>
    /// <param name="loadBalancer">Load balancer instance</param>
    public ConvexGrpcLoadBalancedClientFactory(
        ILogger<ConvexGrpcLoadBalancedClientFactory> logger,
        IOptions<ConvexGrpcOptions> options,
        ConvexGrpcLoadBalancer loadBalancer)
    {
        _logger = logger;
        _options = options.Value;
        _loadBalancer = loadBalancer;
        _logger.LogInformation("ConvexGrpcLoadBalancedClientFactory initialized");
    }

    /// <summary>
    /// Create a load-balanced gRPC client
    /// </summary>
    public TClient CreateClient<TClient>(string serviceName) where TClient : class
    {
        var endpoint = _loadBalancer.GetLoadBalancedEndpoint(serviceName);
        if (endpoint == null)
        {
            throw new InvalidOperationException($"No healthy endpoints available for service {serviceName}");
        }

        var channel = GetOrCreateChannel(endpoint.Endpoint);
        var client = CreateClientInstance<TClient>(channel);
        
        _logger.LogDebug("Created load-balanced client for service {ServiceName}", serviceName);
        return client;
    }

    /// <summary>
    /// Create a client for specific endpoint
    /// </summary>
    public TClient CreateClientForEndpoint<TClient>(string endpoint) where TClient : class
    {
        var channel = GetOrCreateChannel(endpoint);
        var client = CreateClientInstance<TClient>(channel);
        return client;
    }

    private GrpcChannel GetOrCreateChannel(string endpoint)
    {
        return _channels.GetOrAdd(endpoint, CreateChannel);
    }

    private GrpcChannel CreateChannel(string endpoint)
    {
        var channelOptions = new GrpcChannelOptions
        {
            MaxReceiveMessageSize = _options.MaxReceiveMessageSize,
            MaxSendMessageSize = _options.MaxSendMessageSize,
            Credentials = _options.EnableTls ? ChannelCredentials.SecureSsl : ChannelCredentials.Insecure
        };

        return GrpcChannel.ForAddress(endpoint, channelOptions);
    }

    private TClient CreateClientInstance<TClient>(GrpcChannel channel) where TClient : class
    {
        var clientType = typeof(TClient);
        var constructor = clientType.GetConstructor(new[] { typeof(GrpcChannel) });
        
        if (constructor == null)
        {
            throw new InvalidOperationException($"gRPC client {clientType.Name} must have a constructor that takes GrpcChannel");
        }

        return (TClient)constructor.Invoke(new object[] { channel });
    }

    /// <summary>
    /// Dispose the client factory
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            
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
            _logger.LogInformation("ConvexGrpcLoadBalancedClientFactory disposed");
        }
    }
}