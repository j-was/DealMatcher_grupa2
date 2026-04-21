namespace DealMatcher.Backend.UseCases.Features.User.Delete;

public sealed record DeleteCurrentUserCommand(int UserId) : IRequest<Result>;
