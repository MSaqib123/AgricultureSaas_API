// src/Domain/Entities/Records/KaniRecord.cs
using SaaS.MaundCalculator.Domain.Events;

namespace SaaS.MaundCalculator.Domain.Entities.Records;

public class KaniRecord : Entity<Guid>, ITenantEntity, ISoftDeletable, IAuditable
{
    public Guid TenantId { get; private set; }
    public int PersonId { get; private set; }
    public decimal WeightKg { get; private set; }
    public DateTime RecordedAt { get; private set; } = DateTime.UtcNow;

    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public string CreatedBy { get; private set; } = null!;

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public DateTime? UpdatedAt => throw new NotImplementedException();

    public string? UpdatedBy => throw new NotImplementedException();

    private KaniRecord() { }

    public static KaniRecord Create(Guid tenantId, int personId, decimal weightKg, string createdBy)
    {
        var record = new KaniRecord
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            PersonId = personId,
            WeightKg = weightKg,
            CreatedBy = createdBy
        };
        record.AddDomainEvent(new KaniRecordCreatedEvent(record.Id, weightKg));
        return record;
    }

    public decimal ConvertToMaund() => WeightKg / 40m * 0.8m; // Kani = 80% of Kapas

    public void SoftDelete(string deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        //AddDomainEvent(new KaniRecordDeletedEvent(Id));
    }

    private void AddDomainEvent(IDomainEvent @event)
        => _domainEvents.Add(@event);
}