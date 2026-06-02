namespace DealMatcher.Backend.UnitTests.UseCases.Features.Ban;

public class GetBansQueryHandlerTests
{
    private readonly IReadRepository<BanEntity> _bansRepository;
    private readonly IReadRepository<UserEntity> _usersRepository;
    private readonly IMapper _mapper;
    private readonly GetBansQueryHandler _handler;

    public GetBansQueryHandlerTests()
    {
        _bansRepository = Substitute.For<IReadRepository<BanEntity>>();
        _usersRepository = Substitute.For<IReadRepository<UserEntity>>();
        _mapper = Substitute.For<IMapper>();

        _handler = new GetBansQueryHandler(
            _bansRepository,
            _usersRepository,
            _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithBans_WhenUserIsAdmin()
    {
        var query = new GetBansQuery(1, null, null);
        var adminUser = CreateAdminUser(1);

        var ban1 = CreateBanEntity(1, 2, 1, true);
        var ban2 = CreateBanEntity(2, 3, 1, true);

        var bans = new List<BanEntity> { ban1, ban2 };

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);

        _bansRepository.ListAsync(Arg.Any<CancellationToken>())
            .Returns(bans);

        _mapper.Map<BanDTO>(ban1)
            .Returns(CreateBanDTO(ban1));

        _mapper.Map<BanDTO>(ban2)
            .Returns(CreateBanDTO(ban2));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(2);

        result.Value[0].Id.ShouldBe(2);
        result.Value[1].Id.ShouldBe(1);
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenAdminDoesNotExist()
    {
        var query = new GetBansQuery(1, null, null);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns((UserEntity?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);

        await _bansRepository.DidNotReceive()
            .ListAsync(Arg.Any<CancellationToken>());

        _mapper.DidNotReceive()
            .Map<BanDTO>(Arg.Any<BanEntity>());
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        var query = new GetBansQuery(1, null, null);
        var regularUser = CreateUser(1);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(regularUser);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);

        await _bansRepository.DidNotReceive()
            .ListAsync(Arg.Any<CancellationToken>());

        _mapper.DidNotReceive()
            .Map<BanDTO>(Arg.Any<BanEntity>());
    }

    [Fact]
    public async Task Handle_ShouldFilterByUserId_WhenUserIdIsProvided()
    {
        var query = new GetBansQuery(1, 2, null);
        var adminUser = CreateAdminUser(1);

        var matchingBan = CreateBanEntity(1, 2, 1, true);
        var otherBan = CreateBanEntity(2, 3, 1, true);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);

        _bansRepository.ListAsync(Arg.Any<CancellationToken>())
            .Returns([matchingBan, otherBan]);

        _mapper.Map<BanDTO>(matchingBan)
            .Returns(CreateBanDTO(matchingBan));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Count.ShouldBe(1);
        result.Value[0].UserId.ShouldBe(2);

        _mapper.Received(1).Map<BanDTO>(matchingBan);
        _mapper.DidNotReceive().Map<BanDTO>(otherBan);
    }

    [Fact]
    public async Task Handle_ShouldFilterByActive_WhenActiveIsProvided()
    {
        var query = new GetBansQuery(1, null, true);
        var adminUser = CreateAdminUser(1);

        var activeBan = CreateBanEntity(1, 2, 1, true);
        var inactiveBan = CreateBanEntity(2, 3, 1, false);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);

        _bansRepository.ListAsync(Arg.Any<CancellationToken>())
            .Returns([activeBan, inactiveBan]);

        _mapper.Map<BanDTO>(activeBan)
            .Returns(CreateBanDTO(activeBan));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Count.ShouldBe(1);
        result.Value[0].IsActive.ShouldBeTrue();

        _mapper.Received(1).Map<BanDTO>(activeBan);
        _mapper.DidNotReceive().Map<BanDTO>(inactiveBan);
    }

    [Fact]
    public async Task Handle_ShouldFilterByUserIdAndActive_WhenBothFiltersAreProvided()
    {
        var query = new GetBansQuery(1, 2, true);
        var adminUser = CreateAdminUser(1);

        var matchingBan = CreateBanEntity(1, 2, 1, true);
        var inactiveBanForSameUser = CreateBanEntity(2, 2, 1, false);
        var activeBanForOtherUser = CreateBanEntity(3, 3, 1, true);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);

        _bansRepository.ListAsync(Arg.Any<CancellationToken>())
            .Returns([matchingBan, inactiveBanForSameUser, activeBanForOtherUser]);

        _mapper.Map<BanDTO>(matchingBan)
            .Returns(CreateBanDTO(matchingBan));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Count.ShouldBe(1);
        result.Value[0].UserId.ShouldBe(2);
        result.Value[0].IsActive.ShouldBeTrue();

        _mapper.Received(1).Map<BanDTO>(matchingBan);
        _mapper.DidNotReceive().Map<BanDTO>(inactiveBanForSameUser);
        _mapper.DidNotReceive().Map<BanDTO>(activeBanForOtherUser);
    }

    private static UserEntity CreateAdminUser(int id)
    {
        var admin = new UserEntity("admin@email.com", "Admin", "Admin");

        typeof(UserEntity).GetProperty("Id")!.SetValue(admin, id);
        typeof(UserEntity).GetProperty("Status")!.SetValue(admin, UserStatus.Admin);
        return admin;
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var cts = new CancellationTokenSource();
        var query = new GetBansQuery(1, null, null);
        var adminUser = CreateAdminUser(1);
        var ban = CreateBanEntity(1, 2, 1, true);

        _usersRepository.GetByIdAsync(1, cts.Token)
            .Returns(adminUser);

        _bansRepository.ListAsync(cts.Token)
            .Returns([ban]);

        _mapper.Map<BanDTO>(ban)
            .Returns(CreateBanDTO(ban));

        await _handler.Handle(query, cts.Token);

        await _usersRepository.Received(1)
            .GetByIdAsync(1, cts.Token);

        await _bansRepository.Received(1)
            .ListAsync(cts.Token);
    }

    private static UserEntity CreateUser(int id)
    {
        var user = new UserEntity($"user{id}@example.com", "User", "Test");

        typeof(UserEntity).GetProperty("Id")!.SetValue(user, id);
        typeof(UserEntity).GetProperty("Status")!.SetValue(user, UserStatus.Active);
        return user;
    }

    private static BanEntity CreateBanEntity(int id, int userId, int issuedBy, bool isActive)
    {
        var ban = new BanEntity(
            userId,
            "Reason test",
            issuedBy,
            DateTime.UtcNow.AddDays(7));

        typeof(BanEntity).GetProperty("Id")!.SetValue(ban, id);
        typeof(BanEntity).GetProperty("IsActive")!.SetValue(ban, isActive);

        return ban;
    }

    private static BanDTO CreateBanDTO(BanEntity ban) =>
        new(ban.Id, ban.UserId, ban.Reason, ban.IssuedBy, ban.IssuedAt, ban.ExpiresAt, ban.IsActive);
}
