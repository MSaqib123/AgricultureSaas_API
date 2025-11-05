// src/Application/Features/Records/Commands/CreateKapasCommand.cs
using FluentValidation;
using MediatR;
using SaaS.MaundCalculator.Application.Common.Interfaces;
using SaaS.MaundCalculator.Domain.Entities.Records;

namespace SaaS.MaundCalculator.Application.Features.Records.Commands;

public record CreateKapasCommand(
    decimal WeightKg,
    int PersonId,
    Guid? FamilyId = null
) : IRequest<CreateKapasResponse>, ITransactionalRequest;

public record CreateKapasResponse(Guid RecordId, decimal MaundEquivalent);

public class CreateKapasCommandValidator : AbstractValidator<CreateKapasCommand>
{
    public CreateKapasCommandValidator()
    {
        RuleFor(x => x.WeightKg).GreaterThan(0);
        RuleFor(x => x.PersonId).GreaterThan(0);
    }
}

public class CreateKapasCommandHandler : IRequestHandler<CreateKapasCommand, CreateKapasResponse>
{
    private readonly ITenantRepository<KapasRecord> _repo;
    private readonly ITenantContext _tenant;
    private readonly ISubscriptionService _subscription;

    public CreateKapasCommandHandler(
        ITenantRepository<KapasRecord> repo,
        ITenantContext tenant,
        ISubscriptionService subscription)
    {
        _repo = repo;
        _tenant = tenant;
        _subscription = subscription;
    }

    public async Task<CreateKapasResponse> Handle(CreateKapasCommand request, CancellationToken ct)
    {
        await _subscription.EnforceLimitsAsync(_tenant.TenantId, ct);

        var record = KapasRecord.Create(
            _tenant.TenantId,
            request.PersonId,
            request.WeightKg,
            "system"
        );

        await _repo.AddAsync(record, ct);
        await _repo.UnitOfWork.SaveChangesAsync(ct);

        return new CreateKapasResponse(record.Id, record.ConvertToMaund());
    }
}