// src/Domain/Entities/Records/KapasRecord.cs
using Domain.Events;
using System;

namespace Domain.Entities.Records;

public class KapasRecord : Entity<Guid>, ITenantEntity, ISoftDeletable, IAuditable
{
    public Guid TenantId { get; private set; }
    public int PersonId { get; private set; }
    public Person Person { get; private set; } = null!;
    public decimal WeightKg { get; private set; }
    public DateTime RecordedAt { get; private set; } = DateTime.UtcNow;

    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public string CreatedBy { get; private set; } = null!;
    public DateTime? UpdatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private KapasRecord() { }

    public static KapasRecord Create(Guid tenantId, int personId, decimal weightKg, string createdBy)
    {
        if (weightKg <= 0) throw new ArgumentException("Weight must be positive.", nameof(weightKg));

        var record = new KapasRecord
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            PersonId = personId,
            WeightKg = weightKg,
            CreatedBy = createdBy
        };

        record.AddDomainEvent(new KapasRecordCreatedEvent(record.Id, weightKg, tenantId));
        return record;
    }

    public decimal ConvertToMaund() => WeightKg / 40m;

    public void SoftDelete(string deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = deletedBy;
        AddDomainEvent(new KapasRecordDeletedEvent(Id, TenantId));
    }

    private void AddDomainEvent(IDomainEvent @event)
        => _domainEvents.Add(@event);
}