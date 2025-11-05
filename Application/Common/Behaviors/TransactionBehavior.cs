// src/Application/Common/Behaviors/TransactionBehavior.cs
using MediatR;
using SaaS.MaundCalculator.Application.Common.Interfaces;
using System.Data;

namespace SaaS.MaundCalculator.Application.Common.Behaviors;

public class TransactionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITransactionalRequest
{
    private readonly IUnitOfWork _uow;

    public TransactionBehavior(IUnitOfWork uow) => _uow = uow;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        using var transaction = await _uow.BeginTransactionAsync(ct);
        try
        {
            var response = await next();
            await _uow.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return response;
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}