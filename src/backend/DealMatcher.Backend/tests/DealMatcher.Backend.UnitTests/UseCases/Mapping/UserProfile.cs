namespace DealMatcher.Backend.UnitTests.UseCases.Mapping;

public class UserProfileTests
{
    private readonly IMapper _mapper;

    public UserProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserProfile>();
        }, new SerilogLoggerFactory());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Configuration_IsValid()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserProfile>();
        }, new SerilogLoggerFactory());

        configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_UserToUserDTO_MapsBasicPropertiesCorrectly()
    {
        var user = new UserEntity("john@example.com", "John", "Doe");
        typeof(UserEntity).GetProperty("Id")!.SetValue(user, 1);

        var dto = _mapper.Map<UserDTO>(user);

        dto.Id.ShouldBe(1);
        dto.Email.ShouldBe("john@example.com");
        dto.Name.ShouldBe("John");
        dto.Surname.ShouldBe("Doe");
        dto.CreatedAt.ShouldBe(user.CreatedAt);
    }

    [Fact]
    public void Map_UserToUserDTO_MapsStatusToUpperCase()
    {
        var user = new UserEntity("john@example.com", "John", "Doe");
        typeof(UserEntity).GetProperty("Status")!.SetValue(user, UserStatus.Active);

        var dto = _mapper.Map<UserDTO>(user);

        dto.Status.ShouldBe("ACTIVE");
    }

    [Fact]
    public void Map_UserToUserDTO_MapsAllStatusValuesToUpperCase()
    {
        var statuses = new[] { UserStatus.Active, UserStatus.Inactive, UserStatus.Banned };
        var expectedStatuses = new[] { "ACTIVE", "INACTIVE", "BANNED" };

        for (var i = 0; i < statuses.Length; i++)
        {
            var user = new UserEntity("test@example.com", "Test", "User");
            typeof(UserEntity).GetProperty("Status")!.SetValue(user, statuses[i]);

            var dto = _mapper.Map<UserDTO>(user);

            dto.Status.ShouldBe(expectedStatuses[i]);
        }
    }

    [Fact]
    public void Map_UserToUserDTO_Handles_Empty_Surname()
    {
        var user = new UserEntity("john@example.com", "John", "");

        var dto = _mapper.Map<UserDTO>(user);

        dto.Surname.ShouldBe("");
    }
}
