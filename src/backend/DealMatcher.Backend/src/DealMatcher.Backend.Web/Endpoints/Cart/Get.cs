using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DealMatcher.Backend.Core.Aggregates.Cart.DTOs;
using DealMatcher.Backend.UseCases.Features.Cart.Get;

namespace DealMatcher.Backend.Web.Endpoints.Cart;

public class Get(IMediator mediator) : EndpointWithoutRequest<List<CartItemDTO>>
{
    public override void Configure()
    {
        Version(1);
        Get("/cart/items");
        Summary(s =>
        {
            s.Summary = "Get cart contents";
            s.Description = "Returns all items in the authenticated user's cart";
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

        var request = new GetMyCartQuery(userId);
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct: ct);
    }
}
