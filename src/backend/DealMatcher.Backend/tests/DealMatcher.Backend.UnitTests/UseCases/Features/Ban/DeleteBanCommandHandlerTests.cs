using Ardalis.Specification;

namespace DealMatcher.Backend.UnitTests.UseCases.Features.Ban;

public class DeleteBanCommandHandlerTests
{
    private readonly IRepository<BanEntity> _bansRepository;
    private readonly IReadRepository<UserEntity> _usersRepository;
    private readonly DeleteBanCommandHandler _handler;

    public DeleteBanCommandHandlerTests()
    {
        _bansRepository = Substitute.For<IRepository<BanEntity>>();
        _usersRepository = Substitute.For<IReadRepository<UserEntity>>();

        _handler = new DeleteBanCommandHandler(
            _bansRepository,
            _usersRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAdminDeletesBan()
    {
        var adminUser = CreateAdminUser(1);
        var bannedUser = CreateBannedUser(2);
        var ban = CreateBanEntity(10, 2, 1);
        var command = new DeleteBanCommand(1, 10);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);

        _bansRepository.FirstOrDefaultAsync(
                Arg.Any<ISpecification<BanEntity>>(),
                Arg.Any<CancellationToken>())
            .Returns(ban);

        _usersRepository.GetByIdAsync(2, Arg.Any<CancellationToken>())
            .Returns(bannedUser);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Status.ShouldBe(ResultStatus.Ok);

        bannedUser.Status.ShouldBe(UserStatus.Active);

        await _bansRepository.Received(1)
            .DeleteAsync(ban, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenAdminDoesNotExist()
    {
        var command = new DeleteBanCommand(1, 10);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns((UserEntity?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);

        await _bansRepository.DidNotReceive()
            .FirstOrDefaultAsync(
                Arg.Any<ISpecification<BanEntity>>(),
                Arg.Any<CancellationToken>());

        await _bansRepository.DidNotReceive()
            .DeleteAsync(Arg.Any<BanEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        var command = new DeleteBanCommand(1, 10);
        var regularUser = CreateUser(1);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(regularUser);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);

        await _bansRepository.DidNotReceive()
            .FirstOrDefaultAsync(
                Arg.Any<ISpecification<BanEntity>>(),
                Arg.Any<CancellationToken>());

        await _bansRepository.DidNotReceive()
            .DeleteAsync(Arg.Any<BanEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldUpdateBannedUserStatusToActive_WhenBanIsDeleted()
    {
        var adminUser = CreateAdminUser(1);
        var bannedUser = CreateBannedUser(2);
        var ban = CreateBanEntity(10, 2, 1);
        var command = new DeleteBanCommand(1, 10);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);

        _bansRepository.FirstOrDefaultAsync(
                Arg.Any<ISpecification<BanEntity>>(),
                Arg.Any<CancellationToken>())
            .Returns(ban);

        _usersRepository.GetByIdAsync(2, Arg.Any<CancellationToken>())
            .Returns(bannedUser);

        await _handler.Handle(command, CancellationToken.None);

        bannedUser.Status.ShouldBe(UserStatus.Active);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var cts = new CancellationTokenSource();
        var command = new DeleteBanCommand(1, 10);
        var adminUser = CreateAdminUser(1);
        var bannedUser = CreateBannedUser(2);
        var ban = CreateBanEntity(10, 2, 1);

        _usersRepository.GetByIdAsync(1, cts.Token)
            .Returns(adminUser);

        _bansRepository.FirstOrDefaultAsync(
                Arg.Any<ISpecification<BanEntity>>(),
                cts.Token)
            .Returns(ban);

        _usersRepository.GetByIdAsync(2, cts.Token)
            .Returns(bannedUser);

        await _handler.Handle(command, cts.Token);

        await _usersRepository.Received(1)
            .GetByIdAsync(1, cts.Token);

        await _usersRepository.Received(1)
            .GetByIdAsync(2, cts.Token);

        await _bansRepository.Received(1)
            .FirstOrDefaultAsync(
                Arg.Any<ISpecification<BanEntity>>(),
                cts.Token);

        await _bansRepository.Received(1)
            .DeleteAsync(ban, cts.Token);
    }

    private static UserEntity CreateAdminUser(int id)
    {
        var admin = new UserEntity("admin@email.com", "Admin", "Admin");

        typeof(UserEntity).GetProperty("Id")!.SetValue(admin, id);
        typeof(UserEntity).GetProperty("Status")!.SetValue(admin, UserStatus.Admin);
        return admin;
    }

    private static UserEntity CreateUser(int id)
    {
        var user = new UserEntity($"user{id}@example.com", "User", "Test");

        typeof(UserEntity).GetProperty("Id")!.SetValue(user, id);
        typeof(UserEntity).GetProperty("Status")!.SetValue(user, UserStatus.Active);
        return user;
    }

    private static UserEntity CreateBannedUser(int id)
    {
        var user = new UserEntity($"user{id}@example.com", "Banned", "Test");

        typeof(UserEntity).GetProperty("Id")!.SetValue(user, id);
        typeof(UserEntity).GetProperty("Status")!.SetValue(user, UserStatus.Banned);
        return user;
    }

    private static BanEntity CreateBanEntity(int id, int userId, int issuedBy)
    {
        var ban = new BanEntity(
            userId,
            "Reason test",
            issuedBy,
            DateTime.UtcNow.AddDays(7));

        typeof(BanEntity).GetProperty("Id")!.SetValue(ban, id);
        typeof(BanEntity).GetProperty("IsActive")!.SetValue(ban, true);
        return ban;
    }
}
