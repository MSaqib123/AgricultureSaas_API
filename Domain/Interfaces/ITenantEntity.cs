// src/Domain/Entities/ITenantEntity.cs
namespace SaaS.MaundCalculator.Domain.Entities;

public interface ITenantEntity
{
    Guid TenantId { get; }
}