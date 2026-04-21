namespace DealMatcher.Backend.UnitTests.UseCases.Features.User.Login;

public class LoginHandlerTests
{
    private readonly IRepository<UserEntity> _usersRepository;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IMapper _mapper;
    private readonly ITokenProvider _tokenProvider;
    private readonly LoginHandler _handler;

    public LoginHandlerTests()
    {
        _usersRepository = Substitute.For<IRepository<UserEntity>>();
        _passwordHashService = Substitute.For<IPasswordHashService>();
        _mapper = Substitute.For<IMapper>();
        _tokenProvider = Substitute.For<ITokenProvider>();
        _handler = new LoginHandler(
            _usersRepository,
            _passwordHashService,
            _mapper,
            _tokenProvider);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithTokenAndUser_WhenCredentialsAreValid()
    {
        // Arrange
        var email = "john.doe@example.com";
        var password = "ValidPass123!";
        var passwordHash = "hashed_password_xyz";
        var command = new LoginCommand(email, password);

        var user = new UserEntity(email, "John", "Doe");
        user.SetNewHash(passwordHash);

        var expectedToken = "jwt_token_123456";
        var expectedUserDto = new UserDTO(1, email, "John", "Doe", "ACTIVE", DateTime.UtcNow);

        _usersRepository.FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHashService.AuthorizePassword(passwordHash, password)
            .Returns(true);
        _tokenProvider.GenerateToken(user)
            .Returns(expectedToken);
        _mapper.Map<UserDTO>(user)
            .Returns(expectedUserDto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.AccessToken.ShouldBe(expectedToken);
        result.Value.User.ShouldBe(expectedUserDto);

        await _usersRepository.Received(1).FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), Arg.Any<CancellationToken>());
        _passwordHashService.Received(1).AuthorizePassword(passwordHash, password);
        _tokenProvider.Received(1).GenerateToken(user);
        _mapper.Received(1).Map<UserDTO>(user);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorized_WhenUserDoesNotExist()
    {
        var command = new LoginCommand("nonexistent@example.com", "password");

        _usersRepository.FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), Arg.Any<CancellationToken>())
            .Returns((UserEntity?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Value.ShouldBeNull();

        await _usersRepository.Received(1).FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), Arg.Any<CancellationToken>());
        _passwordHashService.DidNotReceive().AuthorizePassword(Arg.Any<string>(), Arg.Any<string>());
        _tokenProvider.DidNotReceive().GenerateToken(Arg.Any<UserEntity>());
        _mapper.DidNotReceive().Map<UserDTO>(Arg.Any<UserEntity>());
    }

    [Theory]
    [InlineData("Banned")]
    [InlineData("Inactive")]
    public async Task Handle_ShouldReturnForbidden_WhenUserIsBannedOrInactive(string statusValue)
    {
        var email = "user@example.com";
        var command = new LoginCommand(email, "password");

        var user = new UserEntity(email, "Test", "User");
        var status = statusValue == "Banned" ? UserStatus.Banned : UserStatus.Inactive;

        var statusField = typeof(UserEntity).GetProperty("Status");
        statusField?.SetValue(user, status);

        _usersRepository.FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), Arg.Any<CancellationToken>())
            .Returns(user);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Value.ShouldBeNull();

        await _usersRepository.Received(1).FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), Arg.Any<CancellationToken>());
        _passwordHashService.DidNotReceive().AuthorizePassword(Arg.Any<string>(), Arg.Any<string>());
        _tokenProvider.DidNotReceive().GenerateToken(Arg.Any<UserEntity>());
        _mapper.DidNotReceive().Map<UserDTO>(Arg.Any<UserEntity>());
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorized_WhenPasswordIsInvalid()
    {
        var email = "john@example.com";
        var password = "WrongPassword";
        var passwordHash = "hashed_password";
        var command = new LoginCommand(email, password);

        var user = new UserEntity(email, "John", "Doe");
        user.SetNewHash(passwordHash);

        _usersRepository.FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHashService.AuthorizePassword(passwordHash, password)
            .Returns(false);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Value.ShouldBeNull();

        await _usersRepository.Received(1).FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), Arg.Any<CancellationToken>());
        _passwordHashService.Received(1).AuthorizePassword(passwordHash, password);
        _tokenProvider.DidNotReceive().GenerateToken(Arg.Any<UserEntity>());
        _mapper.DidNotReceive().Map<UserDTO>(Arg.Any<UserEntity>());
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var email = "test@example.com";
        var password = "ValidPass123!";
        var passwordHash = "hashed_password";
        var command = new LoginCommand(email, password);
        var cts = new CancellationTokenSource();

        var user = new UserEntity(email, "Test", "User");
        user.SetNewHash(passwordHash);

        _usersRepository.FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), cts.Token)
            .Returns(user);
        _passwordHashService.AuthorizePassword(passwordHash, password)
            .Returns(true);
        _tokenProvider.GenerateToken(user)
            .Returns("token");
        _mapper.Map<UserDTO>(user)
            .Returns(new UserDTO(1, email, "Test", "User", "ACTIVE", DateTime.UtcNow));

        await _handler.Handle(command, cts.Token);

        await _usersRepository.Received(1).FirstOrDefaultAsync(Arg.Any<UserByEmailSpec>(), cts.Token);
        _passwordHashService.Received(1).AuthorizePassword(passwordHash, password);
        _tokenProvider.Received(1).GenerateToken(user);
        _mapper.Received(1).Map<UserDTO>(user);
    }
}
