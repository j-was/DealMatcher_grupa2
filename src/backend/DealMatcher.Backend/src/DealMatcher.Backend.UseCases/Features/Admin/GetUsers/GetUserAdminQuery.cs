namespace DealMatcher.Backend.UseCases.Features.Ban.GetUsers;

public sealed record GetUserAdminQuery(int UserId, int RequestingUserId) : IRequest<Result<UserDetailsDTO>>;
