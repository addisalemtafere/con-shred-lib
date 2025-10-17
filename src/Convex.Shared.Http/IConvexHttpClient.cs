namespace Convex.Shared.Http;

/// <summary>
/// Professional HTTP client interface for Convex microservices
/// </summary>
public interface IConvexHttpClient
{
    /// <summary>
    /// Sends a GET request to the specified URI
    /// </summary>
    /// <typeparam name="TResponse">The type of the response</typeparam>
    /// <param name="uri">The request URI</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The deserialized response</returns>
    Task<TResponse?> GetAsync<TResponse>(string uri, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a POST request to the specified URI with the provided content
    /// </summary>
    /// <typeparam name="TRequest">The type of the request content</typeparam>
    /// <typeparam name="TResponse">The type of the response</typeparam>
    /// <param name="uri">The request URI</param>
    /// <param name="content">The request content</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The deserialized response</returns>
    Task<TResponse?> PostAsync<TRequest, TResponse>(string uri, TRequest content, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a PUT request to the specified URI with the provided content
    /// </summary>
    /// <typeparam name="TRequest">The type of the request content</typeparam>
    /// <typeparam name="TResponse">The type of the response</typeparam>
    /// <param name="uri">The request URI</param>
    /// <param name="content">The request content</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The deserialized response</returns>
    Task<TResponse?> PutAsync<TRequest, TResponse>(string uri, TRequest content, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a DELETE request to the specified URI
    /// </summary>
    /// <param name="uri">The request URI</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> DeleteAsync(string uri, CancellationToken cancellationToken = default);
}