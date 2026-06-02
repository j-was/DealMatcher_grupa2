namespace DealMatcher.Backend.UnitTests.UseCases.Mapping;

public class BanProfileTests
{
    private readonly IMapper _mapper;

    public BanProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BanProfile>();
        }, new SerilogLoggerFactory());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Configuration_IsValid()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BanProfile>();
        }, new SerilogLoggerFactory());

        configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_BanToBanDTO_MapsBasicPropertiesCorrectly()
    {
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var ban = CreateBanEntity(expiresAt);
        typeof(BanEntity).GetProperty("Id")!.SetValue(ban, 1);

        var dto = _mapper.Map<BanDTO>(ban);

        dto.Id.ShouldBe(1);
        dto.UserId.ShouldBe(ban.UserId);
        dto.Reason.ShouldBe(ban.Reason);
        dto.IssuedBy.ShouldBe(ban.IssuedBy);
        dto.IssuedAt.ShouldBe(ban.IssuedAt);
        dto.ExpiresAt.ShouldBe(ban.ExpiresAt);
        dto.IsActive.ShouldBe(ban.IsActive);
    }

    [Fact]
    public void Map_BanToBanDTO_MapsPermamentBanCorrectly()
    {
        var ban = CreateBanEntity();
        typeof(BanEntity).GetProperty("Id")!.SetValue(ban, 2);

        var dto = _mapper.Map<BanDTO>(ban);

        dto.Id.ShouldBe(2);
        dto.ExpiresAt.ShouldBeNull();
    }

    private static BanEntity CreateBanEntity(DateTime? expiresAt = null)
    {
        return new BanEntity(
            1,
            "Reason test",
            2,
            expiresAt
        );
    }
}
