namespace DealMatcher.Backend.UnitTests.UseCases.Features.Admin;

public class GetOfferActivityQueryHandlerTests
{
    private readonly IReadRepository<ActivityRecord> _activityRepository;
    private readonly IReadRepository<UserEntity> _usersRepository;
    private readonly IMapper _mapper;
    private readonly GetOfferActivityQueryHandler _handler;

    public GetOfferActivityQueryHandlerTests()
    {
        _activityRepository = Substitute.For<IReadRepository<ActivityRecord>>();
        _usersRepository = Substitute.For<IReadRepository<UserEntity>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetOfferActivityQueryHandler(_activityRepository, _usersRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithActivityRecords_WhenUserIsAdmin()
    {
        var query = new GetOfferActivityQuery(1, 1);
        var adminUser = CreateAdminUser(1);
        var records = new List<ActivityRecord>
        {
            new(1, 1, ActionType.View, "127.0.0.1", []),
            new(2, 1, ActionType.Purchase, "127.0.0.1", [])
        };
        var expectedDtos = new List<ActivityRecordDTO>
        {
            new(1, 1, 1, "VIEW", [], "127.0.0.1", DateTime.UtcNow),
            new(2, 2, 1, "PURCHASE", [], "127.0.0.1", DateTime.UtcNow)
        };

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);
        _activityRepository.ListAsync(Arg.Any<ActivityRecordsByOfferIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(records);
        _mapper.Map<List<ActivityRecordDTO>>(records)
            .Returns(expectedDtos);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(expectedDtos);

        await _usersRepository.Received(1).GetByIdAsync(1, Arg.Any<CancellationToken>());
        await _activityRepository.Received(1).ListAsync(
            Arg.Any<ActivityRecordsByOfferIdSpec>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<List<ActivityRecordDTO>>(records);
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        var query = new GetOfferActivityQuery(1, 1);
        var regularUser = new UserEntity("user@example.com", "John", "Doe");

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(regularUser);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);

        await _activityRepository.DidNotReceive().ListAsync(
            Arg.Any<ActivityRecordsByOfferIdSpec>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenUserDoesNotExist()
    {
        var query = new GetOfferActivityQuery(1, 1);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns((UserEntity?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenNoActivityRecords()
    {
        var query = new GetOfferActivityQuery(1, 1);
        var adminUser = CreateAdminUser(1);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);
        _activityRepository.ListAsync(Arg.Any<ActivityRecordsByOfferIdSpec>(), Arg.Any<CancellationToken>())
            .Returns([]);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var query = new GetOfferActivityQuery(1, 1);
        var cts = new CancellationTokenSource();
        var adminUser = CreateAdminUser(1);
        var records = new List<ActivityRecord>
        {
            new(1, 1, ActionType.View, "127.0.0.1", [])
        };

        _usersRepository.GetByIdAsync(1, cts.Token)
            .Returns(adminUser);
        _activityRepository.ListAsync(Arg.Any<ActivityRecordsByOfferIdSpec>(), cts.Token)
            .Returns(records);
        _mapper.Map<List<ActivityRecordDTO>>(records)
            .Returns([]);

        await _handler.Handle(query, cts.Token);

        await _usersRepository.Received(1).GetByIdAsync(1, cts.Token);
        await _activityRepository.Received(1).ListAsync(
            Arg.Any<ActivityRecordsByOfferIdSpec>(), cts.Token);
    }

    private static UserEntity CreateAdminUser(int id)
    {
        var user = new UserEntity("admin@example.com", "Admin", "User");
        typeof(UserEntity).GetProperty("Status")!.SetValue(user, UserStatus.Admin);
        typeof(UserEntity).GetProperty("Id")!.SetValue(user, id);
        return user;
    }
}
