using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DealMatcher.Backend.UseCases.Features.Cart.Delete;

namespace DealMatcher.Backend.Web.Endpoints.Cart;

public sealed class Delete(IMediator mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Version(1);
        Delete("/cart/items/{cartItemId:int}");

        Summary(s =>
        {
            s.Summary = "Remove item from cart";
            s.Description = "Removes a specific item from the user's cart";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userIdRaw =
            User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var cartItemId = Route<int>("cartItemId");

        var result = await mediator.Send(new DeleteCartItemCommand(userId, cartItemId), ct);
        await result.SendResult(this, ct: ct);
    }
}
