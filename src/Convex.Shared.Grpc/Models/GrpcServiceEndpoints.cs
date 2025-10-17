namespace Convex.Shared.Grpc.Models;

/// <summary>
/// gRPC service endpoints for Convex microservices
/// </summary>
public static class GrpcServiceEndpoints
{
    /// <summary>
    /// gRPC service endpoints for load balancing and service discovery
    /// </summary>
    public static class Services
    {
        // Core business services
        /// <summary>
        /// Authentication service endpoint
        /// </summary>
        public const string Auth = "auth-service";

        /// <summary>
        /// User management service endpoint
        /// </summary>
        public const string Users = "user-service";
        
        /// <summary>
        /// Betting service endpoint
        /// </summary>
        public const string Betting = "betting-service";
        
        /// <summary>
        /// Payment processing service endpoint
        /// </summary>
        public const string Payments = "payment-service";
        
        /// <summary>
        /// Game management service endpoint
        /// </summary>
        public const string Games = "game-service";
        
        /// <summary>
        /// Notification service endpoint
        /// </summary>
        public const string Notifications = "notification-service";

        // Admin services
        /// <summary>
        /// Administrative service endpoint
        /// </summary>
        public const string Admin = "admin-service";

        // System services
        /// <summary>
        /// Health check service endpoint
        /// </summary>
        public const string Health = "health-service";

        /// <summary>
        /// Metrics collection service endpoint
        /// </summary>
        public const string Metrics = "metrics-service";
    }

    /// <summary>
    /// gRPC port configuration
    /// </summary>
    public static class Ports
    {
        /// <summary>
        /// Default gRPC port
        /// </summary>
        public const int Default = 50051;
        
        /// <summary>
        /// Authentication service port
        /// </summary>
        public const int Auth = 50051;
        
        /// <summary>
        /// User service port
        /// </summary>
        public const int Users = 50052;
        
        /// <summary>
        /// Betting service port
        /// </summary>
        public const int Betting = 50053;
        
        /// <summary>
        /// Payment service port
        /// </summary>
        public const int Payments = 50054;
        
        /// <summary>
        /// Game service port
        /// </summary>
        public const int Games = 50055;
        
        /// <summary>
        /// Notification service port
        /// </summary>
        public const int Notifications = 50056;
        
        /// <summary>
        /// Admin service port
        /// </summary>
        public const int Admin = 50057;
    }

    /// <summary>
    /// gRPC service discovery configuration
    /// </summary>
    public static class ServiceDiscovery
    {
        /// <summary>
        /// Dictionary mapping service names to their endpoints and ports
        /// </summary>
        public static readonly Dictionary<string, (string Service, int Port)> ServiceEndpoints = new()
        {
            { "AuthService", (Services.Auth, Ports.Auth) },
            { "UserService", (Services.Users, Ports.Users) },
            { "BettingService", (Services.Betting, Ports.Betting) },
            { "PaymentService", (Services.Payments, Ports.Payments) },
            { "GameService", (Services.Games, Ports.Games) },
            { "NotificationService", (Services.Notifications, Ports.Notifications) },
            { "AdminService", (Services.Admin, Ports.Admin) }
        };
    }
}