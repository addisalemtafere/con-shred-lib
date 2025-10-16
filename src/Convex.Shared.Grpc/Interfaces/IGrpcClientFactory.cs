using Grpc.Core;
using Grpc.Net.Client;

namespace Convex.Shared.Grpc.Interfaces;

/// <summary>
/// Factory for creating gRPC clients with built-in connection management
/// </summary>
public interface IGrpcClientFactory
{
    /// <summary>
    /// Create a gRPC client for the specified service
    /// </summary>
    /// <typeparam name="TClient">The gRPC client type</typeparam>
    /// <param name="serviceName">The service name for discovery</param>
    /// <returns>Configured gRPC client</returns>
    TClient CreateClient<TClient>(string serviceName) where TClient : ClientBase<TClient>;

    /// <summary>
    /// Create a gRPC client with custom channel options
    /// </summary>
    /// <typeparam name="TClient">The gRPC client type</typeparam>
    /// <param name="serviceName">The service name for discovery</param>
    /// <param name="options">Custom channel options</param>
    /// <returns>Configured gRPC client</returns>
    TClient CreateClient<TClient>(string serviceName, GrpcChannelOptions options) where TClient : ClientBase<TClient>;

    /// <summary>
    /// Create a gRPC client with specific endpoint
    /// </summary>
    /// <typeparam name="TClient">The gRPC client type</typeparam>
    /// <param name="endpoint">The service endpoint</param>
    /// <returns>Configured gRPC client</returns>
    TClient CreateClient<TClient>(Uri endpoint) where TClient : ClientBase<TClient>;
}
