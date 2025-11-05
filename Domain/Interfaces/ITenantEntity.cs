// src/Domain/Entities/ITenantEntity.cs
namespace Domain.Entities;

public interface ITenantEntity
{
    Guid TenantId { get; }
}