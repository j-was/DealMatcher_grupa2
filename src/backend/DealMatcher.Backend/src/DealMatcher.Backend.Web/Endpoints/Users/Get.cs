using System.IdentityModel.Tokens.Jwt;
using DealMatcher.Backend.UseCases.Features.User.Get;

namespace DealMatcher.Backend.Web.Endpoints.Users;

public class Get(IMediator mediator) : EndpointWithoutRequest<UserDTO>
{
    public override void Configure()
    {
        AllowAnonymous();
        Version(1);
        Get("/users/me");
        Summary(s =>
        {
            s.Summary = "Zwraca dane zalogowanego użytkownika";
            s.Description = "Jeżeli użytkownik jest zalogowany w sesji, to zwracany jest obiekt z jego podstawowymi danymi.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userIdFromClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

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