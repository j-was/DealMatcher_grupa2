namespace DealMatcher.Backend.UnitTests.UseCases.Features.User.Update;

public class UpdateUserHandlerTests
{
    private readonly IRepository<UserEntity> _usersRepository;
    private readonly IMapper _mapper;
    private readonly UpdateUserHandler _handler;
    private readonly IPublisher _publisher;

    public UpdateUserHandlerTests()
    {
        _usersRepository = Substitute.For<IRepository<UserEntity>>();
        _mapper = Substitute.For<IMapper>();
        _publisher = Substitute.For<IPublisher>();
        _handler = new UpdateUserHandler(_usersRepository, _mapper, _publisher);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithUpdatedUser_WhenUserExists()
    {
        var user = new UserEntity("test@example.com", "OldName", "OldSurname");
        var command = new UpdateUserCommand(1, "NewName", "NewSurname");
        var expectedDto = new UserDTO(1, "test@example.com", "NewName", "NewSurname", "ACTIVE", DateTime.UtcNow);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(user);
        _mapper.Map<UserDTO>(user).Returns(expectedDto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(expectedDto);
        user.Name.ShouldBe("NewName");
        user.Surname.ShouldBe("NewSurname");
        await _usersRepository.Received(1).UpdateAsync(user, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<UserDTO>(user);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        var command = new UpdateUserCommand(999, "NewName", "NewSurname");

        _usersRepository.GetByIdAsync(999, Arg.Any<CancellationToken>()).Returns((UserEntity?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.Errors.ShouldContain("Nie znaleziono użytkownika");
        await _usersRepository.DidNotReceive().UpdateAsync(Arg.Any<UserEntity>(), Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<UserDTO>(Arg.Any<UserEntity>());
    }

    [Fact]
    public async Task Handle_ShouldAllow_EmptySurname()
    {
        var user = new UserEntity("test@example.com", "OldName", "OldSurname");
        var command = new UpdateUserCommand(1, "NewName", "");
        var expectedDto = new UserDTO(1, "test@example.com", "NewName", "", "ACTIVE", DateTime.UtcNow);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(user);
        _mapper.Map<UserDTO>(user).Returns(expectedDto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        user.Surname.ShouldBe("");
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var user = new UserEntity("test@example.com", "OldName", "OldSurname");
        var command = new UpdateUserCommand(1, "NewName", "NewSurname");
        var cts = new CancellationTokenSource();

        _usersRepository.GetByIdAsync(1, cts.Token).Returns(user);
        _mapper.Map<UserDTO>(user).Returns(new UserDTO(1, "test@example.com", "NewName", "NewSurname", "ACTIVE", DateTime.UtcNow));

        await _handler.Handle(command, cts.Token);

        await _usersRepository.Received(1).GetByIdAsync(1, cts.Token);
        await _usersRepository.Received(1).UpdateAsync(user, cts.Token);
    }
}
