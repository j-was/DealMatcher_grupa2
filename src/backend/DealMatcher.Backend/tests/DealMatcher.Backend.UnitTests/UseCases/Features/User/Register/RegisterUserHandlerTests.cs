namespace DealMatcher.Backend.UnitTests.UseCases.Features.User.Register;

public class RegisterUserHandlerTests
{
    private readonly IRepository<UserEntity> _usersRepository;
    private readonly IPasswordHashService _passwordHasher;
    private readonly IMapper _mapper;
    private readonly RegisterUserHandler _handler;
    private readonly IPublisher _publisher;

    public RegisterUserHandlerTests()
    {
        _usersRepository = Substitute.For<IRepository<UserEntity>>();
        _passwordHasher = Substitute.For<IPasswordHashService>();
        _mapper = Substitute.For<IMapper>();
        _publisher = Substitute.For<IPublisher>();
        _handler = new RegisterUserHandler(
            _usersRepository,
            _passwordHasher,
            _mapper, _publisher);
    }

    [Fact]
    public async Task Handle_ShouldReturnCreated_WithUserDTO_WhenUserIsNew()
    {
        var email = "newuser@example.com";
        var name = "Jane";
        var surname = "Smith";
        var password = "ValidPass123!";
        var hashedPassword = "hashed_password_xyz";
        var command = new RegisterUserCommand(email, name, surname, password);

        var expectedUserDto = new UserDTO(1, email, name, surname, "ACTIVE", DateTime.UtcNow);

        _usersRepository.FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), Arg.Any<CancellationToken>())
            .Returns((UserEntity?)null);
        _passwordHasher.HashPassword(password)
            .Returns(hashedPassword);
        _mapper.Map<UserDTO>(Arg.Any<UserEntity>())
            .Returns(expectedUserDto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(expectedUserDto);

        await _usersRepository.Received(1)
            .FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), Arg.Any<CancellationToken>());
        _passwordHasher.Received(1).HashPassword(password);
        await _usersRepository.Received(1).AddAsync(Arg.Is<UserEntity>(u =>
                u.Email == email &&
                u.Name == name &&
                u.Surname == surname &&
                u.Status == UserStatus.Active &&
                u.PasswordHash == hashedPassword),
            Arg.Any<CancellationToken>());
        await _usersRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<UserDTO>(Arg.Any<UserEntity>());
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenUserWithEmailAlreadyExists()
    {
        var email = "existing@example.com";
        var command = new RegisterUserCommand(email, "New", "User", "password");

        var existingUser = new UserEntity(email, "Existing", "User");

        _usersRepository.FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), Arg.Any<CancellationToken>())
            .Returns(existingUser);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Value.ShouldBeNull();

        await _usersRepository.Received(1)
            .FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), Arg.Any<CancellationToken>());
        _passwordHasher.DidNotReceive().HashPassword(Arg.Any<string>());
        await _usersRepository.DidNotReceive().AddAsync(Arg.Any<UserEntity>(), Arg.Any<CancellationToken>());
        await _usersRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<UserDTO>(Arg.Any<UserEntity>());
    }

    [Fact]
    public async Task Handle_ShouldHashPasswordCorrectly_WhenCreatingUser()
    {
        var email = "test@example.com";
        var name = "Test";
        var surname = "User";
        var password = "SecurePass123!";
        var expectedHash = "hashed_secure_password";
        var command = new RegisterUserCommand(email, name, surname, password);

        UserEntity? capturedUser = null;

        _usersRepository.FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), Arg.Any<CancellationToken>())
            .Returns((UserEntity?)null);
        _passwordHasher.HashPassword(password)
            .Returns(expectedHash);
        await _usersRepository.AddAsync(Arg.Do<UserEntity>(user => capturedUser = user), Arg.Any<CancellationToken>());
        _mapper.Map<UserDTO>(Arg.Any<UserEntity>())
            .Returns(new UserDTO(1, email, name, surname, "ACTIVE", DateTime.UtcNow));

        await _handler.Handle(command, CancellationToken.None);

        capturedUser.ShouldNotBeNull();
        capturedUser.PasswordHash.ShouldBe(expectedHash);
        _passwordHasher.Received(1).HashPassword(password);
    }

    [Fact]
    public async Task Handle_ShouldCreateUserWithActiveStatus_WhenRegistering()
    {
        var command = new RegisterUserCommand("test@example.com", "Test", "User", "password");

        UserEntity? capturedUser = null;

        _usersRepository.FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), Arg.Any<CancellationToken>())
            .Returns((UserEntity?)null);
        _passwordHasher.HashPassword(Arg.Any<string>())
            .Returns("hashed");
        await _usersRepository.AddAsync(Arg.Do<UserEntity>(user => capturedUser = user), Arg.Any<CancellationToken>());
        _mapper.Map<UserDTO>(Arg.Any<UserEntity>())
            .Returns(new UserDTO(1, "test@example.com", "Test", "User", "ACTIVE", DateTime.UtcNow));

        await _handler.Handle(command, CancellationToken.None);


        capturedUser.ShouldNotBeNull();
        capturedUser.Status.ShouldBe(UserStatus.Active);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var command = new RegisterUserCommand("test@example.com", "Test", "User", "password");
        var cts = new CancellationTokenSource();

        _usersRepository.FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), cts.Token)
            .Returns((UserEntity?)null);
        _passwordHasher.HashPassword(Arg.Any<string>())
            .Returns("hashed");
        _mapper.Map<UserDTO>(Arg.Any<UserEntity>())
            .Returns(new UserDTO(1, "test@example.com", "Test", "User", "ACTIVE", DateTime.UtcNow));

        await _handler.Handle(command, cts.Token);

        await _usersRepository.Received(1).FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), cts.Token);
        await _usersRepository.Received(1).AddAsync(Arg.Any<UserEntity>(), cts.Token);
        await _usersRepository.Received(1).SaveChangesAsync(cts.Token);
    }

    [Fact]
    public async Task Handle_ShouldMapUserToUserDTO_AfterSuccessfulCreation()
    {
        var command = new RegisterUserCommand("test@example.com", "Test", "User", "password");
        var expectedDto = new UserDTO(1, command.Email, command.Name, command.Surname, "ACTIVE", DateTime.UtcNow);

        _usersRepository.FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), Arg.Any<CancellationToken>())
            .Returns((UserEntity?)null);
        _passwordHasher.HashPassword(Arg.Any<string>())
            .Returns("hashed");

        UserEntity? userToMap = null;
        _mapper.Map<UserDTO>(Arg.Do<UserEntity>(u => userToMap = u))
            .Returns(expectedDto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Value.ShouldBe(expectedDto);
        userToMap.ShouldNotBeNull();
        userToMap.Email.ShouldBe(command.Email);
        userToMap.Name.ShouldBe(command.Name);
        userToMap.Surname.ShouldBe(command.Surname);
    }
}
