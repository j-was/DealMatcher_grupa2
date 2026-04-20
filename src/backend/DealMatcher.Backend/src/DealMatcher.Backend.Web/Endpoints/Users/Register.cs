namespace DealMatcher.Backend.Web.Endpoints.Users;

public sealed class Register(
    IMediator mediator) : Endpoint<RegisterUserRequest, UserDTO>
{
    public override void Configure()
    {
        Post("/users/register");
        Version(1);
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Register a new user";
            s.Description = "Creates a new user account with ACTIVE status";
        });
    }

    public override async Task HandleAsync(RegisterUserRequest request, CancellationToken ct)
    {
        var command = new RegisterUserCommand(request.Email, request.Name, request.Surname, request.Password);
        var result = await mediator.Send(command, ct);

        await result.SendResult(this, ct);
    }
}
