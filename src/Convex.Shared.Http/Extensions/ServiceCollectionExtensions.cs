using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Convex.Shared.Http.Policies;
using Convex.Shared.Http.Services;
using System.Text;

namespace Convex.Shared.Http.Extensions;

/// <summary>
/// Extension methods for configuring Convex HTTP client
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Convex HTTP client with default configuration
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configureOptions">Optional configuration action</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexHttpClient(
        this IServiceCollection services,
        Action<ConvexHttpClientOptions>? configureOptions = null)
    {
        // Configure options
        if (configureOptions != null)
        {
            services.Configure(configureOptions);
        }

        // Register token management service if OAuth2 is enabled
        if (configureOptions != null)
        {
            var tempOptions = new ConvexHttpClientOptions();
            configureOptions(tempOptions);
            if (tempOptions.Authentication.Type == AuthenticationTypes.OAuth2ClientCredentials && 
                tempOptions.Authentication.EnableTokenManagement)
            {
                services.AddMemoryCache();
                services.AddSingleton<ITokenManagementService, TokenManagementService>();
            }
        }

        // Register diagnostics and health check services
        services.AddSingleton<IHttpClientDiagnostics, HttpClientDiagnostics>();
        services.AddHealthChecks()
            .AddCheck<HttpClientHealthCheck>("http-client", tags: new[] { "http", "client", "external" });

        // Register the HTTP client with Polly policies
        services.AddHttpClient<IConvexHttpClient, ConvexHttpClient>()
            .AddPolicyHandler((serviceProvider, request) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<ConvexHttpClientOptions>>().Value;
                var logger = serviceProvider.GetRequiredService<ILogger<ConvexHttpClient>>();
                return ResiliencePolicyBuilder.CreateResiliencePolicy(options, logger);
            });

        return services;
    }

    /// <summary>
    /// Adds Convex HTTP client with specific base address
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="baseAddress">The base address for the HTTP client</param>
    /// <param name="apiKey">Optional API key</param>
    /// <param name="timeout">Optional timeout (default: 30 seconds)</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexHttpClient(
        this IServiceCollection services,
        string baseAddress,
        string? apiKey = null,
        TimeSpan? timeout = null)
    {
        services.Configure<ConvexHttpClientOptions>(options =>
        {
            options.BaseAddress = new Uri(baseAddress);
            options.ApiKey = apiKey;
            options.Timeout = timeout ?? TimeSpan.FromSeconds(30);
        });

        services.AddHttpClient<IConvexHttpClient, ConvexHttpClient>()
            .AddPolicyHandler((serviceProvider, request) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<ConvexHttpClientOptions>>().Value;
                var logger = serviceProvider.GetRequiredService<ILogger<ConvexHttpClient>>();
                return ResiliencePolicyBuilder.CreateResiliencePolicy(options, logger);
            });

        return services;
    }

    /// <summary>
    /// Adds Convex HTTP client for service-to-service communication
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="serviceName">The name of the service</param>
    /// <param name="baseAddress">The base address for the service</param>
    /// <param name="apiKey">Optional API key</param>
    /// <param name="timeout">Optional timeout (default: 30 seconds)</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexServiceClient(
        this IServiceCollection services,
        string serviceName,
        string baseAddress,
        string? apiKey = null,
        TimeSpan? timeout = null)
    {
        services.AddHttpClient(serviceName, client =>
        {
            client.BaseAddress = new Uri(baseAddress);
            client.Timeout = timeout ?? TimeSpan.FromSeconds(30);

            if (!string.IsNullOrEmpty(apiKey))
            {
                client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
            }
        });

        return services;
    }

    /// <summary>
    /// Adds multiple Convex HTTP clients with different authentication types
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="clients">Dictionary of client configurations</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexHttpClients(
        this IServiceCollection services,
        Dictionary<string, ConvexHttpClientOptions> clients)
    {
        foreach (var client in clients)
        {
            var clientName = client.Key;
            var options = client.Value;

            // Register token management service if OAuth2 is enabled
            if (options.Authentication.Type == AuthenticationTypes.OAuth2ClientCredentials && 
                options.Authentication.EnableTokenManagement)
            {
                services.AddMemoryCache();
                services.AddSingleton<ITokenManagementService>(provider => 
                    new TokenManagementService(
                        provider.GetRequiredService<HttpClient>(),
                        provider.GetRequiredService<IMemoryCache>(),
                        provider.GetRequiredService<ILogger<TokenManagementService>>(),
                        Microsoft.Extensions.Options.Options.Create(options)));
            }

            // Register named HTTP client
            services.AddHttpClient(clientName, httpClient =>
            {
                if (options.BaseAddress != null)
                    httpClient.BaseAddress = options.BaseAddress;
                
                httpClient.Timeout = options.Timeout;

                // Configure authentication based on type
                ConfigureAuthentication(httpClient, options);
            })
            .AddPolicyHandler((serviceProvider, request) =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<ConvexHttpClient>>();
                return ResiliencePolicyBuilder.CreateResiliencePolicy(options, logger);
            });
        }

        return services;
    }

    /// <summary>
    /// Adds a named Convex HTTP client with specific authentication
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="clientName">The name of the client</param>
    /// <param name="configureOptions">Configuration action</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexHttpClient(
        this IServiceCollection services,
        string clientName,
        Action<ConvexHttpClientOptions> configureOptions)
    {
        var options = new ConvexHttpClientOptions();
        configureOptions(options);

        // Register token management service if OAuth2 is enabled
        if (options.Authentication.Type == AuthenticationTypes.OAuth2ClientCredentials && 
            options.Authentication.EnableTokenManagement)
        {
            services.AddMemoryCache();
            services.AddSingleton<ITokenManagementService>(provider => 
                new TokenManagementService(
                    provider.GetRequiredService<HttpClient>(),
                    provider.GetRequiredService<IMemoryCache>(),
                    provider.GetRequiredService<ILogger<TokenManagementService>>(),
                    Microsoft.Extensions.Options.Options.Create(options)));
        }

        // Register named HTTP client
        services.AddHttpClient(clientName, httpClient =>
        {
            if (options.BaseAddress != null)
                httpClient.BaseAddress = options.BaseAddress;
            
            httpClient.Timeout = options.Timeout;

            // Configure authentication based on type
            ConfigureAuthentication(httpClient, options);
        })
        .AddPolicyHandler((serviceProvider, request) =>
        {
            var logger = serviceProvider.GetRequiredService<ILogger<ConvexHttpClient>>();
            return ResiliencePolicyBuilder.CreateResiliencePolicy(options, logger);
        });

        return services;
    }

    private static void ConfigureAuthentication(HttpClient httpClient, ConvexHttpClientOptions options)
    {
        switch (options.Authentication.Type)
        {
            case AuthenticationTypes.ApiKey:
                if (!string.IsNullOrEmpty(options.Authentication.ApiKey))
                {
                    httpClient.DefaultRequestHeaders.Add(options.Authentication.ApiKeyHeaderName, options.Authentication.ApiKey);
                }
                break;

            case AuthenticationTypes.BearerToken:
                if (!string.IsNullOrEmpty(options.Authentication.BearerToken))
                {
                    httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", options.Authentication.BearerToken);
                }
                break;

            case AuthenticationTypes.Basic:
                if (!string.IsNullOrEmpty(options.Authentication.Username) && !string.IsNullOrEmpty(options.Authentication.Password))
                {
                    var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{options.Authentication.Username}:{options.Authentication.Password}"));
                    httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
                }
                break;

            case AuthenticationTypes.Custom:
                if (!string.IsNullOrEmpty(options.Authentication.CustomHeaderName) && !string.IsNullOrEmpty(options.Authentication.CustomHeaderValue))
                {
                    httpClient.DefaultRequestHeaders.Add(options.Authentication.CustomHeaderName, options.Authentication.CustomHeaderValue);
                }
                break;
        }

        // Add custom headers
        foreach (var header in options.DefaultHeaders)
        {
            httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
        }
    }

    /// <summary>
    /// Adds Convex HTTP client with configuration from appsettings.json
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <param name="sectionName">The configuration section name (default: "ConvexHttpClient")</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexHttpClient(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = "ConvexHttpClient")
    {
        services.Configure<ConvexHttpClientOptions>(configuration.GetSection(sectionName));
        services.AddHttpClient<IConvexHttpClient, ConvexHttpClient>()
            .AddPolicyHandler((serviceProvider, request) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<ConvexHttpClientOptions>>().Value;
                var logger = serviceProvider.GetRequiredService<ILogger<ConvexHttpClient>>();
                return ResiliencePolicyBuilder.CreateResiliencePolicy(options, logger);
            });

        return services;
    }
}