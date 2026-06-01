namespace DealMatcher.Backend.UseCases.Features.User.Update;

public sealed record UpdateUserCommand(int UserId, string Name, string Surname) : IRequest<Result<UserDTO>>;
