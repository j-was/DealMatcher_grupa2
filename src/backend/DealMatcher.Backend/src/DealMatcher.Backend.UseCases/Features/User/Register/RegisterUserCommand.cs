namespace DealMatcher.Backend.UseCases.Features.User.Register;

public sealed record RegisterUserCommand(string Email, string Name, string Surname, string Password)
    : ICommand<Result<UserDTO>>;
