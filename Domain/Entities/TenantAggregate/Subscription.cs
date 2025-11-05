// src/Domain/Entities/TenantAggregate/Subscription.cs
using Domain.ValueObjects;

namespace Domain.Entities.TenantAggregate;

public class Subscription : Entity<Guid>
{
    public Guid TenantId { get; private set; }
    public string PlanId { get; private set; } = null!;
    public Money Price { get; private set; } = null!;
    public DateTime StartsAt { get; private set; }
    public DateTime? EndsAt { get; private set; }
    public bool IsActive { get; private set; }

    private Subscription() { }

    public static Subscription Create(Guid tenantId, string planId, Money price)
    {
        var sub = new Subscription
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            PlanId = planId,
            Price = price,
            StartsAt = DateTime.UtcNow,
            IsActive = true
        };
        return sub;
    }

    public void Cancel()
    {
        IsActive = false;
        EndsAt = DateTime.UtcNow;
    }
}