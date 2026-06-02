using DealMatcher.Backend.UseCases.Features.Admin.GetUsers;

namespace DealMatcher.Backend.UnitTests.UseCases.Features.Admin;

public class GetUsersQueryHandlerTests
{
    private readonly IReadRepository<UserEntity> _usersRepository;
    private readonly IMapper _mapper;
    private readonly GetUsersQueryHandler _handler;

    public GetUsersQueryHandlerTests()
    {
        _usersRepository = Substitute.For<IReadRepository<UserEntity>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetUsersQueryHandler(_usersRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithPagedUsers_WhenUserIsAdmin()
    {
        var query = new GetUsersQuery(1, 10, null, 1);
        var adminUser = CreateAdminUser(1);
        var users = new List<UserEntity> { CreateUser(1, "user1@example.com"), CreateUser(2, "user2@example.com") };

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);
        _usersRepository.ListAsync(Arg.Any<CancellationToken>())
            .Returns(users);
        _mapper.Map<UserDTO>(Arg.Any<UserEntity>())
            .Returns(
                new UserDTO(1, "user1@example.com", "User", "One", "ACTIVE", DateTime.UtcNow),
                new UserDTO(2, "user2@example.com", "User", "Two", "ACTIVE", DateTime.UtcNow));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Items.Count.ShouldBe(2);
        result.Value.Total.ShouldBe(2);
        result.Value.Page.ShouldBe(1);
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        var query = new GetUsersQuery(1, 10, null, 1);
        var regularUser = new UserEntity("user@example.com", "John", "Doe");

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(regularUser);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenUserDoesNotExist()
    {
        var query = new GetUsersQuery(1, 10, null, 1);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns((UserEntity?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);
    }

    [Fact]
    public async Task Handle_ShouldFilterByStatus()
    {
        var query = new GetUsersQuery(1, 10, "BANNED", 1);
        var adminUser = CreateAdminUser(1);
        var activeUser = CreateUser(1, "active@example.com", UserStatus.Active);
        var bannedUser = CreateUser(2, "banned@example.com", UserStatus.Banned);
        var users = new List<UserEntity> { activeUser, bannedUser };

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);
        _usersRepository.ListAsync(Arg.Any<CancellationToken>())
            .Returns(users);
        _mapper.Map<UserDTO>(Arg.Any<UserEntity>())
            .Returns(new UserDTO(2, "banned@example.com", "User", "Two", "BANNED", DateTime.UtcNow));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Items.Count.ShouldBe(1);
    }

    [Fact]
    public async Task Handle_ShouldRespectPagination()
    {
        var query = new GetUsersQuery(2, 3, null, 1);
        var adminUser = CreateAdminUser(1);
        var users = Enumerable.Range(1, 10)
            .Select(i => CreateUser(i, $"user{i}@example.com"))
            .ToList();

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);
        _usersRepository.ListAsync(Arg.Any<CancellationToken>())
            .Returns(users);
        _mapper.Map<UserDTO>(Arg.Any<UserEntity>())
            .Returns(new UserDTO(1, "user@example.com", "User", "Name", "ACTIVE", DateTime.UtcNow));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Page.ShouldBe(2);
        result.Value.Items.Count.ShouldBe(3);
        result.Value.Total.ShouldBe(10);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var query = new GetUsersQuery(1, 10, null, 1);
        var cts = new CancellationTokenSource();
        var adminUser = CreateAdminUser(1);
        var users = new List<UserEntity> { CreateUser(1, "user1@example.com") };

        _usersRepository.GetByIdAsync(1, cts.Token)
            .Returns(adminUser);
        _usersRepository.ListAsync(cts.Token)
            .Returns(users);
        _mapper.Map<UserDTO>(Arg.Any<UserEntity>())
            .Returns(new UserDTO(1, "user1@example.com", "User", "One", "ACTIVE", DateTime.UtcNow));

        await _handler.Handle(query, cts.Token);

        await _usersRepository.Received(1).GetByIdAsync(1, cts.Token);
        await _usersRepository.Received(1).ListAsync(cts.Token);
    }

    private static UserEntity CreateAdminUser(int id)
    {
        var user = new UserEntity("admin@example.com", "Admin", "User");
        typeof(UserEntity).GetProperty("Status")!.SetValue(user, UserStatus.Admin);
        typeof(UserEntity).GetProperty("Id")!.SetValue(user, id);
        return user;
    }

    private static UserEntity CreateUser(int id, string email, UserStatus? status = null)
    {
        var user = new UserEntity(email, "User", "Name");
        typeof(UserEntity).GetProperty("Id")!.SetValue(user, id);
        typeof(UserEntity).GetProperty("Status")!.SetValue(user, status ?? UserStatus.Active);
        return user;
    }
}
