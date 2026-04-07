namespace DealMatcher.Backend.UseCases.Features.User.Get;

public sealed record GetUserQuery(int userId): IRequest<Result<UserDTO>>;
