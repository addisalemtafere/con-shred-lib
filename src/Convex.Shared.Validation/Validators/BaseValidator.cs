using FluentValidation;

namespace Convex.Shared.Validation.Validators;

/// <summary>
/// Base validator class for Convex entities
/// </summary>
/// <typeparam name="T">The type to validate</typeparam>
public abstract class BaseValidator<T> : AbstractValidator<T>
{
    /// <summary>
    /// Validates email format
    /// </summary>
    /// <param name="email">The email to validate</param>
    /// <returns>True if valid email</returns>
    protected bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        return System.Text.RegularExpressions.Regex.IsMatch(email, 
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    /// <summary>
    /// Validates phone number format
    /// </summary>
    /// <param name="phone">The phone number to validate</param>
    /// <returns>True if valid phone number</returns>
    protected bool IsValidPhoneNumber(string phone)
    {
        if (string.IsNullOrEmpty(phone))
            return false;

        return System.Text.RegularExpressions.Regex.IsMatch(phone, 
            @"^\+?[1-9]\d{1,14}$");
    }

    /// <summary>
    /// Validates password strength
    /// </summary>
    /// <param name="password">The password to validate</param>
    /// <returns>True if strong password</returns>
    protected bool IsStrongPassword(string password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < 8)
            return false;

        return System.Text.RegularExpressions.Regex.IsMatch(password, 
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]");
    }
}
