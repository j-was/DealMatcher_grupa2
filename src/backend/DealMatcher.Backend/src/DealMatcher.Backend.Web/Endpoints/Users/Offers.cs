namespace DealMatcher.Backend.Web.Endpoints.Users;

public class MeOffers(IMediator mediator) : EndpointWithoutRequest<List<OfferDTO>>
{
    public override void Configure()
    {
        Get("/users/me/offers");
        Version(1);

        Summary(s =>
        {
            s.Summary = "Get my offers";
            s.Description = "Returns all offers posted by the authenticated user";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
                        User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var query = new GetMyOffersQuery(userId);
        var result = await mediator.Send(query, ct);

        await result.SendResult(this, ct: ct);
    }
}
