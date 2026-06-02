namespace DealMatcher.Backend.UnitTests.UseCases.Mapping;

public class ActivityRecordProfileTests
{
    private readonly IMapper _mapper;

    public ActivityRecordProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ActivityRecordProfile>();
        }, new SerilogLoggerFactory());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Configuration_IsValid()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ActivityRecordProfile>();
        }, new SerilogLoggerFactory());

        configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_ActivityRecordToActivityRecordDTO_MapsBasicPropertiesCorrectly()
    {
        var details = new List<ActivityDetail> { new("Browser", "Chrome"), new("OS", "Windows") };
        var record = new ActivityRecord(1, 5, ActionType.Purchase, "192.168.1.1", details);
        typeof(ActivityRecord).GetProperty("Id")!.SetValue(record, 100);

        var dto = _mapper.Map<ActivityRecordDTO>(record);

        dto.Id.ShouldBe(100);
        dto.UserId.ShouldBe(1);
        dto.OfferId.ShouldBe(5);
        dto.Action.ShouldBe("PURCHASE");
        dto.IpAddress.ShouldBe("192.168.1.1");
        dto.CreatedAt.ShouldBe(record.CreatedAt);
    }

    [Fact]
    public void Map_ActivityRecordToActivityRecordDTO_MapsActionToUpperCase()
    {
        var record = new ActivityRecord(1, ActionType.View, "127.0.0.1", []);

        var dto = _mapper.Map<ActivityRecordDTO>(record);

        dto.Action.ShouldBe("VIEW");
    }

    [Fact]
    public void Map_ActivityRecordToActivityRecordDTO_MapsDetailsToDictionary()
    {
        var details = new List<ActivityDetail> { new("Price", "99.99"), new("Quantity", "2") };
        var record = new ActivityRecord(1, 5, ActionType.Purchase, "10.0.0.1", details);

        var dto = _mapper.Map<ActivityRecordDTO>(record);

        dto.Details.ShouldNotBeNull();
        dto.Details.Count.ShouldBe(2);
        dto.Details["Price"].ShouldBe("99.99");
        dto.Details["Quantity"].ShouldBe("2");
    }

    [Fact]
    public void Map_ActivityRecordToActivityRecordDTO_WithNullOfferId_MapsCorrectly()
    {
        var record = new ActivityRecord(1, ActionType.Login, "192.168.1.1", []);

        var dto = _mapper.Map<ActivityRecordDTO>(record);

        dto.OfferId.ShouldBeNull();
    }

    [Fact]
    public void Map_ActivityRecordToActivityRecordDTO_MapsAllActionTypesToUpperCase()
    {
        var actions = new[]
        {
            ActionType.Create, ActionType.Update, ActionType.Delete, ActionType.Status_Change, ActionType.Login,
            ActionType.Logout
        };
        var expected = new[] { "CREATE", "UPDATE", "DELETE", "STATUS_CHANGE", "LOGIN", "LOGOUT" };

        for (var i = 0; i < actions.Length; i++)
        {
            var record = new ActivityRecord(1, actions[i], "127.0.0.1", []);
            var dto = _mapper.Map<ActivityRecordDTO>(record);
            dto.Action.ShouldBe(expected[i]);
        }
    }
}
