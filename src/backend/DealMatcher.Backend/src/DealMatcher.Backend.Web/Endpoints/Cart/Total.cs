using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DealMatcher.Backend.Core.Aggregates.Cart.DTOs;
using DealMatcher.Backend.UseCases.Features.Cart.DTOs;
using DealMatcher.Backend.UseCases.Features.Cart.Total;
namespace DealMatcher.Backend.Web.Endpoints.Cart;

public class Total(IMediator mediator) : EndpointWithoutRequest<CartTotalDTO>
{
    public override void Configure()
    {
        Version(1);
        Get("/cart/total");
        Summary(s =>
        {
            s.Summary = "Get cart total";
            s.Description = "Returns the total price of all items in the cart";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userIdFromClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                              ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdFromClaim, out var userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var request = new GetCartTotalQuery(userId);
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct: ct);
    }
}
