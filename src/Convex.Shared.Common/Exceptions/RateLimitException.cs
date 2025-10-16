using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convex.Shared.Common.Exceptions
{
    /// <summary>
    /// Rate limit exceeded exception
    /// </summary>
    public class RateLimitException : ConvexException
    {
        public int Limit { get; }
        public int Remaining { get; }
        public DateTime ResetTime { get; }
        public int RetryAfter { get; }

        public RateLimitException(
            int limit,
            int remaining,
            DateTime resetTime,
            int retryAfter,
            string? correlationId = null)
            : base("RATE_LIMIT", "Rate limit exceeded", "Too many requests. Please try again later.",
                   new { Limit = limit, Remaining = remaining, ResetTime = resetTime, RetryAfter = retryAfter },
                   null, correlationId)
        {
            Limit = limit;
            Remaining = remaining;
            ResetTime = resetTime;
            RetryAfter = retryAfter;
        }

        // Helper constructor with TimeSpan for retry after
        public RateLimitException(
            int limit,
            int remaining,
            DateTime resetTime,
            TimeSpan retryAfter,
            string? correlationId = null)
            : this(limit, remaining, resetTime, (int)retryAfter.TotalSeconds, correlationId)
        {
        }

        // Helper constructor for common rate limiting scenarios
        public RateLimitException(
            int limit,
            int remaining,
            DateTime resetTime,
            string? correlationId = null)
            : this(limit, remaining, resetTime, (int)(resetTime - DateTime.UtcNow).TotalSeconds, correlationId)
        {
        }
    }
}