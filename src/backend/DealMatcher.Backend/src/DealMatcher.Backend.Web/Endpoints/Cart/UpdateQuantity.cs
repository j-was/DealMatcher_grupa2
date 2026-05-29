using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DealMatcher.Backend.Core.Aggregates.User;
using DealMatcher.Backend.UseCases.Features.Cart.UpdateQuantity;

namespace DealMatcher.Backend.Web.Endpoints.Cart;

public class UpdateQuantity(IMediator mediator) : Endpoint<UpdateCartItemQuantityRequest>
{
    public override void Configure()
    {
        Version(1);
        Patch("/cart/items/{CartItemId:int}");
        Summary(s =>
        {
            s.Summary = "Update cart item quantity";
            s.Description = "Changes the quantity of a specific cart item";
        });
    }

    public override async Task HandleAsync(UpdateCartItemQuantityRequest req, CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                        ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var request = new UpdateCartItemQuantityCommand(req.CartItemId, userId, req.Quantity);
        var result = await mediator.Send(request, ct);
        await result.SendResult(this, ct);
    }
}
