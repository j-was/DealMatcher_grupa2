namespace DealMatcher.Backend.UnitTests.UseCases.Features.Admin;

public class GetUserAdminQueryHandlerTests
{
    private readonly IRepository<UserEntity> _usersRepository;
    private readonly IRepository<OfferEntity> _offersRepository;
    private readonly IRepository<ActivityRecord> _activityRepository;
    private readonly GetUserAdminQueryHandler _handler;

    public GetUserAdminQueryHandlerTests()
    {
        _usersRepository = Substitute.For<IRepository<UserEntity>>();
        _offersRepository = Substitute.For<IRepository<OfferEntity>>();
        _activityRepository = Substitute.For<IRepository<ActivityRecord>>();
        _handler = new GetUserAdminQueryHandler(_usersRepository, _offersRepository, _activityRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithUserDetails_WhenUserIsAdmin()
    {
        var query = new GetUserAdminQuery(2, 1);
        var adminUser = CreateAdminUser(1);
        var targetUser = CreateUser(2, "target@example.com");
        var offers = new List<OfferEntity>
        {
            CreateOfferEntity(1, 2),
            CreateOfferEntity(2, 2)
        };
        var activities = new List<ActivityRecord>
        {
            new(2, 1, ActionType.Purchase, "127.0.0.1", []),
            new(2, ActionType.Login, "127.0.0.1", [])
        };

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);
        _usersRepository.GetByIdAsync(2, Arg.Any<CancellationToken>())
            .Returns(targetUser);
        _offersRepository.ListAsync(Arg.Any<OffersBySellerIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(offers);
        _activityRepository.ListAsync(Arg.Any<ActivityRecordsByUserIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(activities);
        _activityRepository.ListAsync(Arg.Any<ActivityRecordsByOfferIdSpec>(), Arg.Any<CancellationToken>())
            .Returns([new ActivityRecord(3, 1, ActionType.Purchase, "127.0.0.1", [])]);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldBe(2);
        result.Value.Email.ShouldBe("target@example.com");
        result.Value.TotalOffers.ShouldBe(2);
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        var query = new GetUserAdminQuery(2, 1);
        var regularUser = new UserEntity("user@example.com", "John", "Doe");

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(regularUser);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenTargetUserDoesNotExist()
    {
        var query = new GetUserAdminQuery(999, 1);
        var adminUser = CreateAdminUser(1);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);
        _usersRepository.GetByIdAsync(999, Arg.Any<CancellationToken>())
            .Returns((UserEntity?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldCalculateTotalPurchasesCorrectly()
    {
        var query = new GetUserAdminQuery(2, 1);
        var adminUser = CreateAdminUser(1);
        var targetUser = CreateUser(2, "seller@example.com");
        var offers = new List<OfferEntity> { CreateOfferEntity(1, 2) };
        var userActivities = new List<ActivityRecord>
        {
            new(2, ActionType.Login, "127.0.0.1", []),
            new(2, 1, ActionType.Purchase, "127.0.0.1", []),
            new(2, 1, ActionType.Purchase, "127.0.0.1", []),
            new(2, 1, ActionType.View, "127.0.0.1", [])
        };
        var offerActivities = new List<ActivityRecord>
        {
            new(2, 1, ActionType.Purchase, "127.0.0.1", []),
            new(2, 1, ActionType.Purchase, "127.0.0.1", []),
            new(2, 1, ActionType.View, "127.0.0.1", [])
        };

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(adminUser);
        _usersRepository.GetByIdAsync(2, Arg.Any<CancellationToken>())
            .Returns(targetUser);
        _offersRepository.ListAsync(Arg.Any<OffersBySellerIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(offers);
        _activityRepository.ListAsync(Arg.Any<ActivityRecordsByUserIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(userActivities);
        _activityRepository.ListAsync(Arg.Any<ActivityRecordsByOfferIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(offerActivities);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.TotalPurchases.ShouldBe(2);
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenRequestingUserDoesNotExist()
    {
        var query = new GetUserAdminQuery(2, 1);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns((UserEntity?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);
    }

    private static UserEntity CreateAdminUser(int id)
    {
        var user = new UserEntity("admin@example.com", "Admin", "User");
        typeof(UserEntity).GetProperty("Status")!.SetValue(user, UserStatus.Admin);
        typeof(UserEntity).GetProperty("Id")!.SetValue(user, id);
        return user;
    }

    private static UserEntity CreateUser(int id, string email)
    {
        var user = new UserEntity(email, "User", "Name");
        typeof(UserEntity).GetProperty("Id")!.SetValue(user, id);
        return user;
    }

    private static OfferEntity CreateOfferEntity(int id, int sellerId)
    {
        var offer = new OfferEntity("Offer", "Description", 100m, ["image.jpg"], sellerId,
            [], 1, [new OfferProperty("Color", "Red")], 10);
        typeof(OfferEntity).GetProperty("Id")!.SetValue(offer, id);
        return offer;
    }
}
