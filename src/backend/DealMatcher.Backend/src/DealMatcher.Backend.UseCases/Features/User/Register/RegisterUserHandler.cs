namespace DealMatcher.Backend.UseCases.Features.User.Register;

public class RegisterUserHandler(
    IRepository<UserEntity> usersRepository,
    IPasswordHashService passwordHasher,
    IMapper mapper,
    IPublisher publisher) : IRequestHandler<RegisterUserCommand, Result<UserDTO>>
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

        await publisher.Publish(
            new UserCreatedEvent(user.Id, user.Name, user.Surname),
            cancellationToken);

        return Result.Created(res);
    }
}
