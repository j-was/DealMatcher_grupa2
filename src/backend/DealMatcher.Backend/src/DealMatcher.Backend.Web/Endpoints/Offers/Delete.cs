using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DealMatcher.Backend.Core.Aggregates.User;
using DealMatcher.Backend.UseCases.Features.Offer.Delete;

namespace DealMatcher.Backend.Web.Endpoints.Offers;

public class Delete(IMediator mediator) : Endpoint<DeleteOfferRequest>
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

    public override async Task HandleAsync(DeleteOfferRequest req, CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                        ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var request = new DeleteOfferCommand(req.OfferId, userId);
        var result = await mediator.Send(request, ct);
        await result.SendResult(this, ct);
    }
}
