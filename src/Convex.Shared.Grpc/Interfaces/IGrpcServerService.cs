namespace Convex.Shared.Grpc.Interfaces;

/// <summary>
/// Base interface for gRPC server services
/// </summary>
public interface IGrpcServerService
{
    /// <summary>
    /// Service name for registration
    /// </summary>
    string ServiceName { get; }

    /// <summary>
    /// Service port
    /// </summary>
    int Port { get; }

    /// <summary>
    /// Service description
    /// </summary>
    string Description { get; }
}