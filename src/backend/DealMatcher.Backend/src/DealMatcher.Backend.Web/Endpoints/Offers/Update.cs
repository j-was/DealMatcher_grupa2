using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DealMatcher.Backend.Core.Aggregates.User;
using DealMatcher.Backend.UseCases.Features.Offer.Update;

namespace DealMatcher.Backend.Web.Endpoints.Offers;

public class Update(IMediator mediator) : Endpoint<UpdateOfferRequest>
{
    public override void Configure()
    {
        Version(1);
        Patch("/offers/{OfferId:int}");
        Summary(s =>
        {
            s.Summary = "Update an offer";
            s.Description = "Updates an existing offer (returns to DRAFT status for revalidation)";
        });
    }

    public override async Task HandleAsync(UpdateOfferRequest req, CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                        ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var request = new UpdateOfferCommand(
            req.OfferId,
            userId,
            req.Title,
            req.Description,
            req.Price,
            req.Images,
            req.Tags,
            req.Properties,
            req.Availability);

        var result = await mediator.Send(request, ct);
        await result.SendResult(this, ct);
    }
}
