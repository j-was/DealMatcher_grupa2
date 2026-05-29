namespace DealMatcher.Backend.UnitTests.UseCases.Features.Ban;

public class CreateBanCommandHandlerTests
{
    private readonly IRepository<BanEntity> _bansRepository;
    private readonly IReadRepository<UserEntity> _usersRepository;
    private readonly IMapper _mapper;
    private readonly CreateBanCommandHandler _handler;

    public CreateBanCommandHandlerTests()
    {
        _bansRepository = Substitute.For<IRepository<BanEntity>>();
        _usersRepository = Substitute.For<IReadRepository<UserEntity>>();
        _mapper = Substitute.For<IMapper>();

        _handler = new CreateBanCommandHandler(_bansRepository, _usersRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAdminCreatesBan()
    {
        var adminUser = CreateAdminUser(1);
        var userToBan = CreateUser(2);
        var createBanDTO = new CreateBanDTO
        {
            UserId = 2,
            Reason = "Breaking rules",
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        var command = new CreateBanCommand(1, createBanDTO);

        var expectedDTO = new BanDTO(1, 2, "Breaking rules", 1, DateTime.UtcNow, createBanDTO.ExpiresAt, true);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);

        _usersRepository.GetByIdAsync(2, Arg.Any<CancellationToken>())
            .Returns(userToBan);

        _bansRepository.ListAsync(Arg.Any<CancellationToken>())
            .Returns([]);

        _mapper.Map<BanDTO>(Arg.Any<BanEntity>())
            .Returns(expectedDTO);


        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(expectedDTO);

        await _bansRepository.Received(1)
            .AddAsync(Arg.Is<BanEntity>(ban =>
                ban.UserId == createBanDTO.UserId &&
                ban.Reason == createBanDTO.Reason &&
                ban.IssuedBy == command.UserId &&
                ban.ExpiresAt == createBanDTO.ExpiresAt),
                Arg.Any<CancellationToken>());

        _mapper.Received(1).Map<BanDTO>(Arg.Any<BanEntity>());
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenRequestingUserNotExist()
    {
        var createBanDTO = new CreateBanDTO
        {
            UserId = 2,
            Reason = "Breaking rules",
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        var command = new CreateBanCommand(1, createBanDTO);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns((UserEntity?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);

        await _bansRepository.DidNotReceive()
            .AddAsync(Arg.Any<BanEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenRequestingUserNotAdmin()
    {
        var regularUser = CreateUser(1);
        var createBanDTO = new CreateBanDTO
        {
            UserId = 2,
            Reason = "Breaking rules",
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        var command = new CreateBanCommand(1, createBanDTO);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(regularUser);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);

        await _bansRepository.DidNotReceive()
            .AddAsync(Arg.Any<BanEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserToBanNotExist()
    {
        var adminUser = CreateAdminUser(1);
        var createBanDTO = new CreateBanDTO
        {
            UserId = 2,
            Reason = "Breaking rules",
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        var command = new CreateBanCommand(1, createBanDTO);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);

        _usersRepository.GetByIdAsync(2, Arg.Any<CancellationToken>())
            .Returns((UserEntity?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.NotFound);

        await _bansRepository.DidNotReceive()
            .AddAsync(Arg.Any<BanEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenUserIsAlreadyBanned()
    {
        var adminUser = CreateAdminUser(1);
        var userToBan = CreateUser(2);
        var activeBan = CreateBanEntity(2, 1, true);

        var createBanDTO = new CreateBanDTO
        {
            UserId = 2,
            Reason = "Breaking rules",
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        var command = new CreateBanCommand(1, createBanDTO);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);

        _usersRepository.GetByIdAsync(2, Arg.Any<CancellationToken>())
            .Returns(userToBan);

        _bansRepository.ListAsync(Arg.Any<CancellationToken>())
            .Returns([activeBan]);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Conflict);

        await _bansRepository.DidNotReceive()
            .AddAsync(Arg.Any<BanEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var cts = new CancellationTokenSource();
        var adminUser = CreateAdminUser(1);
        var userToBan = CreateUser(2);

        var createBanDTO = new CreateBanDTO
        {
            UserId = 2,
            Reason = "Breaking rules",
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        var command = new CreateBanCommand(1, createBanDTO);

        var expectedDto = new BanDTO(
            1,
            2,
            "Breaking rules",
            1,
            DateTime.UtcNow,
            createBanDTO.ExpiresAt,
            true);

        _usersRepository.GetByIdAsync(1, cts.Token)
            .Returns(adminUser);
        _usersRepository.GetByIdAsync(2, cts.Token)
            .Returns(userToBan);
        _bansRepository.ListAsync(cts.Token)
            .Returns([]);

        _mapper.Map<BanDTO>(Arg.Any<BanEntity>())
            .Returns(expectedDto);

        await _handler.Handle(command, cts.Token);
        await _usersRepository.Received(1)
            .GetByIdAsync(1, cts.Token);
        await _usersRepository.Received(1)
            .GetByIdAsync(2, cts.Token);
        await _bansRepository.Received(1)
            .ListAsync(cts.Token);
        await _bansRepository.Received(1)
            .AddAsync(Arg.Any<BanEntity>(), cts.Token);
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

    private static BanEntity CreateBanEntity(int userId, int issuedBy, bool isActive)
    {
        var ban = new BanEntity(
            userId,
            "Reason test",
            issuedBy,
            DateTime.UtcNow.AddDays(7));

        typeof(BanEntity).GetProperty("Id")!.SetValue(ban, 1);
        typeof(BanEntity).GetProperty("IsActive")!.SetValue(ban, isActive);
        return ban;
    }
}
