namespace Convex.Shared.Models.DTOs;

/// <summary>
/// Validation result DTO
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// Whether validation passed
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Error message if validation failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// List of validation errors
    /// </summary>
    public List<string> Errors { get; set; } = new List<string>();

    /// <summary>
    /// Creates a successful validation result
    /// </summary>
    public static ValidationResult Success()
    {
        return new ValidationResult { IsValid = true };
    }

    /// <summary>
    /// Creates a failed validation result
    /// </summary>
    /// <param name="errorMessage">Error message</param>
    public static ValidationResult Failure(string errorMessage)
    {
        return new ValidationResult
        {
            IsValid = false,
            ErrorMessage = errorMessage,
            Errors = new List<string> { errorMessage }
        };
    }

    /// <summary>
    /// Creates a failed validation result with multiple errors
    /// </summary>
    /// <param name="errors">List of errors</param>
    public static ValidationResult Failure(List<string> errors)
    {
        return new ValidationResult
        {
            IsValid = false,
            Errors = errors,
            ErrorMessage = errors.FirstOrDefault()
        };
    }
}