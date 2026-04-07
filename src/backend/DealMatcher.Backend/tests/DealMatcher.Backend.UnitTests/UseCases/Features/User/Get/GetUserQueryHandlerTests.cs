using DealMatcher.Backend.UseCases.Features.User.Get;

namespace DealMatcher.Backend.UnitTests.UseCases.Features.User.Get;

public class GetUserQueryHandlerTests
{
    private readonly IRepository<UserEntity> _usersRepository;
    private readonly IMapper _mapper;
    private readonly GetUserQueryHandler _handler;

    public GetUserQueryHandlerTests()
    {
        _usersRepository = Substitute.For<IRepository<UserEntity>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetUserQueryHandler(
            _usersRepository,
            _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithUser_WhenUserExists()
    {
        var userId = 1;
        var query = new GetUserQuery(userId);

        var user = new UserEntity("john.doe@example.com", "John", "Doe");
        var expectedUserDto = new UserDTO(
            1,
            "john.doe@example.com",
            "John",
            "Doe",
            "ACTIVE",
            DateTime.UtcNow);

        _usersRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(user);

        _mapper.Map<UserDTO>(user)
            .Returns(expectedUserDto);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.ShouldBe(expectedUserDto);

        await _usersRepository.Received(1).GetByIdAsync(userId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<UserDTO>(user);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        var userId = 999;
        var query = new GetUserQuery(userId);

        _usersRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns((UserEntity?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Value.ShouldBeNull();

        await _usersRepository.Received(1).GetByIdAsync(userId, Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<UserDTO>(Arg.Any<UserEntity>());
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var userId = 5;
        var query = new GetUserQuery(userId);
        var cts = new CancellationTokenSource();

        var user = new UserEntity("test@example.com", "Test", "User");
        var expectedUserDto = new UserDTO(
            5,
            "test@example.com",
            "Test",
            "User",
            "ACTIVE",
            DateTime.UtcNow);

        _usersRepository.GetByIdAsync(userId, cts.Token)
            .Returns(user);

        _mapper.Map<UserDTO>(user)
            .Returns(expectedUserDto);

        var result = await _handler.Handle(query, cts.Token);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();

        await _usersRepository.Received(1).GetByIdAsync(userId, cts.Token);
        _mapper.Received(1).Map<UserDTO>(user);
    }
}
