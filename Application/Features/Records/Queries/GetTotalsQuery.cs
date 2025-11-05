// src/Application/Features/Records/Queries/GetTotalsQuery.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS.MaundCalculator.Application.Common.Interfaces;

namespace SaaS.MaundCalculator.Application.Features.Records.Queries;

public record GetTotalsQuery : IRequest<TotalsDto>;

public record TotalsDto(decimal TotalKapasMaund, decimal TotalKaniMaund, decimal GrandTotal);

public class GetTotalsQueryHandler : IRequestHandler<GetTotalsQuery, TotalsDto>
{
    private readonly ITenantDbContextFactory _factory;

    public GetTotalsQueryHandler(ITenantDbContextFactory factory) => _factory = factory;

    public async Task<TotalsDto> Handle(GetTotalsQuery request, CancellationToken ct)
    {
        using var ctx = _factory.CreateDbContext();

        var kapas = await ctx.KapasRecords
            .Where(x => !x.IsDeleted)
            .SumAsync(x => x.WeightKg, ct);

        var kani = await ctx.KaniRecords
            .Where(x => !x.IsDeleted)
            .SumAsync(x => x.WeightKg, ct);

        return new TotalsDto(
            TotalKapasMaund: kapas / 40m,
            TotalKaniMaund: kani / 40m * 0.8m,
            GrandTotal: (kapas / 40m) + (kani / 40m * 0.8m)
        );
    }
}