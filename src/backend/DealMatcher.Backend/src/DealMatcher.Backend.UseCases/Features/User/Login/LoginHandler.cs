namespace DealMatcher.Backend.UseCases.Features.User.Login;

public class LoginHandler(
    IRepository<UserEntity> usersRepository,
    IPasswordHashService passwordHashService,
    IMapper mapper,
    ITokenProvider tokenService,
    IPublisher publisher) : IRequestHandler<LoginCommand, Result<LoginDTO>>
{
    public async Task<Result<LoginDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var spec = new UserByEmailSpec(request.Email);
        var user = await usersRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (user is null)
        {
            return Result.Unauthorized();
        }

        if (user.Status == UserStatus.Banned || user.Status == UserStatus.Inactive)
        {
            return Result.Forbidden();
        }

        if (!passwordHashService.AuthorizePassword(user.PasswordHash, request.Password))
        {
            return Result.Unauthorized();
        }

        var res = new LoginDTO(tokenService.GenerateToken(user), mapper.Map<UserDTO>(user));

        await publisher.Publish(
            new UserLoggedInEvent(user.Id),
            cancellationToken);

        return Result.Success(res);
    }
}
