// src/Application/Features/Records/Commands/SoftDeleteKapasCommand.cs
using MediatR;

namespace SaaS.MaundCalculator.Application.Features.Records.Commands;

public record SoftDeleteKapasCommand(Guid RecordId) : IRequest<Unit>;

public class SoftDeleteKapasCommandHandler : IRequestHandler<SoftDeleteKapasCommand, Unit>
{
    private readonly ITenantRepository<KapasRecord> _repo;
    private readonly ITenantContext _tenant;

    public SoftDeleteKapasCommandHandler(ITenantRepository<KapasRecord> repo, ITenantContext tenant)
    {
        _repo = repo;
        _tenant = tenant;
    }

    public async Task<Unit> Handle(SoftDeleteKapasCommand request, CancellationToken ct)
    {
        var record = await _repo.GetByIdAsync(request.RecordId, ct)
            ?? throw new NotFoundException("Record not found.");

        if (record.TenantId != _tenant.TenantId)
            throw new ForbiddenAccessException();

        record.SoftDelete("user");
        await _repo.UnitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}