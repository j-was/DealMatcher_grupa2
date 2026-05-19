namespace DealMatcher.Backend.Web.Endpoints.Admin;

public class GetOfferActivity(IMediator mediator) : Endpoint<GetOfferActivityRequest>
{
    public override void Configure()
    {
        Version(1);
        Get("/admin/activity/offer");
        Summary(s =>
        {
            s.Summary = "Get offer activity (admin)";
            s.Description = "Returns activity history for a specific offer";
        });
    }

    public override async Task HandleAsync(GetOfferActivityRequest req, CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                        ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var request = new GetOfferActivityQuery(req.OfferId, userId);
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct);
    }
}
