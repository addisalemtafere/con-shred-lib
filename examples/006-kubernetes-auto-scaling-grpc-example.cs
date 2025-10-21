using Convex.Shared.Grpc.Configuration;
using Convex.Shared.Grpc.Extensions;
using Convex.Shared.Grpc.Services;
using Convex.Shared.Grpc.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Convex.Examples.KubernetesAutoScaling;

/// <summary>
/// Example demonstrating professional Kubernetes auto-scaling gRPC services
/// </summary>
public class KubernetesAutoScalingGrpcExample
{
    /// <summary>
    /// Configure services for Kubernetes auto-scaling
    /// </summary>
    public static void ConfigureServices(IServiceCollection services)
    {
        // Add Convex gRPC services with Kubernetes auto-scaling support
        services.AddConvexGrpc(options =>
        {
            // Enable Kubernetes auto-scaling features
            options.EnableAutoScaling = true;
            options.EnableServiceDiscovery = true;
            options.EnableLoadBalancing = true;
            
            // Professional service naming
            options.ServiceName = "convex-betting-service";
            options.ServiceInstancePrefix = "convex-grpc";
            
            // Auto-scaling thresholds
            options.AutoScaleCpuThreshold = 70.0;
            options.AutoScaleMemoryThreshold = 80.0;
            options.AutoScaleRequestThreshold = 100.0;
            options.AutoScaleDownCpuThreshold = 30.0;
            options.AutoScaleDownMemoryThreshold = 40.0;
            options.AutoScaleDownRequestThreshold = 20.0;
            
            // Scaling limits
            options.MinServiceInstances = 2;
            options.MaxServiceInstances = 10;
            
            // Health check configuration
            options.EnableHealthChecks = true;
            options.EnableMetrics = true;
            options.EnableDistributedTracing = true;
            
            // Production settings
            options.EnableTls = true;
            options.EnableAuthentication = true;
            options.EnableCompression = true;
            options.ServerPort = 50051;
        });

        // Add health check endpoints for Kubernetes probes
        services.AddHealthChecks()
            .AddCheck<ConvexGrpcHealthCheck>("grpc-health-check");
    }

    /// <summary>
    /// Example of using Kubernetes service discovery
    /// </summary>
    public static async Task ServiceDiscoveryExample(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<KubernetesAutoScalingGrpcExample>>();
        var serviceDiscovery = serviceProvider.GetRequiredService<ConvexGrpcKubernetesServiceDiscovery>();
        
        // Get service endpoint using professional service discovery
        var userServiceEndpoint = serviceDiscovery.GetServiceEndpoint("convex-user-service");
        if (userServiceEndpoint != null)
        {
            logger.LogInformation("Discovered user service endpoint: {Endpoint}", userServiceEndpoint.Endpoint);
            logger.LogInformation("Service health: {IsHealthy}, Score: {HealthScore}", 
                userServiceEndpoint.IsHealthy, userServiceEndpoint.HealthScore);
        }

        // Get all available endpoints for a service
        var allEndpoints = serviceDiscovery.GetAllServiceEndpoints("convex-payment-service");
        logger.LogInformation("Found {Count} payment service endpoints", allEndpoints.Count);

        // Get service discovery statistics
        var stats = serviceDiscovery.GetDiscoveryStatistics();
        foreach (var service in stats.ServiceStatistics)
        {
            logger.LogInformation("Service {ServiceName}: {HealthyEndpoints}/{TotalEndpoints} healthy endpoints", 
                service.Key, service.Value.HealthyEndpoints, service.Value.TotalEndpoints);
        }
    }

    /// <summary>
    /// Example of using health check service for Kubernetes probes
    /// </summary>
    public static async Task HealthCheckExample(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<KubernetesAutoScalingGrpcExample>>();
        var healthCheckService = serviceProvider.GetRequiredService<ConvexGrpcHealthCheckService>();
        
        // Get basic health status
        var healthStatus = healthCheckService.GetHealthStatus();
        logger.LogInformation("Service health: {IsHealthy}, Score: {HealthScore}, CPU: {CpuUsage}%, Memory: {MemoryUsage}%", 
            healthStatus.IsHealthy, healthStatus.HealthScore, healthStatus.CpuUsage, healthStatus.MemoryUsage);

        // Get detailed health status for readiness probe
        var detailedHealth = healthCheckService.GetDetailedHealthStatus();
        logger.LogInformation("Detailed health check: {IsHealthy}", detailedHealth.IsHealthy);
        foreach (var component in detailedHealth.ComponentHealth)
        {
            logger.LogInformation("Component {ComponentName}: {IsHealthy}", 
                component.Key, component.Value.IsHealthy);
        }

        // Get liveness probe result
        var livenessResult = healthCheckService.GetLivenessStatus();
        logger.LogInformation("Liveness probe: {IsAlive}, Uptime: {Uptime}", 
            livenessResult.IsAlive, livenessResult.Uptime);

        // Get readiness probe result
        var readinessResult = healthCheckService.GetReadinessStatus();
        logger.LogInformation("Readiness probe: {IsReady}", readinessResult.IsReady);
    }

