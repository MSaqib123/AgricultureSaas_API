// src/Domain/Events/KapasRecordDeletedEvent.cs
namespace SaaS.MaundCalculator.Domain.Events;

public record KapasRecordDeletedEvent(Guid RecordId, Guid TenantId) : IDomainEvent;