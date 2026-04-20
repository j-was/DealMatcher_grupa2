using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DealMatcher.Backend.UseCases.Features.User.Delete;

namespace DealMatcher.Backend.Web.Endpoints.Users;

public sealed class MeDelete(IMediator mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Version(1);
        Delete("/users/me");

        Summary(s =>
        {
            s.Summary = "Delete my account";
            s.Description = "Deactivates the account associated with logged in user";
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

        var result = await mediator.Send(new DeleteCurrentUserCommand(userId), ct);
        await result.SendResult(this, ct: ct);
    }
}
