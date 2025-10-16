using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convex.Shared.Common.Exceptions
{
    /// <summary>
    /// Unauthorized access exception
    /// </summary>
    public class UnauthorizedException : ConvexException
    {
        public UnauthorizedException(
            string message,
            string userMessage,
            object? details = null,
            string[]? suggestions = null,
            string? correlationId = null)
            : base("AUTH_001", message, userMessage, details, suggestions, correlationId)
        {
        }

        // Helper constructor for common unauthorized scenarios
        public UnauthorizedException(
            string userMessage,
            string? correlationId = null)
            : this("Unauthorized access", userMessage, null, null, correlationId)
        {
        }

        // Helper constructor with resource-specific details
        public UnauthorizedException(
            string resourceType,
            string resourceId,
            string action,
            string? correlationId = null)
            : this(
                $"Unauthorized to perform {action} on {resourceType} with ID {resourceId}",
                $"You don't have permission to {action} this {resourceType.ToLower()}",
                new { ResourceType = resourceType, ResourceId = resourceId, Action = action },
                new[] { "Contact your administrator for access permissions", "Verify you're logged in with the correct account" },
                correlationId)
        {
        }

       
    }
}