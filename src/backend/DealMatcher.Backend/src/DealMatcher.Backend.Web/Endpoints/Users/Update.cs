using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DealMatcher.Backend.UseCases.Features.User.Update;

namespace DealMatcher.Backend.Web.Endpoints.Users;

public sealed class MeUpdate(IMediator mediator) : Endpoint<MeUpdateRequest, UserDTO>
{
    public override void Configure()
    {
        Version(1);
        Put("/users/me");

        Summary(s =>
        {
            s.Summary = "Update current user profile";
            s.Description = "Updates profile information for the authenticated user";
        });
    }

    public override async Task HandleAsync(MeUpdateRequest req, CancellationToken ct)
    {
        var userIdFromClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdFromClaim, out var userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var result = await mediator.Send(
            new UpdateUserCommand(userId, req.Name, req.Surname),
            ct);

        await result.SendResult(this, ct: ct);
    }
}
