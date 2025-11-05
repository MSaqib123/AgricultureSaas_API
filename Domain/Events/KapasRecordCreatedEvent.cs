// src/Domain/Events/KapasRecordCreatedEvent.cs
namespace SaaS.MaundCalculator.Domain.Events;

public record KapasRecordCreatedEvent(
    Guid RecordId,
    decimal WeightKg,
    Guid TenantId) : IDomainEvent;