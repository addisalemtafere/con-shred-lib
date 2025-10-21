using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using Convex.Shared.Http.Policies;
using Convex.Shared.Http.Services;
using Polly;

namespace Convex.Shared.Http;

/// <summary>
/// Professional HTTP client implementation for Convex microservices
/// </summary>
public class ConvexHttpClient : IConvexHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ConvexHttpClient> _logger;
    private readonly ConvexHttpClientOptions _options;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IAsyncPolicy<HttpResponseMessage> _resiliencePolicy;
    private readonly ITokenManagementService? _tokenService;

    public ConvexHttpClient(
        HttpClient httpClient,
        ILogger<ConvexHttpClient> logger,
        IOptions<ConvexHttpClientOptions> options,
        ITokenManagementService? tokenService = null)
    {
        _httpClient = httpClient;
        _logger = logger;
        _options = options.Value;
        _tokenService = tokenService;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
        _resiliencePolicy = ResiliencePolicyBuilder.CreateResiliencePolicy(_options, _logger);

        ConfigureHttpClient();
    }

    public async Task<TResponse?> GetAsync<TResponse>(string uri, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uri))
            throw new ArgumentException("URI cannot be null or empty", nameof(uri));

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var requestId = Guid.NewGuid().ToString("N")[..8];

        try
        {
            if (_options.EnableDebugLogging)
            {
                _logger.LogDebug("[{RequestId}] Starting GET request to {Uri}", requestId, uri);
            }

            await SetAuthenticationHeaderAsync();

            var response = await _resiliencePolicy.ExecuteAsync(async (ct) => 
                await _httpClient.GetAsync(uri, ct), cancellationToken);
            
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<TResponse>(content, _jsonOptions);

            stopwatch.Stop();

            if (_options.EnableLogging)
            {
                _logger.LogInformation("[{RequestId}] GET {Uri} completed in {ElapsedMs}ms with status {StatusCode}", 
                    requestId, uri, stopwatch.ElapsedMilliseconds, response.StatusCode);
            }

            if (_options.EnableDebugLogging)
            {
                _logger.LogDebug("[{RequestId}] Response content length: {ContentLength}", 
                    requestId, content.Length);
            }

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "[{RequestId}] GET {Uri} failed after {ElapsedMs}ms", 
                requestId, uri, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string uri, TRequest content, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uri))
            throw new ArgumentException("URI cannot be null or empty", nameof(uri));
        if (content == null)
            throw new ArgumentNullException(nameof(content));

        await SetAuthenticationHeaderAsync();

        var json = JsonSerializer.Serialize(content, _jsonOptions);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _resiliencePolicy.ExecuteAsync(async (ct) => 
            await _httpClient.PostAsync(uri, httpContent, ct), cancellationToken);
        
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string uri, TRequest content, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uri))
            throw new ArgumentException("URI cannot be null or empty", nameof(uri));
        if (content == null)
            throw new ArgumentNullException(nameof(content));

        await SetAuthenticationHeaderAsync();

        var json = JsonSerializer.Serialize(content, _jsonOptions);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _resiliencePolicy.ExecuteAsync(async (ct) => 
            await _httpClient.PutAsync(uri, httpContent, ct), cancellationToken);
        
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
    }

    public async Task<bool> DeleteAsync(string uri, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uri))
            throw new ArgumentException("URI cannot be null or empty", nameof(uri));

        await SetAuthenticationHeaderAsync();

        var response = await _resiliencePolicy.ExecuteAsync(async (ct) => 
            await _httpClient.DeleteAsync(uri, ct), cancellationToken);
        
        return response.IsSuccessStatusCode;
    }

    private void ConfigureHttpClient()
    {
        if (_options.BaseAddress != null)
        {
            _httpClient.BaseAddress = _options.BaseAddress;
        }

        _httpClient.Timeout = _options.Timeout;

        // Add API key if configured
        if (!string.IsNullOrEmpty(_options.ApiKey))
        {
            _httpClient.DefaultRequestHeaders.Add("X-API-Key", _options.ApiKey);
        }

        // Add Bearer token if configured
        if (!string.IsNullOrEmpty(_options.BearerToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _options.BearerToken);
        }

        // Add custom headers
        foreach (var header in _options.DefaultHeaders)
        {
            _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
        }
    }

    private async Task SetAuthenticationHeaderAsync()
    {
        switch (_options.Authentication.Type)
        {
            case AuthenticationTypes.None:
                // No authentication required
                break;

            case AuthenticationTypes.ApiKey:
                if (!string.IsNullOrEmpty(_options.Authentication.ApiKey))
                {
                    _httpClient.DefaultRequestHeaders.Remove(_options.Authentication.ApiKeyHeaderName);
                    _httpClient.DefaultRequestHeaders.Add(_options.Authentication.ApiKeyHeaderName, _options.Authentication.ApiKey);
                }
                break;

            case AuthenticationTypes.BearerToken:
                if (!string.IsNullOrEmpty(_options.Authentication.BearerToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _options.Authentication.BearerToken);
                }
                break;

            case AuthenticationTypes.Basic:
                if (!string.IsNullOrEmpty(_options.Authentication.Username) && !string.IsNullOrEmpty(_options.Authentication.Password))
                {
                    var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_options.Authentication.Username}:{_options.Authentication.Password}"));
                    _httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
                }
                break;

            case AuthenticationTypes.OAuth2ClientCredentials:
                if (_tokenService != null && _options.Authentication.EnableTokenManagement)
                {
                    try
                    {
                        var token = await _tokenService.GetAccessTokenAsync();
                        _httpClient.DefaultRequestHeaders.Authorization = 
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to obtain OAuth2 access token");
                        throw;
                    }
                }
                break;

            case AuthenticationTypes.Custom:
                if (!string.IsNullOrEmpty(_options.Authentication.CustomHeaderName) && !string.IsNullOrEmpty(_options.Authentication.CustomHeaderValue))
                {
                    _httpClient.DefaultRequestHeaders.Remove(_options.Authentication.CustomHeaderName);
                    _httpClient.DefaultRequestHeaders.Add(_options.Authentication.CustomHeaderName, _options.Authentication.CustomHeaderValue);
                }
                break;
        }
    }

    private void LogRequestContent<T>(string requestId, T content, string method, string uri)
    {
        if (_options.EnableContentLogging && content != null)
        {
            try
            {
                var json = JsonSerializer.Serialize(content, _jsonOptions);
                var truncatedContent = json.Length > _options.MaxContentLogLength 
                    ? json[.._options.MaxContentLogLength] + "..." 
                    : json;
                
                _logger.LogDebug("[{RequestId}] {Method} {Uri} content: {Content}", 
                    requestId, method, uri, truncatedContent);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[{RequestId}] Failed to serialize content for logging", requestId);
            }
        }
    }

    private void LogResponseContent(string requestId, string content, string method, string uri)
    {
        if (_options.EnableContentLogging && !string.IsNullOrEmpty(content))
        {
            var truncatedContent = content.Length > _options.MaxContentLogLength 
                ? content[.._options.MaxContentLogLength] + "..." 
                : content;
            
            _logger.LogDebug("[{RequestId}] {Method} {Uri} response: {Content}", 
                requestId, method, uri, truncatedContent);
        }
    }
}