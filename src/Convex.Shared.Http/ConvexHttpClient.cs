using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

    public ConvexHttpClient(
        HttpClient httpClient,
        ILogger<ConvexHttpClient> logger,
        IOptions<ConvexHttpClientOptions> options)
    {
        _httpClient = httpClient;
        _logger = logger;
        _options = options.Value;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        ConfigureHttpClient();
    }

    public async Task<TResponse?> GetAsync<TResponse>(string uri, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.EnableLogging)
            {
                _logger.LogDebug("GET {Uri}", uri);
            }

            var response = await _httpClient.GetAsync(uri, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<TResponse>(content, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GET {Uri} failed", uri);
            throw;
        }
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string uri, TRequest content, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.EnableLogging)
            {
                _logger.LogDebug("POST {Uri}", uri);
            }

            var json = JsonSerializer.Serialize(content, _jsonOptions);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, httpContent, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST {Uri} failed", uri);
            throw;
        }
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string uri, TRequest content, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.EnableLogging)
            {
                _logger.LogDebug("PUT {Uri}", uri);
            }

            var json = JsonSerializer.Serialize(content, _jsonOptions);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(uri, httpContent, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PUT {Uri} failed", uri);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(string uri, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_options.EnableLogging)
            {
                _logger.LogDebug("DELETE {Uri}", uri);
            }

            var response = await _httpClient.DeleteAsync(uri, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DELETE {Uri} failed", uri);
            return false;
        }
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

}
