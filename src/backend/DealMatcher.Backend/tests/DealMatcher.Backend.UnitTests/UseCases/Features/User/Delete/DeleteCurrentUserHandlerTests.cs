namespace DealMatcher.Backend.UnitTests.UseCases.Features.User.Delete;

public class DeleteCurrentUserHandlerTests
{
    private readonly IRepository<UserEntity> _usersRepository;
    private readonly DeleteCurrentUserHandler _handler;
    private readonly IPublisher _publisher;

    public DeleteCurrentUserHandlerTests()
    {
        _usersRepository = Substitute.For<IRepository<UserEntity>>();
        _publisher = Substitute.For<IPublisher>();
        _handler = new DeleteCurrentUserHandler(_usersRepository, _publisher);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserExists()
    {
        var user = new UserEntity("test@example.com", "Test", "User");
        var command = new DeleteCurrentUserCommand(1);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(user);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        user.Status.ShouldBe(UserStatus.Inactive);

        await _usersRepository.Received(1).UpdateAsync(user, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        var command = new DeleteCurrentUserCommand(999);

        _usersRepository.GetByIdAsync(999, Arg.Any<CancellationToken>()).Returns((UserEntity?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.Errors.ShouldContain("Nie znaleziono użytkownika");
        await _usersRepository.DidNotReceive().UpdateAsync(Arg.Any<UserEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var user = new UserEntity("test@example.com", "Test", "User");
        var command = new DeleteCurrentUserCommand(1);
        var cts = new CancellationTokenSource();

        _usersRepository.GetByIdAsync(1, cts.Token).Returns(user);

        await _handler.Handle(command, cts.Token);

        await _usersRepository.Received(1).GetByIdAsync(1, cts.Token);
        await _usersRepository.Received(1).UpdateAsync(user, cts.Token);
    }
}
