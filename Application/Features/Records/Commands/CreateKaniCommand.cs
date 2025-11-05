// src/Application/Features/Records/Commands/CreateKaniCommand.cs
using MediatR;
using SaaS.MaundCalculator.Application.Common.Interfaces;

namespace SaaS.MaundCalculator.Application.Features.Records.Commands;

public record CreateKaniCommand(decimal WeightKg, int PersonId)
    : IRequest<CreateKaniResponse>, ITransactionalRequest;

public record CreateKaniResponse(Guid RecordId, decimal MaundEquivalent);

public class CreateKaniCommandHandler : IRequestHandler<CreateKaniCommand, CreateKaniResponse>
{
    private readonly ITenantRepository<KaniRecord> _repo;
    private readonly ITenantContext _tenant;
    private readonly ISubscriptionService _subscription;

    public CreateKaniCommandHandler(
        ITenantRepository<KaniRecord> repo,
        ITenantContext tenant,
        ISubscriptionService subscription)
    {
        _repo = repo;
        _tenant = tenant;
        _subscription = subscription;
    }

    public async Task<CreateKaniResponse> Handle(CreateKaniCommand request, CancellationToken ct)
    {
        await _subscription.EnforceLimitsAsync(_tenant.TenantId, ct);

        var record = KaniRecord.Create(_tenant.TenantId, request.PersonId, request.WeightKg, "system");
        await _repo.AddAsync(record, ct);
        await _repo.UnitOfWork.SaveChangesAsync(ct);

        return new CreateKaniResponse(record.Id, record.ConvertToMaund());
    }
}