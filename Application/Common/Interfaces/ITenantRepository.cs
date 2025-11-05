// src/Application/Common/Interfaces/ITenantRepository.cs
using SaaS.MaundCalculator.Domain.Entities;

namespace SaaS.MaundCalculator.Application.Common.Interfaces;

public interface ITenantRepository<T> : IRepository<T> where T : class, ITenantEntity
{
    IQueryable<T> Query();
}