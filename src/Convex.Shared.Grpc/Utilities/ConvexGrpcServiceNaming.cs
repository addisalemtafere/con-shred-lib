using Convex.Shared.Grpc.Configuration;

namespace Convex.Shared.Grpc.Utilities;

/// <summary>
/// Professional service naming utilities for Kubernetes auto-scaling
/// </summary>
public static class ConvexGrpcServiceNaming
{
    /// <summary>
    /// Generate professional service name for Kubernetes
    /// </summary>
    /// <param name="serviceType">Type of service (user, payment, betting, etc.)</param>
    /// <param name="environment">Environment (dev, staging, prod)</param>
    /// <param name="version">Service version</param>
    /// <returns>Professional service name</returns>
    public static string GenerateServiceName(string serviceType, string environment = "prod", string version = "v1")
    {
        var cleanServiceType = SanitizeServiceName(serviceType);
        var cleanEnvironment = SanitizeServiceName(environment);
        var cleanVersion = SanitizeServiceName(version);
        
        return $"convex-{cleanServiceType}-service-{cleanEnvironment}-{cleanVersion}";
    }

    /// <summary>
    /// Generate professional instance name for auto-scaled pods
    /// </summary>
    /// <param name="serviceName">Base service name</param>
    /// <param name="instanceId">Instance identifier</param>
    /// <param name="replicaIndex">Replica index</param>
    /// <returns>Professional instance name</returns>
    public static string GenerateInstanceName(string serviceName, string instanceId, int replicaIndex)
    {
        var cleanServiceName = SanitizeServiceName(serviceName);
        var cleanInstanceId = SanitizeServiceName(instanceId);
        
        return $"{cleanServiceName}-{cleanInstanceId}-{replicaIndex:D3}";
    }

    /// <summary>
    /// Generate professional Kubernetes service endpoint
    /// </summary>
    /// <param name="serviceName">Service name</param>
    /// <param name="namespace">Kubernetes namespace</param>
    /// <param name="port">Service port</param>
    /// <returns>Professional service endpoint</returns>
    public static string GenerateServiceEndpoint(string serviceName, string @namespace = "default", int port = 50051)
    {
        var cleanServiceName = SanitizeServiceName(serviceName);
        var cleanNamespace = SanitizeServiceName(@namespace);
        
        return $"http://{cleanServiceName}.{cleanNamespace}.svc.cluster.local:{port}";
    }

    /// <summary>
    /// Generate professional Kubernetes labels for service discovery
    /// </summary>
    /// <param name="serviceType">Service type</param>
    /// <param name="environment">Environment</param>
    /// <param name="version">Version</param>
    /// <param name="team">Team name</param>
    /// <returns>Professional Kubernetes labels</returns>
    public static Dictionary<string, string> GenerateKubernetesLabels(string serviceType, string environment = "prod", string version = "v1", string team = "convex")
    {
        return new Dictionary<string, string>
        {
            ["app"] = $"convex-{SanitizeServiceName(serviceType)}-service",
            ["version"] = SanitizeServiceName(version),
            ["environment"] = SanitizeServiceName(environment),
            ["team"] = SanitizeServiceName(team),
            ["component"] = "grpc-service",
            ["managed-by"] = "convex-platform",
            ["part-of"] = "convex-betting-platform"
        };
    }

    /// <summary>
    /// Generate professional Kubernetes annotations for service metadata
    /// </summary>
    /// <param name="serviceType">Service type</param>
    /// <param name="description">Service description</param>
    /// <param name="maintainer">Maintainer information</param>
    /// <returns>Professional Kubernetes annotations</returns>
    public static Dictionary<string, string> GenerateKubernetesAnnotations(string serviceType, string description = "", string maintainer = "convex-team")
    {
        return new Dictionary<string, string>
        {
            ["convex.service/type"] = SanitizeServiceName(serviceType),
            ["convex.service/description"] = description,
            ["convex.service/maintainer"] = maintainer,
            ["convex.service/created-by"] = "convex-grpc-platform",
            ["convex.service/auto-scaling"] = "enabled",
            ["convex.service/load-balancing"] = "enabled"
        };
    }

    /// <summary>
    /// Generate professional health check endpoint path
    /// </summary>
    /// <param name="serviceName">Service name</param>
    /// <param name="checkType">Health check type (liveness, readiness, startup)</param>
    /// <returns>Professional health check endpoint</returns>
    public static string GenerateHealthCheckEndpoint(string serviceName, string checkType = "readiness")
    {
        var cleanServiceName = SanitizeServiceName(serviceName);
        var cleanCheckType = SanitizeServiceName(checkType);
        
        return $"/health/{cleanServiceName}/{cleanCheckType}";
    }

