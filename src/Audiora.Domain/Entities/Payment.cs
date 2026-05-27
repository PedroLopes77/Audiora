using Audiora.Domain.Common;

namespace Audiora.Domain.Entities;

public class Payment : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public decimal Amount { get; set; }
    public string Currency { get; set; } = "BRL";
    public string Status { get; set; } = "Pending"; // Pending, Approved, Refused, Refunded
    public string? ExternalReference { get; set; }
    public string? PaymentMethod { get; set; } // CreditCard, Pix, Boleto
    public DateTime? PaidAt { get; set; }

    public Guid? SubscriptionId { get; set; }
    public Subscription? Subscription { get; set; }
}