namespace DealMatcher.Backend.UnitTests.Infrastructure.Authentication;

public class BcryptHashServiceTests
{
    private readonly BcryptHashService _service;

    public BcryptHashServiceTests()
    {
        _service = new BcryptHashService();
    }

    [Fact]
    public void HashPassword_Should_Return_NonEmpty_String()
    {
        var password = "TestPassword123!";

        var hash = _service.HashPassword(password);

        hash.ShouldNotBeNullOrWhiteSpace();
        hash.ShouldNotBe(password);
    }

    [Fact]
    public void HashPassword_Should_Generate_Different_Hashes_For_Same_Password()
    {
        var password = "TestPassword123!";

        var hash1 = _service.HashPassword(password);
        var hash2 = _service.HashPassword(password);

        hash1.ShouldNotBe(hash2);
    }

    [Fact]
    public void HashPassword_Should_Generate_Valid_BCrypt_Hash()
    {
        var password = "TestPassword123!";

        var hash = _service.HashPassword(password);

        hash.ShouldStartWith("$2a$12$");
    }

    [Theory]
    [InlineData("simple")]
    [InlineData("P@ssw0rd!2024")]
    [InlineData("VeryLongPasswordThatExceedsNormalLength123!@#")]
    [InlineData("12345")]
    [InlineData("!@#$%^&*()")]
    public void HashPassword_Should_Handle_Various_Password_Formats(string password)
    {
        var hash = _service.HashPassword(password);

        hash.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public void AuthorizePassword_Should_Return_True_For_Correct_Password()
    {
        var password = "CorrectPassword123!";
        var hash = _service.HashPassword(password);

        var result = _service.AuthorizePassword(hash, password);

        result.ShouldBeTrue();
    }

    [Fact]
    public void AuthorizePassword_Should_Return_False_For_Incorrect_Password()
    {
        var password = "CorrectPassword123!";
        var wrongPassword = "WrongPassword456!";
        var hash = _service.HashPassword(password);

        var result = _service.AuthorizePassword(hash, wrongPassword);

        result.ShouldBeFalse();
    }

    [Theory]
    [InlineData("", "password")]
    [InlineData("hash", "")]
    [InlineData("   ", "password")]
    [InlineData("hash", "   ")]
    public void AuthorizePassword_Should_Return_False_When_Inputs_Are_Null_Or_Whitespace(string hash, string password)
    {
        var result = _service.AuthorizePassword(hash, password);

        result.ShouldBeFalse();
    }

    [Fact]
    public void AuthorizePassword_Should_Return_False_For_Invalid_Hash_Format()
    {
        var password = "TestPassword123!";
        var invalidHash = "invalid-hash-format";

        var result = _service.AuthorizePassword(invalidHash, password);

        result.ShouldBeFalse();
    }

    [Fact]
    public void AuthorizePassword_Should_Return_False_For_Empty_Hash()
    {
        var password = "TestPassword123!";
        var emptyHash = "";

        var result = _service.AuthorizePassword(emptyHash, password);

        result.ShouldBeFalse();
    }

    [Fact]
    public void AuthorizePassword_Should_Be_Case_Sensitive()
    {
        var password = "Password123!";
        var wrongCasePassword = "password123!";
        var hash = _service.HashPassword(password);

        var result = _service.AuthorizePassword(hash, wrongCasePassword);

        result.ShouldBeFalse();
    }

    [Fact]
    public void AuthorizePassword_Should_Handle_Special_Characters_In_Password()
    {
        var password = "P@$$w0rd!@#$%^&*()";
        var hash = _service.HashPassword(password);

        var result = _service.AuthorizePassword(hash, password);

        result.ShouldBeTrue();
    }

    [Fact]
    public void AuthorizePassword_Should_Handle_Unicode_Characters()
    {
        var password = "P@ssw0rdŁódź2024!";
        var hash = _service.HashPassword(password);

        var result = _service.AuthorizePassword(hash, password);

        result.ShouldBeTrue();
    }
}
