// src/Domain/Entities/Family.cs
namespace SaaS.MaundCalculator.Domain.Entities;

public class Family : Entity<int>, ITenantEntity
{
    public Guid TenantId { get; private set; }
    public string FamilyName { get; private set; } = null!;
    private readonly List<Person> _members = new();
    public IReadOnlyCollection<Person> Members => _members.AsReadOnly();

    private Family() { }

    public static Family Create(Guid tenantId, string familyName)
    {
        return new Family
        {
            Id = 0,
            TenantId = tenantId,
            FamilyName = familyName.Trim()
        };
    }
}