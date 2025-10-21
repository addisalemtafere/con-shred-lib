# Convex gRPC Kubernetes Integration

This document describes the professional Kubernetes integration for Convex gRPC services.

## Overview

The Convex gRPC platform provides comprehensive Kubernetes integration that works seamlessly with Kubernetes' native HPA (Horizontal Pod Autoscaler) and VPA (Vertical Pod Autoscaler). This solution leverages Kubernetes' built-in auto-scaling mechanisms while providing professional service discovery, health monitoring, and load balancing.

## Key Features

### 🚀 **Kubernetes-Native Auto-Scaling**
- Integrates with Kubernetes HPA for horizontal scaling
- Supports VPA for vertical scaling
- Professional service discovery with automatic endpoint management
- Health check endpoints for Kubernetes probes

### ⚖️ **Client-Side Load Balancing**
- **Solves the Kubernetes L4 load balancing issue with gRPC**
- Implements client-side load balancing to distribute gRPC calls across pods
- Professional service discovery with automatic endpoint management
- Health check endpoints for Kubernetes probes

### 🏗️ **Professional Service Architecture**
- **ConvexGrpcKubernetesServiceDiscovery**: Service discovery with professional naming
- **ConvexGrpcHealthCheckService**: Health monitoring for Kubernetes probes
- **ConvexGrpcServiceManager**: Service management and load balancing
- **ConvexGrpcLoadBalancer**: Client-side load balancing for Kubernetes
- **ConvexGrpcLoadBalancedClientFactory**: gRPC client factory with load balancing
- **ConvexGrpcServiceNaming**: Professional naming utilities for Kubernetes

### 📊 **Advanced Monitoring & Metrics**
- Real-time health scoring (0-100)
- Request rate and response time tracking
- Component-specific health checks
- Kubernetes-compatible metrics endpoints

### 🔧 **Professional Configuration**
- Professional service naming conventions
- Kubernetes labels and annotations
- Health check and metrics endpoints

## Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    Kubernetes Cluster                      │
├─────────────────────────────────────────────────────────────┤
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────┐ │
│  │   HPA Controller│  │   VPA Controller│  │   K8s API   │ │
│  └─────────────────┘  └─────────────────┘  └─────────────┘ │
├─────────────────────────────────────────────────────────────┤
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────┐ │
│  │  gRPC Service   │  │  gRPC Service   │  │  gRPC Service│ │
│  │   Instance 1    │  │   Instance 2    │  │   Instance N│ │
│  └─────────────────┘  └─────────────────┘  └─────────────┘ │
├─────────────────────────────────────────────────────────────┤
│  ┌─────────────────────────────────────────────────────────┐ │
│  │         Convex gRPC Load Balancer                      │ │
│  │  • Service Discovery                                   │ │
│  │  • Health Monitoring                                    │ │
│  │  • Load Balancing                                       │ │
│  │  • Metrics Collection                                   │ │
│  └─────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
```

## Quick Start

### 1. Configure Services

```csharp
// Program.cs or Startup.cs
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
    
    // Scaling limits
    options.MinServiceInstances = 2;
    options.MaxServiceInstances = 10;
    
    // Health check configuration
    options.EnableHealthChecks = true;
    options.EnableMetrics = true;
});
```

### 2. Use Service Discovery

```csharp
public class BettingService
{
    private readonly ConvexGrpcKubernetesServiceDiscovery _serviceDiscovery;
    
    public BettingService(ConvexGrpcKubernetesServiceDiscovery serviceDiscovery)
    {
        _serviceDiscovery = serviceDiscovery;
    }
    
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        // Get the best available payment service endpoint
        var paymentEndpoint = _serviceDiscovery.GetServiceEndpoint("convex-payment-service");
        
        if (paymentEndpoint == null)
        {
            throw new ServiceUnavailableException("Payment service not available");
        }
        
        // Use the endpoint for gRPC communication
        // ... implementation
    }
}
```

### 3. Health Check Endpoints

```csharp
// Add health checks for Kubernetes probes
services.AddHealthChecks()
    .AddCheck<ConvexGrpcHealthCheck>("grpc-health-check");

// Health check endpoints are automatically available at:
// GET /health/{service-name}/liveness
// GET /health/{service-name}/readiness
// GET /health/{service-name}/startup
```

## Professional Service Naming

### Service Names
```csharp
// Generate professional service names
var userServiceName = ConvexGrpcServiceNaming.GenerateServiceName("user", "prod", "v1");
// Result: "convex-user-service-prod-v1"

var paymentServiceName = ConvexGrpcServiceNaming.GenerateServiceName("payment", "prod", "v1");
// Result: "convex-payment-service-prod-v1"
```

### Instance Names
```csharp
// Generate professional instance names
var instanceName = ConvexGrpcServiceNaming.GenerateInstanceName(
    "convex-user-service", "pod-001", 1);
// Result: "convex-user-service-pod-001-001"
```

### Service Endpoints
```csharp
// Generate professional service endpoints
var endpoint = ConvexGrpcServiceNaming.GenerateServiceEndpoint(
    "convex-user-service", "default", 50051);
