namespace DealMatcher.Backend.UnitTests.Infrastructure.Authentication;

public class JwtTokenProviderTests
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtTokenProvider> _logger;
    private readonly JwtTokenProvider _tokenProvider;
    private const string TestSecretKey = "this-is-a-very-long-secret-key-for-testing-purposes-12345";
    private const string TestIssuer = "https://test-issuer.com";
    private const string TestAudience = "https://test-audience.com";

    public JwtTokenProviderTests()
    {
        _configuration = Substitute.For<IConfiguration>();
        _logger = Substitute.For<ILogger<JwtTokenProvider>>();

        _configuration["Authentication:Jwt:SecretKey"].Returns(TestSecretKey);
        _configuration["Authentication:Jwt:Issuer"].Returns(TestIssuer);
        _configuration["Authentication:Jwt:Audience"].Returns(TestAudience);

        _tokenProvider = new JwtTokenProvider(_configuration, _logger);
    }

    [Fact]
    public void Constructor_Should_Throw_When_SecretKey_Not_Configured()
    {
        var config = Substitute.For<IConfiguration>();
        config["Authentication:Jwt:SecretKey"].Returns((string?)null);
        config["Authentication:Jwt:Issuer"].Returns(TestIssuer);
        config["Authentication:Jwt:Audience"].Returns(TestAudience);

        Should.Throw<InvalidOperationException>(() => new JwtTokenProvider(config, _logger))
            .Message.ShouldContain("JWT SecretKey not configured");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Issuer_Not_Configured()
    {
        var config = Substitute.For<IConfiguration>();
        config["Authentication:Jwt:SecretKey"].Returns(TestSecretKey);
        config["Authentication:Jwt:Issuer"].Returns((string?)null);
        config["Authentication:Jwt:Audience"].Returns(TestAudience);

        Should.Throw<InvalidOperationException>(() => new JwtTokenProvider(config, _logger))
            .Message.ShouldContain("JWT Issuer not configured");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Audience_Not_Configured()
    {
        var config = Substitute.For<IConfiguration>();
        config["Authentication:Jwt:SecretKey"].Returns(TestSecretKey);
        config["Authentication:Jwt:Issuer"].Returns(TestIssuer);
        config["Authentication:Jwt:Audience"].Returns((string?)null);

        Should.Throw<InvalidOperationException>(() => new JwtTokenProvider(config, _logger))
            .Message.ShouldContain("JWT Audience not configured");
    }

    [Fact]
    public void GenerateToken_Should_Return_Valid_Jwt_Token()
    {
        var user = CreateTestUser();

        var token = _tokenProvider.GenerateToken(user);

        token.ShouldNotBeNullOrWhiteSpace();
        token.Split('.').Length.ShouldBe(3);
    }

    [Fact]
    public void GenerateToken_Should_Throw_When_User_Is_Null()
    {
        Should.Throw<ArgumentNullException>(() => _tokenProvider.GenerateToken(null!));
    }

    [Fact]
    public void GenerateToken_Should_Contain_User_Claims()
    {
        var user = CreateTestUser();

        var token = _tokenProvider.GenerateToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Claims.ShouldContain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == user.Id.ToString());
        jwtToken.Claims.ShouldContain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == user.Email);
        jwtToken.Claims.ShouldContain(c => c.Type == JwtRegisteredClaimNames.GivenName && c.Value == user.Name);
        jwtToken.Claims.ShouldContain(c => c.Type == JwtRegisteredClaimNames.FamilyName && c.Value == user.Surname);
    }

    [Fact]
    public void GenerateToken_Should_Contain_Jti_Claim()
    {
        var user = CreateTestUser();

        var token = _tokenProvider.GenerateToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Claims.ShouldContain(c => c.Type == JwtRegisteredClaimNames.Jti);
        jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public void GenerateToken_Should_Contain_Iat_Claim()
    {
        var user = CreateTestUser();

        var token = _tokenProvider.GenerateToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Claims.ShouldContain(c => c.Type == JwtRegisteredClaimNames.Iat);
    }

    [Fact]
    public void GenerateToken_Should_Set_Correct_Issuer_And_Audience()
    {
        var user = CreateTestUser();

        var token = _tokenProvider.GenerateToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Issuer.ShouldBe(TestIssuer);
        jwtToken.Audiences.ShouldContain(TestAudience);
    }

    [Fact]
    public void GenerateToken_Should_Set_Expiration_One_Hour_From_Now()
    {
        var user = CreateTestUser();
        var beforeGeneration = DateTime.UtcNow;

        var token = _tokenProvider.GenerateToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var expectedExpiration = beforeGeneration.AddHours(1);
        jwtToken.ValidTo.ShouldBeGreaterThan(beforeGeneration);
        jwtToken.ValidTo.ShouldBeLessThanOrEqualTo(expectedExpiration.AddMinutes(1));
    }

    [Fact]
    public void GenerateToken_Should_Generate_Different_Tokens_For_Different_Users()
    {
        var user1 = CreateTestUser(id: 1, email: "user1@test.com");
        var user2 = CreateTestUser(id: 2, email: "user2@test.com");

        var token1 = _tokenProvider.GenerateToken(user1);
        var token2 = _tokenProvider.GenerateToken(user2);

        token1.ShouldNotBe(token2);
    }

    [Fact]
    public void GenerateToken_Should_Generate_Different_Tokens_For_Same_User_Called_Twice()
    {
        var user = CreateTestUser();

        var token1 = _tokenProvider.GenerateToken(user);
        var token2 = _tokenProvider.GenerateToken(user);

        token1.ShouldNotBe(token2);
    }

    [Fact]
    public void GenerateToken_Should_Log_Information()
    {
        var user = CreateTestUser();

        _tokenProvider.GenerateToken(user);

        _logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains(user.Id.ToString())),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task ValidateTokenAsync_Should_Return_True_For_Valid_Token()
    {
        var user = CreateTestUser();
        var token = _tokenProvider.GenerateToken(user);

        var result = await _tokenProvider.ValidateTokenAsync(token);

        result.ShouldBeTrue();
    }

    [Fact]
    public async Task ValidateTokenAsync_Should_Throw_For_Null_Token()
    {
        await Should.ThrowAsync<ArgumentNullException>(() => _tokenProvider.ValidateTokenAsync(null!));
    }

    [Fact]
    public async Task ValidateTokenAsync_Should_Throw_For_Empty_Token()
    {
        await Should.ThrowAsync<ArgumentException>(() => _tokenProvider.ValidateTokenAsync(""));
    }

    [Fact]
    public async Task ValidateTokenAsync_Should_Throw_For_Invalid_Token()
    {
        var invalidToken = "invalid.token.string";

        await Should.ThrowAsync<Exception>(() => _tokenProvider.ValidateTokenAsync(invalidToken));
    }

    [Fact]
    public async Task ValidateTokenAsync_Should_Throw_For_Token_With_Invalid_Signature()
    {
        var user = CreateTestUser();
        var validToken = _tokenProvider.GenerateToken(user);
        var parts = validToken.Split('.');
        var tamperedToken = $"{parts[0]}.{parts[1]}.tampered_signature";

        await Should.ThrowAsync<Exception>(() => _tokenProvider.ValidateTokenAsync(tamperedToken));
    }

    [Fact]
    public async Task ValidateTokenAsync_Should_Throw_For_Token_With_Wrong_Issuer()
    {
        var configWithWrongIssuer = Substitute.For<IConfiguration>();
        configWithWrongIssuer["Authentication:Jwt:SecretKey"].Returns(TestSecretKey);
        configWithWrongIssuer["Authentication:Jwt:Issuer"].Returns("wrong-issuer");
        configWithWrongIssuer["Authentication:Jwt:Audience"].Returns(TestAudience);

        var wrongIssuerProvider = new JwtTokenProvider(configWithWrongIssuer, _logger);
        var user = CreateTestUser();
        var token = wrongIssuerProvider.GenerateToken(user);

        await Should.ThrowAsync<Exception>(() => _tokenProvider.ValidateTokenAsync(token));
    }

    private static User CreateTestUser(int id = 1, string email = "test@example.com", string name = "Test",
        string surname = "User")
    {
        var user = new User(email, name, surname);
        typeof(User).GetProperty("Id")!.SetValue(user, id);
        return user;
    }
}
