namespace DealMatcher.Backend.Web.Endpoints.Purchases;

public class Complete(IMediator mediator)
    : EndpointWithoutRequest
{
    public override void Configure()
    {
        Version(1);
        Delete("/purchases/complete");
        Summary(s =>
        {
            s.Summary = "Completes purchase";
            s.Description = "Completes purchase and removing purchasing offer from the cart";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                        ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var request = new CompletePurchaseCommand(userId);
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct);
    }
}
