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
        public const string Auth = "auth-service";
        public const string Users = "user-service";
        public const string Betting = "betting-service";
        public const string Payments = "payment-service";
        public const string Games = "game-service";
        public const string Notifications = "notification-service";
        
        // Admin services
        public const string Admin = "admin-service";
        
        // System services
        public const string Health = "health-service";
        public const string Metrics = "metrics-service";
    }

    /// <summary>
    /// gRPC port configuration
    /// </summary>
    public static class Ports
    {
        public const int Default = 50051;
        public const int Auth = 50051;
        public const int Users = 50052;
        public const int Betting = 50053;
        public const int Payments = 50054;
        public const int Games = 50055;
        public const int Notifications = 50056;
        public const int Admin = 50057;
    }

    /// <summary>
    /// gRPC service discovery configuration
    /// </summary>
    public static class ServiceDiscovery
    {
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
