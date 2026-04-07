namespace DealMatcher.Backend.Web.Endpoints.Users;

public sealed class Register(
    IMediator mediator) : Endpoint<RegisterUserRequest, UserDTO>
{
    public override void Configure()
    {
        Post("/auth/google-login");
        Version(1);
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Authenticate with Google";
            s.Description = "Validates Google token and returns JWT access token";
        });
    }

    public override async Task HandleAsync(RegisterUserRequest request, CancellationToken ct)
    {
        var command = new RegisterUserCommand(request);
        var result = await mediator.Send(command, ct);

        await result.SendResult(this, ct);
    }
}
