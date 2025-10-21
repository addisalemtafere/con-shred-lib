using Grpc.Core;

namespace Convex.Shared.Grpc.Models;

/// <summary>
/// Simple API key authentication for gRPC services
/// </summary>
public static class SimpleApiKeyAuthentication
{
    /// <summary>
    /// API key header name for gRPC metadata
    /// </summary>
    public const string ApiKeyHeaderName = "x-api-key";

    /// <summary>
    /// Authorization header name for gRPC metadata
    /// </summary>
    public const string AuthorizationHeaderName = "authorization";

    /// <summary>
    /// Create API key metadata for gRPC calls
    /// </summary>
    /// <param name="apiKey">The API key to include</param>
    /// <returns>Metadata with API key</returns>
    public static Metadata CreateApiKeyMetadata(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API key cannot be null or empty", nameof(apiKey));

        var metadata = new Metadata();
        metadata.Add(ApiKeyHeaderName, apiKey);
        return metadata;
    }

    /// <summary>
    /// Create authorization metadata for gRPC calls
    /// </summary>
    /// <param name="apiKey">The API key to include</param>
    /// <returns>Metadata with authorization header</returns>
    public static Metadata CreateAuthorizationMetadata(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API key cannot be null or empty", nameof(apiKey));

        var metadata = new Metadata();
        metadata.Add(AuthorizationHeaderName, $"Bearer {apiKey}");
        return metadata;
    }

    /// <summary>
    /// Extract API key from gRPC metadata
    /// </summary>
    /// <param name="metadata">The gRPC metadata</param>
    /// <returns>The API key if found, null otherwise</returns>
    public static string? ExtractApiKey(Metadata metadata)
    {
        if (metadata == null)
            return null;

        // Try API key header first
        var apiKeyEntry = metadata.FirstOrDefault(m => m.Key.Equals(ApiKeyHeaderName, StringComparison.OrdinalIgnoreCase));
        if (apiKeyEntry != null)
            return apiKeyEntry.Value;

        // Try authorization header
        var authEntry = metadata.FirstOrDefault(m => m.Key.Equals(AuthorizationHeaderName, StringComparison.OrdinalIgnoreCase));
        if (authEntry != null && authEntry.Value.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return authEntry.Value.Substring(7); // Remove "Bearer " prefix

        return null;
    }

    /// <summary>
    /// Validate API key against list of valid keys
    /// </summary>
    /// <param name="apiKey">The API key to validate</param>
    /// <param name="validApiKeys">List of valid API keys</param>
    /// <returns>True if API key is valid, false otherwise</returns>
    public static bool ValidateApiKey(string? apiKey, List<string> validApiKeys)
    {
        if (string.IsNullOrWhiteSpace(apiKey) || validApiKeys == null || validApiKeys.Count == 0)
            return false;

        return validApiKeys.Contains(apiKey, StringComparer.Ordinal);
    }
}
