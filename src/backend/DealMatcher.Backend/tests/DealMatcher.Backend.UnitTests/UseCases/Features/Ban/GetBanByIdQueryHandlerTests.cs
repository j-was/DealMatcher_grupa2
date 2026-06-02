using Ardalis.Specification;

namespace DealMatcher.Backend.UnitTests.UseCases.Features.Ban;

public class GetBanByIdQueryHandlerTests
{
    private readonly IReadRepository<BanEntity> _bansRepository;
    private readonly IReadRepository<UserEntity> _usersRepository;
    private readonly IMapper _mapper;
    private readonly GetBanByIdQueryHandler _handler;

    public GetBanByIdQueryHandlerTests()
    {
        _bansRepository = Substitute.For<IReadRepository<BanEntity>>();
        _usersRepository = Substitute.For<IReadRepository<UserEntity>>();
        _mapper = Substitute.For<IMapper>();

        _handler = new GetBanByIdQueryHandler(
            _bansRepository,
            _usersRepository,
            _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithBan_WhenUserIsAdmin()
    {
        var query = new GetBanByIdQuery(1, 10);
        var adminUser = CreateAdminUser(1);
        var ban = CreateBanEntity(10, 2, 1);

        var expectedDTO = new BanDTO(10, 2, "Reason test", 1, ban.IssuedAt, ban.ExpiresAt, true);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);

        _bansRepository.FirstOrDefaultAsync(
                Arg.Any<ISpecification<BanEntity>>(),
                Arg.Any<CancellationToken>())
            .Returns(ban);

        _mapper.Map<BanDTO>(ban)
            .Returns(expectedDTO);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.ShouldBe(expectedDTO);

        _mapper.Received(1).Map<BanDTO>(ban);
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenAdminDOesNotExist()
    {
        var query = new GetBanByIdQuery(1, 10);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns((UserEntity?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);

        await _bansRepository.DidNotReceive()
            .FirstOrDefaultAsync(Arg.Any<ISpecification<BanEntity>>(), Arg.Any<CancellationToken>());

        _mapper.DidNotReceive()
            .Map<BanDTO>(Arg.Any<BanEntity>());
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        var query = new GetBanByIdQuery(1, 10);
        var regularUser = CreateUser(1);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(regularUser);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);

        await _bansRepository.DidNotReceive()
            .FirstOrDefaultAsync(
                Arg.Any<ISpecification<BanEntity>>(),
                Arg.Any<CancellationToken>());

        _mapper.DidNotReceive()
            .Map<BanDTO>(Arg.Any<BanEntity>());
    }

    [Fact]
    public async Task Handle_ShouldMapBanToBanDTO_WhenBanExists()
    {
        var query = new GetBanByIdQuery(1, 10);
        var adminUser = CreateAdminUser(1);
        var ban = CreateBanEntity(10, 2, 1);

        var expectedDTO = new BanDTO(10, 2, "Reason test", 1, ban.IssuedAt, ban.ExpiresAt, true);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);

        _bansRepository.FirstOrDefaultAsync(
                Arg.Any<ISpecification<BanEntity>>(),
                Arg.Any<CancellationToken>())
            .Returns(ban);

        _mapper.Map<BanDTO>(ban)
            .Returns(expectedDTO);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Id.ShouldBe(10);
        result.Value.UserId.ShouldBe(2);
        result.Value.IssuedBy.ShouldBe(1);
        result.Value.Reason.ShouldBe("Reason test");

        _mapper.Received(1).Map<BanDTO>(ban);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var cts = new CancellationTokenSource();
        var query = new GetBanByIdQuery(1, 10);
        var adminUser = CreateAdminUser(1);
        var ban = CreateBanEntity(10, 2, 1);

        var expectedDto = new BanDTO(10, 2, "Reason test", 1, ban.IssuedAt, ban.ExpiresAt, true);

        _usersRepository.GetByIdAsync(1, cts.Token)
            .Returns(adminUser);

        _bansRepository.FirstOrDefaultAsync(
                Arg.Any<ISpecification<BanEntity>>(),
                cts.Token)
            .Returns(ban);

        _mapper.Map<BanDTO>(ban)
            .Returns(expectedDto);

        await _handler.Handle(query, cts.Token);

        await _usersRepository.Received(1)
            .GetByIdAsync(1, cts.Token);

        await _bansRepository.Received(1)
            .FirstOrDefaultAsync(
                Arg.Any<ISpecification<BanEntity>>(),
                cts.Token);
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
