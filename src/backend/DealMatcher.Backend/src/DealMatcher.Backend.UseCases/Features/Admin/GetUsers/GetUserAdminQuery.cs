namespace DealMatcher.Backend.UseCases.Features.Admin.GetUsers;

public sealed record GetUserAdminQuery(int UserId, int RequestingUserId) : IRequest<Result<UserDetailsDTO>>;
