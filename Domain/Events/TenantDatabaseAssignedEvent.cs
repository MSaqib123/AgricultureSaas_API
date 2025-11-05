// src/Domain/Events/rnz5UXKNZN4yKaBqkuBnEQF4aTS8N1j6TL.cs
namespace SaaS.MaundCalculator.Domain.Events;

public record TenantDatabaseAssignedEvent(Guid TenantId, string DatabaseName) : IDomainEvent;