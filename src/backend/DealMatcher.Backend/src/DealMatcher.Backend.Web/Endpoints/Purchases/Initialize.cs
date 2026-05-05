namespace DealMatcher.Backend.Web.Endpoints.Purchases;

public class Initialize(IMediator mediator) : Endpoint<InitializePurchaseRequest>
{
    public override void Configure()
    {
        Version(1);
        Post("/purchases/initialize");
        Summary(s =>
        {
            s.Summary = "Initialize purchase";
            s.Description = "Creates an order and redirects to external payment provider";
        });
    }

    public override async Task HandleAsync(InitializePurchaseRequest req, CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                        ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var request = new AddToCartCommand(userId, req.OfferId, req.Quantity);
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct);
    }
}
