namespace Convex.Shared.Common.Constants;

/// <summary>
/// HTTP status codes and constants for Convex APIs
/// </summary>
public static class HttpConstants
{
    /// <summary>
    /// HTTP status codes
    /// </summary>
    public static class StatusCodes
    {
        // Success codes
        public const int OK = 200;

        public const int Created = 201;
        public const int NoContent = 204;

        // Client error codes
        public const int BadRequest = 400;

        public const int Unauthorized = 401;
        public const int Forbidden = 403;
        public const int NotFound = 404;
        public const int MethodNotAllowed = 405;
        public const int Conflict = 409;
        public const int UnprocessableEntity = 422;
        public const int TooManyRequests = 429;

        // Server error codes
        public const int InternalServerError = 500;

        public const int BadGateway = 502;
        public const int ServiceUnavailable = 503;
        public const int GatewayTimeout = 504;
    }

    /// <summary>
    /// HTTP methods
    /// </summary>
    public static class Methods
    {
        public const string GET = "GET";
        public const string POST = "POST";
        public const string PUT = "PUT";
        public const string PATCH = "PATCH";
        public const string DELETE = "DELETE";
        public const string HEAD = "HEAD";
        public const string OPTIONS = "OPTIONS";
    }

    /// <summary>
    /// Content types
    /// </summary>
    public static class ContentTypes
    {
        public const string Json = "application/json";
        public const string Xml = "application/xml";
        public const string FormUrlEncoded = "application/x-www-form-urlencoded";
        public const string MultipartFormData = "multipart/form-data";
        public const string TextPlain = "text/plain";
        public const string TextHtml = "text/html";
    }

    /// <summary>
    /// API versioning
    /// </summary>
    public static class ApiVersions
    {
        public const string V1 = "v1";
        public const string V2 = "v2";
        public const string V3 = "v3";
    }

    /// <summary>
    /// Common HTTP headers
    /// </summary>
    public static class Headers
    {
        public const string Authorization = "Authorization";
        public const string ContentType = "Content-Type";
        public const string Accept = "Accept";
        public const string UserAgent = "User-Agent";
        public const string CorrelationId = "X-Correlation-ID";
        public const string RequestId = "X-Request-ID";
        public const string ApiVersion = "X-API-Version";
        public const string RateLimitLimit = "X-RateLimit-Limit";
        public const string RateLimitRemaining = "X-RateLimit-Remaining";
        public const string RateLimitReset = "X-RateLimit-Reset";
        public const string RetryAfter = "Retry-After";
    }
}