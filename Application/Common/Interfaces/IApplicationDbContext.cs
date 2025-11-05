// src/Application/Common/Interfaces/IApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using SaaS.MaundCalculator.Domain.Entities;

namespace SaaS.MaundCalculator.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Tenant> Tenants { get; }
    DbSet<AppUser> Users { get; }
    DbSet<Subscription> Subscriptions { get; }
    DbSet<KapasRecord> KapasRecords { get; }
    DbSet<KaniRecord> KaniRecords { get; }
    DbSet<Person> Persons { get; }
    DbSet<Family> Families { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}