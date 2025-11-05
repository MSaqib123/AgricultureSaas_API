// src/Application/Common/Interfaces/IRepository.cs
namespace SaaS.MaundCalculator.Application.Common.Interfaces;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity, CancellationToken ct = default);
    Task UpdateAsync(T entity, CancellationToken ct = default);
    Task DeleteAsync(T entity, CancellationToken ct = default);
    Task<T?> GetByIdAsync<TKey>(TKey id, CancellationToken ct = default);
}