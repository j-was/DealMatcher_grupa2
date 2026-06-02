namespace DealMatcher.Backend.Web.Endpoints.Offers;

public class Delete(IMediator mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Version(1);
        Delete("/offers/{OfferId:int}");
        Summary(s =>
        {
            s.Summary = "Delete an offer";
            s.Description = "Soft-deletes an offer";
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

        var offerId = Route<int>("OfferId");

        var request = new DeleteOfferCommand(offerId, userId);
        var result = await mediator.Send(request, ct);
        if (result.IsSuccess)
        {
            await SendNoContentAsync(ct);
            return;
        }

        await result.SendResult(this, ct);
    }
}
