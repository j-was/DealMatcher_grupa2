using DealMatcher.Backend.UseCases.Features.Admin.GetOffers;

namespace DealMatcher.Backend.Web.Endpoints.Admin;

public class GetOffers(IMediator mediator) : Endpoint<GetOffersRequest>
{
    public override void Configure()
    {
        Version(1);
        Get("/admin/offers");
        Summary(s =>
        {
            s.Summary = "Get offers for admin";
            s.Description = "Returns paginated offers list for admin";
        });
    }

    public override async Task HandleAsync(GetOffersRequest req, CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                        ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var request = new GetOffersQuery(req.Page, req.Limit, req.Status, userId);
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct);
    }
}
