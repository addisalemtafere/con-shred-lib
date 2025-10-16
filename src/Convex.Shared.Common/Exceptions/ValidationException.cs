using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convex.Shared.Common.Models;

namespace Convex.Shared.Common.Exceptions
{
    /// <summary>
    /// Validation exception for input validation errors
    /// </summary>
    public class ValidationException : ConvexException
    {
        public ValidationError[] ValidationErrors { get; }

        public ValidationException(
            string errorCode,
            string message,
            string userMessage,
            ValidationError[] validationErrors,
            string? correlationId = null)
            : base(errorCode, message, userMessage, null, null, correlationId)
        {
            ValidationErrors = validationErrors;
        }

        // Helper constructor with default error code
        public ValidationException(
            string message,
            string userMessage,
            ValidationError[] validationErrors,
            string? correlationId = null)
            : this("VALIDATION_ERROR", message, userMessage, validationErrors, correlationId)
        {
        }

        // Helper constructor for single validation error
        public ValidationException(
            string field,
            string code,
            string message,
            object? attemptedValue = null,
            object? constraint = null,
            string? correlationId = null)
            : this(
                "VALIDATION_ERROR",
                $"Validation failed for field '{field}': {message}",
                "Please correct the validation errors and try again",
                new[] {
                    new ValidationError {
                        Field = field,
                        Code = code,
                        Message = message,
                        AttemptedValue = attemptedValue,
                        Constraint = constraint
                    }
                },
                correlationId)
        {
        }

        // Helper constructor for multiple field errors with just field and message
        public ValidationException(
            Dictionary<string, string> fieldErrors,
            string? correlationId = null)
            : this(
                "VALIDATION_ERROR",
                "Multiple validation errors occurred",
                "Please correct the validation errors and try again",
                fieldErrors.Select(kvp => new ValidationError
                {
                    Field = kvp.Key,
                    Code = "VALIDATION_ERROR",
                    Message = kvp.Value
                }).ToArray(),
                correlationId)
        {
        }

        // Helper constructor for common validation scenarios
        public ValidationException(
            ValidationError[] validationErrors,
            string? correlationId = null)
            : this(
                "VALIDATION_ERROR",
                "Validation failed",
                "Please correct the validation errors and try again",
                validationErrors,
                correlationId)
        {
        }
    }
}