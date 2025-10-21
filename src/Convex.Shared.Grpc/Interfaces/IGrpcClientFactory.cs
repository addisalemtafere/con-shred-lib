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

    /// <summary>
    /// Create a gRPC client with API key authentication
    /// </summary>
    /// <typeparam name="TClient">The gRPC client type</typeparam>
    /// <param name="serviceName">The service name for discovery</param>
    /// <param name="apiKey">API key for authentication</param>
    /// <returns>Configured gRPC client with API key authentication</returns>
    TClient CreateClientWithApiKey<TClient>(string serviceName, string apiKey) where TClient : ClientBase<TClient>;

    /// <summary>
    /// Create a gRPC client with API key authentication for specific endpoint
    /// </summary>
    /// <typeparam name="TClient">The gRPC client type</typeparam>
    /// <param name="endpoint">The service endpoint</param>
    /// <param name="apiKey">API key for authentication</param>
    /// <returns>Configured gRPC client with API key authentication</returns>
    TClient CreateClientWithApiKey<TClient>(Uri endpoint, string apiKey) where TClient : ClientBase<TClient>;

    /// <summary>
    /// Create a gRPC client with authentication headers
    /// </summary>
    /// <typeparam name="TClient">The gRPC client type</typeparam>
    /// <param name="serviceName">The service name for discovery</param>
    /// <param name="authToken">Authentication token</param>
    /// <returns>Configured gRPC client with authentication</returns>
    TClient CreateClientWithAuth<TClient>(string serviceName, string authToken) where TClient : ClientBase<TClient>;

    /// <summary>
    /// Create a gRPC client with authentication headers for specific endpoint
    /// </summary>
    /// <typeparam name="TClient">The gRPC client type</typeparam>
    /// <param name="endpoint">The service endpoint</param>
    /// <param name="authToken">Authentication token</param>
    /// <returns>Configured gRPC client with authentication</returns>
    TClient CreateClientWithAuth<TClient>(Uri endpoint, string authToken) where TClient : ClientBase<TClient>;
}