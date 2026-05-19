using Ardalis.SharedKernel;

namespace DealMatcher.Backend.UseCases.Features.User.Delete;

public sealed class DeleteCurrentUserHandler(
    IRepository<UserEntity> usersRepository,
    IPublisher publisher
) : IRequestHandler<DeleteCurrentUserCommand, Result>
{
    public async Task<Result> Handle(DeleteCurrentUserCommand request, CancellationToken ct)
    {
        var user = await usersRepository.GetByIdAsync(request.UserId, ct);

        if (user is null)
        {
            return Result.NotFound("Nie znaleziono użytkownika");
        }

        user.UpdateStatus(UserStatus.Inactive);
        user.Delete();
        await usersRepository.UpdateAsync(user, ct);

        await publisher.Publish(
            new UserDeletedEvent(user.Id),
            ct);

        return Result.Success();
    }
}
