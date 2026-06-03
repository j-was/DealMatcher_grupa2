namespace DealMatcher.Backend.Web.Endpoints.Purchases;

public class Complete(IMediator mediator)
    : EndpointWithoutRequest
{
    public override void Configure()
    {
        AllowAnonymous();
        Version(1);
        Delete("/purchases/complete/{UserId:int}");
        Summary(s =>
        {
            s.Summary = "Completes purchase";
            s.Description = "Completes purchase and removing purchasing offer from the cart";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = Route<int>("UserId");

        if (userId <= 0)
        {
            await SendErrorsAsync(400, ct);
            return;
        }

        var request = new CompletePurchaseCommand(userId);
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct);
    }
}
