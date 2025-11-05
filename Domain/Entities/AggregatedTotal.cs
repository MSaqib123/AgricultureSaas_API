// src/Domain/Entities/AggregatedTotal.cs
namespace Domain.Entities;

public class AggregatedTotal : Entity<Guid>
{
    public Guid TenantId { get; private set; }
    public decimal TotalMaund { get; private set; }
    public DateTime AsOfDate { get; private set; }

    private AggregatedTotal() { }

    public static AggregatedTotal Create(Guid tenantId, decimal totalMaund)
    {
        return new AggregatedTotal
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            TotalMaund = totalMaund,
            AsOfDate = DateTime.UtcNow.Date
        };
    }
}