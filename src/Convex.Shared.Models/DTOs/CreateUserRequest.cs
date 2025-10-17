using Convex.Shared.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Convex.Shared.Models.DTOs;

/// <summary>
/// Request DTO for creating a new user
/// </summary>
public class CreateUserRequest
{
    /// <summary>
    /// Phone number (primary identifier)
    /// </summary>
    [Required(ErrorMessage = "Phone number is required")]
    [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be between 10 and 15 characters")]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Full name of the user
    /// </summary>
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters")]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Email address (optional)
    /// </summary>
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email must not exceed 100 characters")]
    public string? Email { get; set; }

    /// <summary>
    /// User type (Better, Agent, Client, etc.)
    /// </summary>
    [Required(ErrorMessage = "User type is required")]
    public UserType UserType { get; set; }

    /// <summary>
    /// Agent ID (required if user type is Client)
    /// </summary>
    public Guid? AgentId { get; set; }

    /// <summary>
    /// Branch ID (optional)
    /// </summary>
    public Guid? BranchId { get; set; }

    /// <summary>
    /// Registration IP address
    /// </summary>
    [StringLength(45)]
    public string? RegistrationIp { get; set; }

    /// <summary>
    /// User preferences and settings (JSON string)
    /// </summary>
    public string? Preferences { get; set; }
}