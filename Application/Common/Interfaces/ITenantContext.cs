// src/Application/Common/Interfaces/ITenantContext.cs
namespace SaaS.MaundCalculator.Application.Common.Interfaces;

public interface ITenantContext
{
    Guid TenantId { get; }
    void SetTenant(Guid tenantId);
}