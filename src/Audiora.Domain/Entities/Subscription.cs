using Audiora.Domain.Common;
using Audiora.Domain.Enums;

namespace Audiora.Domain.Entities;

public class Subscription : BaseEntity
{
    public SubscriptionPlan Plan { get; set; }
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool AutoRenew { get; set; } = true;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public bool IsExpired() => DateTime.UtcNow > ExpiresAt;

    public void Renew(int months = 1)
    {
        ExpiresAt = ExpiresAt.AddMonths(months);
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public static decimal GetPrice(SubscriptionPlan plan) => plan switch
    {
        SubscriptionPlan.Individual => 19.90m,
        SubscriptionPlan.Family => 29.90m,
        SubscriptionPlan.Student => 9.90m,
        _ => 0m
    };
}