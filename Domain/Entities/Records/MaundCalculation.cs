// src/Domain/Entities/Records/MaundCalculation.cs
namespace Domain.Entities.Records;

public class MaundCalculation : Entity<Guid>, ITenantEntity
{
    public Guid TenantId { get; private set; }
    public int PersonId { get; private set; }
    public decimal TotalKapasMaund { get; private set; }
    public decimal TotalKaniMaund { get; private set; }
    public decimal GrandTotalMaund { get; private set; }
    public DateTime CalculatedAt { get; private set; } = DateTime.UtcNow;

    private MaundCalculation() { }

    public static MaundCalculation Create(
        Guid tenantId,
        int personId,
        decimal kapasMaund,
        decimal kaniMaund)
    {
        return new MaundCalculation
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            PersonId = personId,
            TotalKapasMaund = kapasMaund,
            TotalKaniMaund = kaniMaund,
            GrandTotalMaund = kapasMaund + kaniMaund
        };
    }
}