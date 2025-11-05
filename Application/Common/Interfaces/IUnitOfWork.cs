// src/Application/Common/Interfaces/IUnitOfWork.cs
using System.Data;

namespace SaaS.MaundCalculator.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<IDbTransaction> BeginTransactionAsync(CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}