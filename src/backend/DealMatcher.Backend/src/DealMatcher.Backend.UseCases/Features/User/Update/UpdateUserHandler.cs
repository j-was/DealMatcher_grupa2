namespace DealMatcher.Backend.UseCases.Features.User.Update;

public sealed class UpdateUserHandler(
    IRepository<UserEntity> usersRepository,
    IMapper mapper,
    IPublisher publisher
) : IRequestHandler<UpdateUserCommand, Result<UserDTO>>
{
    public async Task<Result<UserDTO>> Handle(UpdateUserCommand request, CancellationToken ct)
    {
        var user = await usersRepository.GetByIdAsync(request.UserId, ct);

        if (user is null)
        {
            return Result.NotFound("Nie znaleziono użytkownika");
        }

        user.UpdateProfile(request.Name, request.Surname);

        await usersRepository.UpdateAsync(user, ct);

        await publisher.Publish(
            new UserUpdatedEvent(user.Id, user.Name, user.Surname),
            ct);

        return Result.Success(mapper.Map<UserDTO>(user));
    }
}
