namespace DealMatcher.Backend.UseCases.Features.User.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<Result<LoginDTO>>;
