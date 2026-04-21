using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DealMatcher.Backend.UseCases.Features.User.Get;

namespace DealMatcher.Backend.Web.Endpoints.Users;

public class Get(IMediator mediator) : EndpointWithoutRequest<UserDTO>
{
    public override void Configure()
    {
        Version(1);
        Get("/users/me");
        Summary(s =>
        {
            s.Summary = "Get current user profile";
            s.Description = "Returns profile information for the authenticated user";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userIdFromClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdFromClaim, out var userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var request = new GetUserQuery(userId);
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct: ct);
    }
}
