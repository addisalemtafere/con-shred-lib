using Convex.Shared.Grpc.Configuration;
using Convex.Shared.Grpc.Extensions;
using Convex.Shared.Grpc.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Convex.Examples.GrpcClientSideLoadBalancing;

/// <summary>
/// Example demonstrating client-side load balancing for gRPC services in Kubernetes
/// This solves the L4 load balancing issue with gRPC in Kubernetes
/// </summary>
public class GrpcClientSideLoadBalancingExample
{
    /// <summary>
    /// Configure services for client-side load balancing
    /// </summary>
    public static void ConfigureServices(IServiceCollection services)
    {
        // Add Convex gRPC services with client-side load balancing
        services.AddConvexGrpc(options =>
        {
            // Enable service discovery for Kubernetes
            options.EnableServiceDiscovery = true;
            options.EnableLoadBalancing = true;
            
            // Professional service naming
            options.ServiceName = "convex-betting-service";
            options.ServiceInstancePrefix = "convex-grpc";
            
            // Health checks for Kubernetes
            options.EnableHealthChecks = true;
            options.EnableMetrics = true;
            
            // Production settings
            options.EnableTls = true;
            options.EnableAuthentication = true;
            options.EnableCompression = true;
            options.ServerPort = 50051;
        });

        // Add health checks for Kubernetes probes
        services.AddHealthChecks()
            .AddCheck<ConvexGrpcHealthCheck>("grpc-health-check");
    }

