// src/Application/Common/Interfaces/ITenantService.cs
using SaaS.MaundCalculator.Domain.Entities;

namespace SaaS.MaundCalculator.Application.Common.Interfaces;

public interface ITenantService
{
    Task CreateTenantAsync(Tenant tenant, AppUser owner, CancellationToken ct = default);
    Task AssignDatabaseToTenantAsync(Guid tenantId, string dbName, CancellationToken ct = default);
}