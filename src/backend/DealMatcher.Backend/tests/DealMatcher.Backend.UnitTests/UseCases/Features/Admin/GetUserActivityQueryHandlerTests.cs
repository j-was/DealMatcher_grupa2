namespace DealMatcher.Backend.UnitTests.UseCases.Features.Admin;

public class GetUserActivityQueryHandlerTests
{
    private readonly IReadRepository<UserEntity> _usersRepository;
    private readonly IReadRepository<ActivityRecord> _activityRepository;
    private readonly IMapper _mapper;
    private readonly GetUserActivityQueryHandler _handler;

    public GetUserActivityQueryHandlerTests()
    {
        _usersRepository = Substitute.For<IReadRepository<UserEntity>>();
        _activityRepository = Substitute.For<IReadRepository<ActivityRecord>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetUserActivityQueryHandler(_usersRepository, _activityRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithActivityRecords_WhenUserIsAdmin()
    {
        var query = new GetUserActivityQuery(1, 2);
        var adminUser = CreateAdminUser(2);
        var records = new List<ActivityRecord>
        {
            new(1, ActionType.Login, "192.168.1.1", []), new(1, 5, ActionType.Purchase, "192.168.1.1", [])
        };
        var expectedDtos = new List<ActivityRecordDTO>
        {
            new(1, 2, null, "LOGIN", [], "192.168.1.1", DateTime.UtcNow),
            new(2, 2, 5, "PURCHASE", [], "192.168.1.1", DateTime.UtcNow)
        };

        _usersRepository.GetByIdAsync(2, Arg.Any<CancellationToken>())
            .Returns(adminUser);
        _activityRepository.ListAsync(Arg.Any<ActivityRecordsByUserIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(records);
        _mapper.Map<List<ActivityRecordDTO>>(records)
            .Returns(expectedDtos);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(expectedDtos);

        await _usersRepository.Received(1).GetByIdAsync(2, Arg.Any<CancellationToken>());
        await _activityRepository.Received(1).ListAsync(
            Arg.Any<ActivityRecordsByUserIdSpec>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<List<ActivityRecordDTO>>(records);
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        var query = new GetUserActivityQuery(1, 2);
        var regularUser = new UserEntity("user@example.com", "John", "Doe");

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(regularUser);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenNoActivityRecords()
    {
        var query = new GetUserActivityQuery(1, 2);
        var adminUser = CreateAdminUser(2);

        _usersRepository.GetByIdAsync(2, Arg.Any<CancellationToken>())
            .Returns(adminUser);

        _activityRepository.ListAsync(Arg.Any<ActivityRecordsByUserIdSpec>(), Arg.Any<CancellationToken>())
            .Returns([]);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var query = new GetUserActivityQuery(1, 2);
        var cts = new CancellationTokenSource();
        var adminUser = CreateAdminUser(2);
        var records = new List<ActivityRecord> { new(1, ActionType.View, "127.0.0.1", []) };

        _usersRepository.GetByIdAsync(2, cts.Token)
            .Returns(adminUser);
        _activityRepository.ListAsync(Arg.Any<ActivityRecordsByUserIdSpec>(), cts.Token)
            .Returns(records);
        _mapper.Map<List<ActivityRecordDTO>>(records)
            .Returns([]);

        await _handler.Handle(query, cts.Token);

        await _usersRepository.Received(1).GetByIdAsync(2, cts.Token);
        await _activityRepository.Received(1).ListAsync(
            Arg.Any<ActivityRecordsByUserIdSpec>(), cts.Token);
    }

    private static UserEntity CreateAdminUser(int id)
    {
        var user = new UserEntity("admin@example.com", "Admin", "User");
        typeof(UserEntity).GetProperty("Status")!.SetValue(user, UserStatus.Admin);
        typeof(UserEntity).GetProperty("Id")!.SetValue(user, id);
        return user;
    }
}
