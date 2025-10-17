namespace Convex.Shared.Common.Constants;

/// <summary>
/// Service base routes for load balancing and service discovery
/// </summary>
public static class ApiRoutes
{
    /// <summary>
    /// Base API versioning
    /// </summary>
    public static class Base
    {
        public const string Api = "/api";
        public const string V1 = "/api/v1";
        public const string V2 = "/api/v2";
    }

    /// <summary>
    /// Service base routes for load balancing
    /// </summary>
    public static class Services
    {
        // Core business services
        public const string Auth = "/api/v1/auth";

        public const string Users = "/api/v1/users";
        public const string Betting = "/api/v1/betting";
        public const string Payments = "/api/v1/payments";
        public const string Games = "/api/v1/games";
        public const string Notifications = "/api/v1/notifications";

        // Admin services
        public const string Admin = "/api/v1/admin";

        // System services
        public const string Health = "/health";

        public const string Metrics = "/metrics";
    }

    /// <summary>
    /// Load balancer configuration
    /// </summary>
    public static class LoadBalancer
    {
        public static readonly string[] ServiceRoutes = new[]
        {
            Services.Auth,
            Services.Users,
            Services.Betting,
            Services.Payments,
            Services.Games,
            Services.Notifications,
            Services.Admin
        };
    }

    /// <summary>
    /// Service discovery endpoints
    /// </summary>
    public static class ServiceDiscovery
    {
        public static readonly Dictionary<string, string> ServiceEndpoints = new()
        {
            { "AuthService", Services.Auth },
            { "UserService", Services.Users },
            { "BettingService", Services.Betting },
            { "PaymentService", Services.Payments },
            { "GameService", Services.Games },
            { "NotificationService", Services.Notifications },
            { "AdminService", Services.Admin }
        };
    }
}