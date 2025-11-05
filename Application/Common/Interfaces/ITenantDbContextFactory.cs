// src/Application/Common/Interfaces/ITenantDbContextFactory.cs
using SaaS.MaundCalculator.Infrastructure.Persistence;

namespace SaaS.MaundCalculator.Application.Common.Interfaces;

public interface ITenantDbContextFactory
{
    TenantDbContext CreateDbContext();
}