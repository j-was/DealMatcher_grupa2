namespace DealMatcher.Backend.UnitTests.Core.ActivityAggregate;

public class ActivityRecordTests
{
    [Fact]
    public void Constructor_WithoutOfferId_ShouldCreateActivityRecord()
    {
        var userId = 1;
        var action = ActionType.Login;
        var ipAddress = "192.168.1.1";
        var details = new List<ActivityDetail> { new("Browser", "Chrome"), new("OS", "Windows") };

        var record = new ActivityRecord(userId, action, ipAddress, details);

        record.UserId.ShouldBe(userId);
        record.OfferId.ShouldBeNull();
        record.Action.ShouldBe(action);
        record.IpAddress.ShouldBe(ipAddress);
        record.Details.ShouldBe(details);
        record.Details.Count.ShouldBe(2);
    }

    [Fact]
    public void Constructor_WithOfferId_ShouldCreateActivityRecord()
    {
        var userId = 1;
        var offerId = 5;
        var action = ActionType.Purchase;
        var ipAddress = "10.0.0.1";
        var details = new List<ActivityDetail> { new("Price", "99.99"), new("Quantity", "1") };

        var record = new ActivityRecord(userId, offerId, action, ipAddress, details);

        record.UserId.ShouldBe(userId);
        record.OfferId.ShouldBe(offerId);
        record.Action.ShouldBe(action);
        record.IpAddress.ShouldBe(ipAddress);
        record.Details.ShouldBe(details);
    }

    [Fact]
    public void Constructor_ShouldSetCreatedAt()
    {
        var before = DateTime.UtcNow;
        var record = new ActivityRecord(1, ActionType.View, "127.0.0.1", []);
        var after = DateTime.UtcNow;

        record.CreatedAt.ShouldBeGreaterThanOrEqualTo(before);
        record.CreatedAt.ShouldBeLessThanOrEqualTo(after);
    }
}