    /// <summary>
    /// Generate professional metrics endpoint path
    /// </summary>
    /// <param name="serviceName">Service name</param>
    /// <param name="metricType">Metric type (prometheus, custom)</param>
    /// <returns>Professional metrics endpoint</returns>
    public static string GenerateMetricsEndpoint(string serviceName, string metricType = "prometheus")
    {
        var cleanServiceName = SanitizeServiceName(serviceName);
        var cleanMetricType = SanitizeServiceName(metricType);
        
        return $"/metrics/{cleanServiceName}/{cleanMetricType}";
    }

    /// <summary>
    /// Generate professional service discovery configuration
    /// </summary>
    /// <param name="options">gRPC configuration options</param>
    /// <returns>Service discovery configuration</returns>
    public static ServiceDiscoveryConfig GenerateServiceDiscoveryConfig(ConvexGrpcOptions options)
    {
        return new ServiceDiscoveryConfig
        {
            ServiceName = options.ServiceName ?? "convex-grpc-service",
            Namespace = "default",
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
            Version = "1.0.0",
            Labels = GenerateKubernetesLabels("grpc", "prod", "v1"),
            Annotations = GenerateKubernetesAnnotations("grpc", "Convex gRPC Service", "convex-team"),
            HealthCheckEndpoint = GenerateHealthCheckEndpoint(options.ServiceName ?? "convex-grpc-service"),
            MetricsEndpoint = GenerateMetricsEndpoint(options.ServiceName ?? "convex-grpc-service"),
            ServiceEndpoint = GenerateServiceEndpoint(options.ServiceName ?? "convex-grpc-service", "default", options.ServerPort)
        };
    }

    /// <summary>
    /// Sanitize service name for Kubernetes compatibility
    /// </summary>
    /// <param name="name">Name to sanitize</param>
    /// <returns>Sanitized name</returns>
    private static string SanitizeServiceName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "unknown";

        return name.ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("_", "-")
            .Replace(".", "-")
            .Replace(":", "-")
            .Replace("/", "-")
            .Replace("\\", "-")
            .Replace("@", "-")
            .Replace("#", "-")
            .Replace("$", "-")
            .Replace("%", "-")
            .Replace("^", "-")
            .Replace("&", "-")
            .Replace("*", "-")
            .Replace("(", "-")
            .Replace(")", "-")
            .Replace("+", "-")
            .Replace("=", "-")
            .Replace("[", "-")
            .Replace("]", "-")
            .Replace("{", "-")
            .Replace("}", "-")
            .Replace("|", "-")
            .Replace(";", "-")
            .Replace("'", "-")
            .Replace("\"", "-")
            .Replace(",", "-")
            .Replace("<", "-")
            .Replace(">", "-")
            .Replace("?", "-")
            .Replace("!", "-")
            .Replace("~", "-")
            .Replace("`", "-")
            .Trim('-')
            .ToLowerInvariant();
    }

    /// <summary>
    /// Validate service name for Kubernetes compatibility
    /// </summary>
    /// <param name="name">Name to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool IsValidKubernetesName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        // Kubernetes names must be lowercase, contain only alphanumeric characters and hyphens,
        // start and end with alphanumeric characters, and be no longer than 63 characters
        return name.Length <= 63 &&
               name.All(c => char.IsLetterOrDigit(c) || c == '-') &&
               char.IsLetterOrDigit(name[0]) &&
               char.IsLetterOrDigit(name[^1]);
    }
}

/// <summary>
/// Service discovery configuration for Kubernetes
/// </summary>
public class ServiceDiscoveryConfig
{
    /// <summary>
    /// Service name
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// Kubernetes namespace
    /// </summary>
    public string Namespace { get; set; } = "default";

    /// <summary>
    /// Environment
    /// </summary>
    public string Environment { get; set; } = "Production";

    /// <summary>
    /// Service version
    /// </summary>
    public string Version { get; set; } = "1.0.0";

    /// <summary>
    /// Kubernetes labels
    /// </summary>
    public Dictionary<string, string> Labels { get; set; } = new();

    /// <summary>
    /// Kubernetes annotations
    /// </summary>
    public Dictionary<string, string> Annotations { get; set; } = new();

    /// <summary>
    /// Health check endpoint
    /// </summary>
    public string HealthCheckEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// Metrics endpoint
    /// </summary>
    public string MetricsEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// Service endpoint
    /// </summary>
    public string ServiceEndpoint { get; set; } = string.Empty;
}
