namespace DealMatcher.Backend.UseCases.Features.User.Get;

public class GetUserQueryHandler(
    IRepository<UserEntity> usersRepository, IMapper mapper) : IRequestHandler<GetUserQuery, Result<UserDTO>>
{
    public async Task<Result<UserDTO>> Handle(GetUserQuery request, CancellationToken ct)
    {
        var user = await usersRepository.GetByIdAsync(request.userId, ct);

        if (user is null)
        {
            return Result.NotFound("Nie znaleziono użytkownika");
        }

        return Result.Success(mapper.Map<UserDTO>(user));
    }
}