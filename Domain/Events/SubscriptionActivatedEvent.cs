// src/Domain/Events/SubscriptionActivatedEvent.cs
namespace SaaS.MaundCalculator.Domain.Events;

public record SubscriptionActivatedEvent(Guid TenantId, Guid SubscriptionId) : IDomainEvent;