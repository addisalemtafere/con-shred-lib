using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Convex.Shared.Common.Models
{
    /// <summary>
    /// API response metadata
    /// </summary>
    public class ResponseMetadata
    {
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("requestId")]
        public string? RequestId { get; set; }

        [JsonPropertyName("version")]
        public string? Version { get; set; }

        [JsonPropertyName("correlationId")]
        public string? CorrelationId { get; set; }


    }
}
