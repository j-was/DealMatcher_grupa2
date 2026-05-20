namespace DealMatcher.Backend.UnitTests.Core.ActivityAggregate;

public class ActivityDetailTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateActivityDetail()
    {
        var detail = new ActivityDetail("Browser", "Firefox");

        detail.Name.ShouldBe("Browser");
        detail.Value.ShouldBe("Firefox");
    }

    [Fact]
    public void Constructor_ShouldTrimNameAndValue()
    {
        var detail = new ActivityDetail("  Browser  ", "  Chrome  ");

        detail.Name.ShouldBe("Browser");
        detail.Value.ShouldBe("Chrome");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_ShouldThrow_WhenNameIsInvalid(string invalidName)
    {
        Should.Throw<ArgumentException>(() =>
            new ActivityDetail(invalidName, "value"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_ShouldThrow_WhenValueIsInvalid(string invalidValue)
    {
        Should.Throw<ArgumentException>(() =>
            new ActivityDetail("name", invalidValue));
    }
}
