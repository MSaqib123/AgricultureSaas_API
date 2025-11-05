// src/Domain/Entities/Person.cs
namespace Domain.Entities;

public class Person : Entity<int>, ITenantEntity
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = null!;
    public int? FamilyId { get; private set; }
    public Family? Family { get; private set; }

    private Person() { }

    public static Person Create(Guid tenantId, string name, int? familyId = null)
    {
        return new Person
        {
            Id = 0, // DB-generated
            TenantId = tenantId,
            Name = name.Trim(),
            FamilyId = familyId
        };
    }
}