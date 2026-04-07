using MediatR;

namespace DealMatcher.Backend.UseCases.Features.User.Login;

public sealed record LoginCommand(string Email, string Password):IRequest<Result<LoginDTO>>;
