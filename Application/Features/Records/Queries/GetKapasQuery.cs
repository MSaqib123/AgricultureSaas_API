// src/Application/Features/Records/Queries/GetKapasQuery.cs
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS.MaundCalculator.Application.Common.Behaviors;
using SaaS.MaundCalculator.Application.Common.Interfaces;

namespace SaaS.MaundCalculator.Application.Features.Records.Queries;

public record GetKapasQuery : IRequest<List<KapasRecordDto>>, ICacheableRequest
{
    public string CacheKey => "all_kapas_records";
    public int CacheDurationMinutes => 5;
}

public record KapasRecordDto(Guid Id, int PersonId, decimal WeightKg, decimal Maund, DateTime RecordedAt);

public class GetKapasQueryHandler : IRequestHandler<GetKapasQuery, List<KapasRecordDto>>
{
    private readonly ITenantDbContextFactory _factory;
    private readonly IMapper _mapper;

    public GetKapasQueryHandler(ITenantDbContextFactory factory, IMapper mapper)
    {
        _factory = factory;
        _mapper = mapper;
    }

    public async Task<List<KapasRecordDto>> Handle(GetKapasQuery request, CancellationToken ct)
    {
        using var context = _factory.CreateDbContext();
        var records = await context.KapasRecords
            .AsNoTracking()
            .OrderByDescending(x => x.RecordedAt)
            .ToListAsync(ct);

        return _mapper.Map<List<KapasRecordDto>>(records);
    }
}