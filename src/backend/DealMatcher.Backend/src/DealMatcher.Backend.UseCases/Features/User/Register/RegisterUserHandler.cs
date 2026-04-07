using DealMatcher.Backend.Core.Aggregates.User.Specifications;
using DealMatcher.Backend.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DealMatcher.Backend.UseCases.Features.User.Register;

public class RegisterUserHandler(IRepository<UserEntity> usersRepository, IPasswordHashService passwordHasher, IMapper mapper):IRequestHandler<RegisterUserCommand,Result<UserDTO>>
{
    public async Task<Result<UserDTO>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingSpec = new UserByEmailSpec(request.Email);
        var existingUser = await usersRepository.FirstOrDefaultAsync(existingSpec, cancellationToken);
        if (existingUser is not null)
        {
            return Result.Conflict("User already exists with given email");
        }

        var hash = passwordHasher.HashPassword(request.Password);
        var user = new UserEntity(request.Email, request.Name, request.Surname);
        user.SetNewHash(hash);
        await usersRepository.AddAsync(user, cancellationToken);
        await usersRepository.SaveChangesAsync(cancellationToken);

        var res = mapper.Map<UserDTO>(user);
        return Result.Created(res);
    }
}
