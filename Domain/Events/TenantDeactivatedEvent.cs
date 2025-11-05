// src/Domain/Events/TenantDeactivatedEvent.cs
namespace SaaS.MaundCalculator.Domain.Events;

public record TenantDeactivatedEvent(Guid TenantId, string Reason) : IDomainEvent;