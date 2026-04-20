using DealMatcher.Backend.UseCases.Features.User.Login;

namespace DealMatcher.Backend.Web.Endpoints.Users;

public class Login(
    IMediator mediator) : Endpoint<LoginRequest, LoginDTO>
{
    public override void Configure()
    {
        Post("/users/login");
        Version(1);
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "User login";
            s.Description = "Authenticates user and returns JWT token";
        });
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var command = new LoginCommand(req.Email, req.Password);
        var result = await mediator.Send(command, ct);

        await result.SendResult(this, ct);
    }
}
