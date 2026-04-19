using Ardalis.SharedKernel;

namespace DealMatcher.Backend.UseCases.Features.User.Delete;

public sealed class DeleteCurrentUserHandler(
    IRepository<UserEntity> usersRepository
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

        await usersRepository.UpdateAsync(user, ct);

        return Result.Success();
    }
}
