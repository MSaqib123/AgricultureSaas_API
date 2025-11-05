// src/Application/Common/Interfaces/ISubscriptionService.cs
namespace SaaS.MaundCalculator.Application.Common.Interfaces;

public interface ISubscriptionService
{
    Task EnforceLimitsAsync(Guid tenantId, CancellationToken ct = default);
    Task<bool> IsActiveAsync(Guid tenantId, CancellationToken ct = default);
}