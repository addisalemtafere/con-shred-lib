using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Convex.Shared.Http.Services;

/// <summary>
/// Service for managing authentication tokens with automatic refresh
/// </summary>
public interface ITokenManagementService
{
    /// <summary>
    /// Gets a valid access token, refreshing if necessary
    /// </summary>
    Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Clears the cached token
    /// </summary>
    void ClearToken();
}

/// <summary>
/// Implementation of token management with caching and automatic refresh
/// </summary>
public class TokenManagementService : ITokenManagementService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly ILogger<TokenManagementService> _logger;
    private readonly ConvexHttpClientOptions _options;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private const string CacheKey = "access_token";

    public TokenManagementService(
        HttpClient httpClient,
        IMemoryCache cache,
        ILogger<TokenManagementService> logger,
        IOptions<ConvexHttpClientOptions> options)
    {
        _httpClient = httpClient;
        _cache = cache;
        _logger = logger;
        _options = options.Value;
    }

    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        if (!_options.Authentication.EnableTokenManagement)
        {
            throw new InvalidOperationException("Token management is not enabled");
        }

        // Check cache first
        if (_cache.TryGetValue(CacheKey, out CachedToken? cachedToken) && 
            cachedToken != null && 
            cachedToken.ExpiresAt > DateTime.UtcNow.Add(_options.Authentication.TokenRefreshBuffer))
        {
            return cachedToken.AccessToken;
        }

        // Use semaphore to prevent multiple concurrent token requests
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            // Double-check cache after acquiring lock
            if (_cache.TryGetValue(CacheKey, out cachedToken) && 
                cachedToken != null && 
                cachedToken.ExpiresAt > DateTime.UtcNow.Add(_options.Authentication.TokenRefreshBuffer))
            {
                return cachedToken.AccessToken;
            }

            // Request new token
            var token = await RequestNewTokenAsync(cancellationToken);
            
            // Cache the token
            var cacheEntry = new CachedToken
            {
                AccessToken = token.AccessToken,
                ExpiresAt = DateTime.UtcNow.Add(TimeSpan.FromSeconds(token.ExpiresIn))
            };

            _cache.Set(CacheKey, cacheEntry, _options.Authentication.TokenCacheDuration);

            _logger.LogDebug("New access token obtained and cached");
            return token.AccessToken;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void ClearToken()
    {
        _cache.Remove(CacheKey);
        _logger.LogDebug("Access token cache cleared");
    }

    private async Task<TokenResponse> RequestNewTokenAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_options.Authentication.TokenEndpoint))
            throw new InvalidOperationException("Token endpoint is not configured");

        var request = new HttpRequestMessage(HttpMethod.Post, _options.Authentication.TokenEndpoint);
        
        // Prepare form data
        var formData = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "client_credentials"),
            new("client_id", _options.Authentication.ClientId ?? throw new InvalidOperationException("Client ID is required")),
            new("client_secret", _options.Authentication.ClientSecret ?? throw new InvalidOperationException("Client secret is required"))
        };

        // Add additional parameters
        foreach (var param in _options.Authentication.AdditionalParameters)
        {
            formData.Add(new KeyValuePair<string, string>(param.Key, param.Value));
        }

        request.Content = new FormUrlEncodedContent(formData);

        try
        {
            _logger.LogDebug("Requesting new access token from {TokenEndpoint}", _options.Authentication.TokenEndpoint);
            
            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (tokenResponse?.AccessToken == null)
                throw new InvalidOperationException("Invalid token response received");

            _logger.LogInformation("Successfully obtained new access token");
            return tokenResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to obtain access token from {TokenEndpoint}", _options.Authentication.TokenEndpoint);
            throw;
        }
    }
}

/// <summary>
/// Cached token information
/// </summary>
internal class CachedToken
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}

/// <summary>
/// Token response from authentication server
/// </summary>
internal class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
    public string? TokenType { get; set; }
    public string? Scope { get; set; }
}