// Result: "http://convex-user-service.default.svc.cluster.local:50051"
```

## Kubernetes Configuration

### Deployment with Auto-Scaling

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: convex-betting-service
  labels:
    app: convex-betting-service
    version: v1.0.0
    environment: production
    team: convex-team
    component: grpc-service
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
        resources:
          requests:
            memory: "256Mi"
            cpu: "250m"
          limits:
            memory: "512Mi"
            cpu: "500m"
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
```

### Horizontal Pod Autoscaler

```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: convex-betting-service-hpa
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
```

## Health Monitoring

### Health Check Results

```csharp
// Get basic health status
var healthStatus = healthCheckService.GetHealthStatus();
Console.WriteLine($"Healthy: {healthStatus.IsHealthy}");
Console.WriteLine($"Health Score: {healthStatus.HealthScore}");
Console.WriteLine($"CPU Usage: {healthStatus.CpuUsage}%");
Console.WriteLine($"Memory Usage: {healthStatus.MemoryUsage}%");

// Get detailed health status
var detailedHealth = healthCheckService.GetDetailedHealthStatus();
foreach (var component in detailedHealth.ComponentHealth)
{
    Console.WriteLine($"{component.Key}: {component.Value.IsHealthy}");
}
```

### Kubernetes Probes

```csharp
// Liveness probe - is the service alive?
var livenessResult = healthCheckService.GetLivenessStatus();
// Returns: IsAlive, Uptime, HealthScore

// Readiness probe - is the service ready to receive traffic?
var readinessResult = healthCheckService.GetReadinessStatus();
// Returns: IsReady, ComponentStatus, HealthScore
```

## Service Discovery

### Automatic Service Discovery

```csharp
// Get the best available service endpoint
var endpoint = serviceDiscovery.GetServiceEndpoint("convex-user-service");
if (endpoint != null)
{
    Console.WriteLine($"Endpoint: {endpoint.Endpoint}");
    Console.WriteLine($"Healthy: {endpoint.IsHealthy}");
    Console.WriteLine($"Health Score: {endpoint.HealthScore}");
}

// Get all available endpoints
var allEndpoints = serviceDiscovery.GetAllServiceEndpoints("convex-payment-service");
Console.WriteLine($"Found {allEndpoints.Count} payment service endpoints");
```

### Service Statistics

```csharp
// Get service discovery statistics
var stats = serviceDiscovery.GetDiscoveryStatistics();
foreach (var service in stats.ServiceStatistics)
{
    Console.WriteLine($"Service: {service.Key}");
    Console.WriteLine($"  Total Endpoints: {service.Value.TotalEndpoints}");
    Console.WriteLine($"  Healthy Endpoints: {service.Value.HealthyEndpoints}");
    Console.WriteLine($"  Unhealthy Endpoints: {service.Value.UnhealthyEndpoints}");
}
```

## Configuration Options

### Auto-Scaling Configuration

```csharp
public class ConvexGrpcOptions
{
    // Auto-scaling settings
    public bool EnableAutoScaling { get; set; } = true;
    public int MinServiceInstances { get; set; } = 1;
    public int MaxServiceInstances { get; set; } = 10;
    
    // Scaling thresholds
    public double AutoScaleCpuThreshold { get; set; } = 70.0;
    public double AutoScaleMemoryThreshold { get; set; } = 80.0;
    public double AutoScaleRequestThreshold { get; set; } = 100.0;
    
    // Scale-down thresholds
    public double AutoScaleDownCpuThreshold { get; set; } = 30.0;
    public double AutoScaleDownMemoryThreshold { get; set; } = 40.0;
    public double AutoScaleDownRequestThreshold { get; set; } = 20.0;
    
    // Professional naming
    public string ServiceInstancePrefix { get; set; } = "convex-grpc";
    public bool EnableServiceDiscovery { get; set; } = true;
}
```

## Best Practices

### 1. **Resource Management**
- Set appropriate CPU and memory requests/limits
- Use resource-based HPA metrics
- Monitor resource usage patterns

### 2. **Health Checks**
- Implement comprehensive health checks
- Use separate liveness and readiness probes
- Set appropriate probe timeouts and thresholds

### 3. **Service Discovery**
- Use professional naming conventions
- Implement circuit breakers for resilience
- Monitor service availability

### 4. **Monitoring**
- Enable metrics collection
- Set up alerting for scaling events
- Monitor service performance

### 5. **Security**
- Use TLS for production services
- Implement proper authentication
- Secure service-to-service communication

## Troubleshooting

### Common Issues

1. **Services Not Scaling**
   - Check HPA configuration
   - Verify resource metrics are available
   - Ensure proper resource requests/limits

2. **Health Check Failures**
   - Verify health check endpoints
   - Check service responsiveness
   - Review health check timeouts

3. **Service Discovery Issues**
   - Verify Kubernetes service configuration
   - Check service labels and selectors
   - Ensure proper namespace configuration

### Debugging

```csharp
// Enable detailed logging
services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Debug);
});

// Check service discovery status
var stats = serviceDiscovery.GetDiscoveryStatistics();
var health = healthCheckService.GetDetailedHealthStatus();
```

## Examples

See the complete examples in:
- `examples/006-kubernetes-auto-scaling-grpc-example.cs`
- `examples/007-kubernetes-deployment-example.yaml`

## Support

For questions and support:
- Check the main README.md for general usage
- Review the examples directory for implementation patterns
- Refer to Kubernetes documentation for HPA/VPA configuration
