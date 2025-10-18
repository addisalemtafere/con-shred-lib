using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Convex.Shared.Common.Exceptions
{
    /// <summary>
    /// Business logic exception for domain-specific errors
    /// </summary>
    [Serializable]
    public class BusinessException : ConvexException
    {
        public BusinessException(
            string errorCode,
            string message,
            string userMessage,
            object? details = null,
            string[]? suggestions = null,
            string? correlationId = null,
            Exception? innerException = null)
            : base(errorCode, message, userMessage, details, suggestions, correlationId, innerException)
        {
        }

        // For JSON serialization/deserialization
        [JsonConstructor]
        protected BusinessException() : base() { }
    }
}