    /// <summary>
    /// Example of using professional service naming
    /// </summary>
    public static void ServiceNamingExample()
    {
        var logger = new LoggerFactory().CreateLogger<KubernetesAutoScalingGrpcExample>();
        
        // Generate professional service names
        var userServiceName = ConvexGrpcServiceNaming.GenerateServiceName("user", "prod", "v1");
        var paymentServiceName = ConvexGrpcServiceNaming.GenerateServiceName("payment", "prod", "v1");
        var bettingServiceName = ConvexGrpcServiceNaming.GenerateServiceName("betting", "prod", "v1");
        
        logger.LogInformation("Generated service names:");
        logger.LogInformation("User Service: {UserServiceName}", userServiceName);
        logger.LogInformation("Payment Service: {PaymentServiceName}", paymentServiceName);
        logger.LogInformation("Betting Service: {BettingServiceName}", bettingServiceName);

        // Generate professional instance names
        var instanceName1 = ConvexGrpcServiceNaming.GenerateInstanceName(userServiceName, "pod-001", 1);
        var instanceName2 = ConvexGrpcServiceNaming.GenerateInstanceName(userServiceName, "pod-002", 2);
        
        logger.LogInformation("Generated instance names:");
        logger.LogInformation("Instance 1: {InstanceName1}", instanceName1);
        logger.LogInformation("Instance 2: {InstanceName2}", instanceName2);

        // Generate professional service endpoints
        var userServiceEndpoint = ConvexGrpcServiceNaming.GenerateServiceEndpoint(userServiceName, "default", 50051);
        var paymentServiceEndpoint = ConvexGrpcServiceNaming.GenerateServiceEndpoint(paymentServiceName, "default", 50052);
        
        logger.LogInformation("Generated service endpoints:");
        logger.LogInformation("User Service: {UserServiceEndpoint}", userServiceEndpoint);
        logger.LogInformation("Payment Service: {PaymentServiceEndpoint}", paymentServiceEndpoint);

        // Generate Kubernetes labels
        var labels = ConvexGrpcServiceNaming.GenerateKubernetesLabels("betting", "prod", "v1", "convex-team");
        logger.LogInformation("Generated Kubernetes labels:");
        foreach (var label in labels)
        {
            logger.LogInformation("  {Key}: {Value}", label.Key, label.Value);
        }

        // Generate Kubernetes annotations
        var annotations = ConvexGrpcServiceNaming.GenerateKubernetesAnnotations("betting", "Betting service for sports betting platform", "convex-team");
        logger.LogInformation("Generated Kubernetes annotations:");
        foreach (var annotation in annotations)
        {
            logger.LogInformation("  {Key}: {Value}", annotation.Key, annotation.Value);
        }
    }

    /// <summary>
    /// Example of using service manager
    /// </summary>
    public static async Task ServiceManagerExample(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<KubernetesAutoScalingGrpcExample>>();
        var serviceManager = serviceProvider.GetRequiredService<ConvexGrpcServiceManager>();
        
        // Register service instances (in real scenario, Kubernetes would do this)
        var userServiceInstance1 = new ServiceInstance
        {
            InstanceId = "convex-user-service-pod-001",
            Endpoint = "http://convex-user-service-pod-001.default.svc.cluster.local:50051",
            IsHealthy = true,
            IsAvailable = true,
            HealthScore = 95,
            ActiveConnections = 10,
            ResponseTimeMs = 50
        };

        var userServiceInstance2 = new ServiceInstance
        {
            InstanceId = "convex-user-service-pod-002",
            Endpoint = "http://convex-user-service-pod-002.default.svc.cluster.local:50051",
            IsHealthy = true,
            IsAvailable = true,
            HealthScore = 90,
            ActiveConnections = 15,
            ResponseTimeMs = 45
        };

        serviceManager.RegisterServiceInstance("convex-user-service", userServiceInstance1);
        serviceManager.RegisterServiceInstance("convex-user-service", userServiceInstance2);

        // Update service metrics (in real scenario, this would come from monitoring)
        var metrics = new ServiceMetrics
        {
            RequestRate = 150.0,
            ErrorRate = 0.5,
            AverageResponseTime = 45.0
        };

        serviceManager.UpdateServiceMetrics("convex-user-service", metrics);

        // Get best service instance
        var bestInstance = serviceManager.GetBestServiceInstance("convex-user-service");
        if (bestInstance != null)
        {
            logger.LogInformation("Best instance selected: {InstanceId} with health score {HealthScore}", 
                bestInstance.InstanceId, bestInstance.HealthScore);
        }

        // Get service management statistics
        var serviceStats = serviceManager.GetServiceStatistics();
        foreach (var service in serviceStats.ServiceStatistics)
        {
            logger.LogInformation("Service {ServiceName}: {InstanceCount} instances, RequestRate: {RequestRate}/s, ResponseTime: {ResponseTime}ms", 
                service.Key, service.Value.InstanceCount, service.Value.AverageRequestRate, service.Value.AverageResponseTime);
        }
    }
}

