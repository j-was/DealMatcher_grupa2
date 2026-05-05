namespace DealMatcher.Backend.Web.Endpoints.Cart;

public class Add(IMediator mediator) : Endpoint<AddToCartRequest>
{
    public override void Configure()
    {
        Version(1);
        Post("/cart/items");
        Summary(s =>
        {
            s.Summary = "Add item to cart";
            s.Description = "Adds an offer to the user's cart";
        });
    }

    public override async Task HandleAsync(AddToCartRequest req, CancellationToken ct)
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
