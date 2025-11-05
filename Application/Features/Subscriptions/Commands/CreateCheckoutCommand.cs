// src/Application/Features/Subscriptions/Commands/CreateCheckoutCommand.cs
using MediatR;
using Stripe.Checkout;

namespace Application.Features.Subscriptions.Commands;

public record CreateCheckoutCommand(string PlanId) : IRequest<string>;

public class CreateCheckoutCommandHandler : IRequestHandler<CreateCheckoutCommand, string>
{
    private readonly ITenantContext _tenant;
    private readonly IConfiguration _config;

    public CreateCheckoutCommandHandler(ITenantContext tenant, IConfiguration config)
    {
        _tenant = tenant;
        _config = config;
    }

    public Task<string> Handle(CreateCheckoutCommand request, CancellationToken ct)
    {
        var domain = _config["App:Domain"] ?? "http://localhost:5000";
        var session = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    Price = request.PlanId,
                    Quantity = 1
                }
            },
            Mode = "subscription",
            SuccessUrl = $"{domain}/subscription/success?session_id={{CHECKOUT_SESSION_ID}}",
            CancelUrl = $"{domain}/subscription/cancel",
            Metadata = new Dictionary<string, string>
            {
                ["tenant_id"] = _tenant.TenantId.ToString()
            }
        };

        var service = new SessionService();
        var checkoutSession = service.Create(session);
        return Task.FromResult(checkoutSession.Url);
    }
}