/// <summary>
/// Custom health check for gRPC services
/// </summary>
public class ConvexGrpcHealthCheck : Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck
{
    private readonly ConvexGrpcHealthCheckService _healthCheckService;

    public ConvexGrpcHealthCheck(ConvexGrpcHealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    public async Task<Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult> CheckHealthAsync(
        Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var healthStatus = _healthCheckService.GetHealthStatus();
            
            if (healthStatus.IsHealthy)
            {
                return Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy(
                    $"Service is healthy. Health Score: {healthStatus.HealthScore}, CPU: {healthStatus.CpuUsage:F1}%, Memory: {healthStatus.MemoryUsage:F1}%");
            }
            else
            {
                return Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Unhealthy(
                    $"Service is unhealthy. Health Score: {healthStatus.HealthScore}, Error: {healthStatus.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            return Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Unhealthy(
                $"Health check failed: {ex.Message}");
        }
    }
}

/// <summary>
/// Example of Kubernetes deployment configuration
/// </summary>
public class KubernetesDeploymentExample
{
    /// <summary>
    /// Example Kubernetes deployment YAML for gRPC service with auto-scaling
    /// </summary>
    public static string GetKubernetesDeploymentYaml()
    {
        return @"
apiVersion: apps/v1
kind: Deployment
metadata:
  name: convex-betting-service
  namespace: default
  labels:
    app: convex-betting-service
    version: v1.0.0
    environment: production
    team: convex-team
    component: grpc-service
    managed-by: convex-platform
    part-of: convex-betting-platform
  annotations:
    convex.service/type: betting
    convex.service/description: Betting service for sports betting platform
    convex.service/maintainer: convex-team
    convex.service/created-by: convex-grpc-platform
    convex.service/auto-scaling: enabled
    convex.service/load-balancing: enabled
spec:
  replicas: 3
  selector:
    matchLabels:
      app: convex-betting-service
  template:
    metadata:
      labels:
        app: convex-betting-service
        version: v1.0.0
        environment: production
        team: convex-team
        component: grpc-service
    spec:
      containers:
      - name: convex-betting-service
        image: convex/betting-service:v1.0.0
        ports:
        - containerPort: 50051
          name: grpc
        - containerPort: 8080
          name: health
        - containerPort: 9090
          name: metrics
        env:
        - name: ASPNETCORE_ENVIRONMENT
          value: Production
        - name: GRPC_PORT
          value: ""50051""
        - name: HEALTH_PORT
          value: ""8080""
        - name: METRICS_PORT
          value: ""9090""
        resources:
          requests:
            memory: ""256Mi""
            cpu: ""250m""
          limits:
            memory: ""512Mi""
            cpu: ""500m""
        livenessProbe:
          httpGet:
            path: /health/convex-betting-service/liveness
            port: 8080
          initialDelaySeconds: 30
          periodSeconds: 10
          timeoutSeconds: 5
          failureThreshold: 3
        readinessProbe:
          httpGet:
            path: /health/convex-betting-service/readiness
            port: 8080
          initialDelaySeconds: 5
          periodSeconds: 5
          timeoutSeconds: 3
          failureThreshold: 3
        startupProbe:
          httpGet:
            path: /health/convex-betting-service/startup
            port: 8080
          initialDelaySeconds: 10
          periodSeconds: 10
          timeoutSeconds: 5
          failureThreshold: 30
---
apiVersion: v1
kind: Service
metadata:
  name: convex-betting-service
  namespace: default
  labels:
    app: convex-betting-service
    version: v1.0.0
    environment: production
    team: convex-team
    component: grpc-service
spec:
  selector:
    app: convex-betting-service
  ports:
  - name: grpc
    port: 50051
    targetPort: 50051
    protocol: TCP
  - name: health
    port: 8080
    targetPort: 8080
    protocol: TCP
  - name: metrics
    port: 9090
    targetPort: 9090
    protocol: TCP
  type: ClusterIP
---
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: convex-betting-service-hpa
  namespace: default
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: convex-betting-service
  minReplicas: 2
  maxReplicas: 10
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 70
  - type: Resource
    resource:
      name: memory
      target:
        type: Utilization
        averageUtilization: 80
  behavior:
    scaleUp:
      stabilizationWindowSeconds: 60
      policies:
      - type: Pods
        value: 2
        periodSeconds: 60
      - type: Percent
        value: 100
        periodSeconds: 15
    scaleDown:
      stabilizationWindowSeconds: 300
      policies:
      - type: Pods
        value: 1
        periodSeconds: 60
";
    }
}