    /// <summary>
    /// Example of using client-side load balancing
    /// </summary>
    public static async Task ClientSideLoadBalancingExample(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<GrpcClientSideLoadBalancingExample>>();
        var clientFactory = serviceProvider.GetRequiredService<ConvexGrpcLoadBalancedClientFactory>();
        
        // Create a load-balanced gRPC client
        // This solves the Kubernetes L4 load balancing issue
        var userServiceClient = clientFactory.CreateLoadBalancedClient<UserService.UserServiceClient>("convex-user-service");
        
        try
        {
            // Make gRPC calls - they will be load balanced across healthy instances
            var request = new UserService.GetUserRequest { UserId = "123" };
            var response = await userServiceClient.GetUserAsync(request);
            
            logger.LogInformation("Successfully called user service with load balancing: {UserId}", response.UserId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error calling user service");
        }

        // Create another load-balanced client for payment service
        var paymentServiceClient = clientFactory.CreateLoadBalancedClient<PaymentService.PaymentServiceClient>("convex-payment-service");
        
        try
        {
            var paymentRequest = new PaymentService.ProcessPaymentRequest 
            { 
                Amount = 100.00m, 
                Currency = "USD" 
            };
            var paymentResponse = await paymentServiceClient.ProcessPaymentAsync(paymentRequest);
            
            logger.LogInformation("Successfully processed payment with load balancing: {TransactionId}", paymentResponse.TransactionId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing payment");
        }
    }

    /// <summary>
    /// Example of monitoring load balancing statistics
    /// </summary>
    public static async Task LoadBalancingStatisticsExample(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<GrpcClientSideLoadBalancingExample>>();
        var clientFactory = serviceProvider.GetRequiredService<ConvexGrpcLoadBalancedClientFactory>();
        
        // Get load balancing statistics
        var stats = clientFactory.GetLoadBalancingStatistics();
        
        foreach (var service in stats.ServiceStatistics)
        {
            logger.LogInformation("Service {ServiceName}: {HealthyEndpoints}/{TotalEndpoints} healthy endpoints", 
                service.Key, service.Value.HealthyEndpoints, service.Value.TotalEndpoints);
        }

        // Get all available endpoints for a service
        var userServiceEndpoints = clientFactory.GetAllServiceEndpoints("convex-user-service");
        logger.LogInformation("Found {Count} user service endpoints", userServiceEndpoints.Count);
        
        foreach (var endpoint in userServiceEndpoints)
        {
            logger.LogInformation("Endpoint: {Endpoint}, Healthy: {IsHealthy}, Health Score: {HealthScore}", 
                endpoint.Endpoint, endpoint.IsHealthy, endpoint.HealthScore);
        }
    }

    /// <summary>
    /// Example of updating endpoint health status
    /// </summary>
    public static async Task UpdateEndpointHealthExample(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<GrpcClientSideLoadBalancingExample>>();
        var clientFactory = serviceProvider.GetRequiredService<ConvexGrpcLoadBalancedClientFactory>();
        
        // Update endpoint health status (in real scenario, this would come from health checks)
        clientFactory.UpdateEndpointHealth("convex-user-service", "http://user-service-pod-1:50051", false);
        clientFactory.UpdateEndpointHealth("convex-user-service", "http://user-service-pod-2:50051", true);
        
        logger.LogInformation("Updated endpoint health status");
    }

    /// <summary>
    /// Example of using specific endpoint (bypass load balancing)
    /// </summary>
    public static async Task SpecificEndpointExample(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<GrpcClientSideLoadBalancingExample>>();
        var clientFactory = serviceProvider.GetRequiredService<ConvexGrpcLoadBalancedClientFactory>();
        
        // Create client for specific endpoint (bypass load balancing)
        var specificEndpoint = "http://user-service-pod-1.default.svc.cluster.local:50051";
        var userServiceClient = clientFactory.CreateClientForEndpoint<UserService.UserServiceClient>(specificEndpoint);
        
        try
        {
            var request = new UserService.GetUserRequest { UserId = "123" };
            var response = await userServiceClient.GetUserAsync(request);
            
            logger.LogInformation("Successfully called user service on specific endpoint: {UserId}", response.UserId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error calling user service on specific endpoint");
        }
    }
}

/// <summary>
/// Example service that uses client-side load balancing
/// </summary>
public class BettingService
{
    private readonly ConvexGrpcLoadBalancedClientFactory _clientFactory;
    private readonly ILogger<BettingService> _logger;

    public BettingService(ConvexGrpcLoadBalancedClientFactory clientFactory, ILogger<BettingService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Process a bet using load-balanced gRPC calls
    /// </summary>
    public async Task<BetResult> ProcessBetAsync(BetRequest request)
    {
        try
        {
            // Get user information using load-balanced client
            var userClient = _clientFactory.CreateLoadBalancedClient<UserService.UserServiceClient>("convex-user-service");
            var userRequest = new UserService.GetUserRequest { UserId = request.UserId };
            var userResponse = await userClient.GetUserAsync(userRequest);
            
            if (!userResponse.IsActive)
            {
                throw new InvalidOperationException("User is not active");
            }

            // Process payment using load-balanced client
            var paymentClient = _clientFactory.CreateLoadBalancedClient<PaymentService.PaymentServiceClient>("convex-payment-service");
            var paymentRequest = new PaymentService.ProcessPaymentRequest 
            { 
                UserId = request.UserId,
                Amount = request.Amount,
                Currency = "USD"
            };
            var paymentResponse = await paymentClient.ProcessPaymentAsync(paymentRequest);
            
            if (!paymentResponse.Success)
            {
                throw new InvalidOperationException("Payment failed");
            }

            // Create bet record
            var betResult = new BetResult
            {
                BetId = Guid.NewGuid().ToString(),
                UserId = request.UserId,
                Amount = request.Amount,
                TransactionId = paymentResponse.TransactionId,
                Status = "Placed",
                CreatedAt = DateTime.UtcNow
            };

            _logger.LogInformation("Successfully processed bet {BetId} for user {UserId} with amount {Amount}", 
                betResult.BetId, betResult.UserId, betResult.Amount);

            return betResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing bet for user {UserId}", request.UserId);
            throw;
        }
    }
}

/// <summary>
/// Example of Kubernetes deployment with client-side load balancing
/// </summary>
public class KubernetesDeploymentExample
{
    /// <summary>
    /// Example Kubernetes deployment YAML for gRPC service with client-side load balancing
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
spec:
  replicas: 3  # Multiple replicas for load balancing
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
        readinessProbe:
          httpGet:
            path: /health/convex-betting-service/readiness
            port: 8080
          initialDelaySeconds: 5
          periodSeconds: 5
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
  type: ClusterIP  # ClusterIP for client-side load balancing
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
";
    }
}

// Example gRPC service definitions (these would be generated from .proto files)
namespace UserService
{
    public class UserServiceClient
    {
        public UserServiceClient(Grpc.Core.ChannelBase channel) { }
        public virtual async Task<GetUserResponse> GetUserAsync(GetUserRequest request) => throw new NotImplementedException();
    }
    
    public class GetUserRequest
    {
        public string UserId { get; set; } = string.Empty;
    }
    
    public class GetUserResponse
    {
        public string UserId { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}

namespace PaymentService
{
    public class PaymentServiceClient
    {
        public PaymentServiceClient(Grpc.Core.ChannelBase channel) { }
        public virtual async Task<ProcessPaymentResponse> ProcessPaymentAsync(ProcessPaymentRequest request) => throw new NotImplementedException();
    }
    
    public class ProcessPaymentRequest
    {
        public string UserId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
    }
    
    public class ProcessPaymentResponse
    {
        public string TransactionId { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
}

// Example models
public class BetRequest
{
    public string UserId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class BetResult
{
    public string BetId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
