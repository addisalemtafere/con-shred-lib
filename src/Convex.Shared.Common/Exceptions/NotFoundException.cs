using System.Text.Json.Serialization;

namespace Convex.Shared.Common.Exceptions
{
    /// <summary>
    /// Resource not found exception
    /// </summary>
    public class NotFoundException : ConvexException
    {
        public string ResourceType { get; }
        public string ResourceId { get; }

        public NotFoundException(
            string resourceType,
            string resourceId,
            string message,
            string userMessage,
            string? correlationId = null)
            : base("NOT_FOUND", message, userMessage, new { ResourceType = resourceType, ResourceId = resourceId }, null, correlationId)
        {
            ResourceType = resourceType;
            ResourceId = resourceId;
        }

        // JSON constructor for deserialization (optional)
        [JsonConstructor]
        protected NotFoundException() : base()
        {
            ResourceType = string.Empty;
            ResourceId = string.Empty;
        }
    }
}