namespace DealMatcher.Backend.Web.Endpoints.Offers;

public class UpdateStatus(IMediator mediator) : Endpoint<UpdateOfferStatusRequest>
{
    public override void Configure()
    {
        Version(1);
        Put("/offers/{OfferId:int}/status");
        Summary(s =>
        {
            s.Summary = "Update offer status";
            s.Description = "Changes offer status";
        });
    }

    public override async Task HandleAsync(UpdateOfferStatusRequest req, CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                        ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var request = new UpdateOfferStatusCommand(req.OfferId, userId, req.Status);
        var result = await mediator.Send(request, ct);
        await result.SendResult(this, ct);
    }
}
