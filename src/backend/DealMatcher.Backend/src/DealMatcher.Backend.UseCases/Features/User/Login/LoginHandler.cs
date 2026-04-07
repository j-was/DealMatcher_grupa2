using DealMatcher.Backend.Core.Aggregates.User;
using DealMatcher.Backend.Core.Aggregates.User.Specifications;
using DealMatcher.Backend.Core.Interfaces;
using FastEndpoints;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace DealMatcher.Backend.UseCases.Features.User.Login;

public class LoginHandler(
    IRepository<UserEntity> usersRepository, IPasswordHashService passwordHashService,IMapper mapper,ITokenProvider tokenService):IRequestHandler<LoginCommand,Result<LoginDTO>>
{
    public async Task<Result<LoginDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var spec = new UserByEmailSpec(request.Email);
        var user = await usersRepository.FirstOrDefaultAsync(spec);
        if (user is null)
        {
            return Result.Unauthorized();
        }

        if (user.Status == UserStatus.Banned ||  user.Status == UserStatus.Inactive)
        {
            return Result.Forbidden();
        }

        if(!passwordHashService.AuthorizePassword(user.PasswordHash, request.Password))
        {
            return Result.Unauthorized();
        }

        var res = new LoginDTO(tokenService.GenerateToken(user),mapper.Map<UserDTO>(user));
        return Result.Success(res);
    }
}
