using Convex.Shared.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Convex.Shared.Models.DTOs;

/// <summary>
/// Transaction data transfer object for API communication
/// </summary>
public class TransactionDto
{
    /// <summary>
    /// Transaction unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User who initiated the transaction
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Transaction reference number
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Reference { get; set; } = string.Empty;

    /// <summary>
    /// Transaction type
    /// </summary>
    public TransactionType Type { get; set; }

    /// <summary>
    /// Transaction amount
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Transaction fee
    /// </summary>
    public decimal Fee { get; set; } = 0;

    /// <summary>
    /// Net amount (after fees)
    /// </summary>
    public decimal NetAmount { get; set; }

    /// <summary>
    /// Balance before transaction
    /// </summary>
    public decimal BalanceBefore { get; set; }

    /// <summary>
    /// Balance after transaction
    /// </summary>
    public decimal BalanceAfter { get; set; }

    /// <summary>
    /// Transaction status
    /// </summary>
    public PaymentStatus Status { get; set; }

    /// <summary>
    /// Payment provider used
    /// </summary>
    public PaymentProvider? Provider { get; set; }

    /// <summary>
    /// External transaction ID from provider
    /// </summary>
    [StringLength(100)]
    public string? ExternalTransactionId { get; set; }

    /// <summary>
    /// Transaction description
    /// </summary>
    [StringLength(200)]
    public string? Description { get; set; }

    /// <summary>
    /// Related ticket ID (for bet transactions)
    /// </summary>
    public Guid? TicketId { get; set; }

    /// <summary>
    /// Processed date
    /// </summary>
    public DateTime? ProcessedAt { get; set; }

    /// <summary>
    /// Failure reason (if failed)
    /// </summary>
    [StringLength(500)]
    public string? FailureReason { get; set; }

    /// <summary>
    /// Additional metadata
    /// </summary>
    public string? Metadata { get; set; }

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}