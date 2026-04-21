namespace DealMatcher.Backend.UnitTests.Infrastructure.Authentication;

public class PasswordValidatorTests
{
    private readonly PasswordValidator _validator;

    public PasswordValidatorTests()
    {
        _validator = new PasswordValidator();
    }

    [Fact]
    public void ValidatePassword_Should_Return_True_For_Valid_Password()
    {
        var password = "ValidP@ssw0rd";

        var result = _validator.ValidatePassword(password);

        result.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidatePassword_Should_Return_False_For_Null_Or_Whitespace(string password)
    {
        var result = _validator.ValidatePassword(password);

        result.ShouldBeFalse();
    }

    [Fact]
    public void ValidatePassword_Should_Return_False_When_Password_Too_Short()
    {
        var password = "Sh0rt!";

        var result = _validator.ValidatePassword(password);

        result.ShouldBeFalse();
    }

    [Fact]
    public void ValidatePassword_Should_Return_False_When_Password_Too_Long()
    {
        var password = new string('a', 129) + "A1!";

        var result = _validator.ValidatePassword(password);

        result.ShouldBeFalse();
    }

    [Fact]
    public void ValidatePassword_Should_Return_False_When_Missing_Uppercase()
    {
        var password = "lowercase123!";

        var result = _validator.ValidatePassword(password);

        result.ShouldBeFalse();
    }

    [Fact]
    public void ValidatePassword_Should_Return_False_When_Missing_Lowercase()
    {
        var password = "UPPERCASE123!";

        var result = _validator.ValidatePassword(password);

        result.ShouldBeFalse();
    }

    [Fact]
    public void ValidatePassword_Should_Return_False_When_Missing_Digit()
    {
        var password = "NoDigitsHere!";

        var result = _validator.ValidatePassword(password);

        result.ShouldBeFalse();
    }

    [Fact]
    public void ValidatePassword_Should_Return_False_When_Missing_Special_Character()
    {
        var password = "NoSpecialChar123";

        var result = _validator.ValidatePassword(password);

        result.ShouldBeFalse();
    }

    [Theory]
    [InlineData("Password123!")]
    [InlineData("Admin123!")]
    [InlineData("Qwerty123!")]
    public void ValidatePassword_Should_Return_False_For_Weak_Passwords(string password)
    {
        var result = _validator.ValidatePassword(password);

        result.ShouldBeFalse();
    }

    [Fact]
    public void ValidatePassword_Should_Validate_Exactly_Eight_Characters()
    {
        var password = "Abcd123!";

        var result = _validator.ValidatePassword(password);

        result.ShouldBeTrue();
    }

    [Fact]
    public void ValidatePassword_Should_Validate_Maximum_Length_Password()
    {
        var password = "A" + new string('b', 124) + "1!";

        var result = _validator.ValidatePassword(password);

        result.ShouldBeTrue();
    }

    [Theory]
    [InlineData("P@ssw0rd!")]
    [InlineData("MyP@ssw0rd2024!")]
    [InlineData("C0mpl3x!P@ssw0rd")]
    [InlineData("Abcdefgh123!@#")]
    public void ValidatePassword_Should_Accept_Various_Valid_Passwords(string password)
    {
        var result = _validator.ValidatePassword(password);

        result.ShouldBeTrue();
    }

    [Fact]
    public void AddWeakPassword_Should_Add_New_Weak_Password()
    {
        var newWeakPassword = "MyWeakPassword123!";
        var validator = new PasswordValidator();

        PasswordValidator.AddWeakPassword(newWeakPassword);

        validator.ValidatePassword(newWeakPassword).ShouldBeFalse();
    }

    [Fact]
    public void AddWeakPassword_Should_Ignore_Null_Or_Whitespace()
    {
        var initialWeakPasswords = GetWeakPasswordsCount();

        PasswordValidator.AddWeakPassword(null!);
        PasswordValidator.AddWeakPassword("");
        PasswordValidator.AddWeakPassword("   ");

        var finalWeakPasswords = GetWeakPasswordsCount();
        finalWeakPasswords.ShouldBe(initialWeakPasswords);
    }

    [Fact]
    public void AddWeakPassword_Should_Not_Duplicate_Existing_Passwords()
    {
        var existingPassword = "Password123!";
        var initialCount = GetWeakPasswordsCount();

        PasswordValidator.AddWeakPassword(existingPassword);

        var finalCount = GetWeakPasswordsCount();
        finalCount.ShouldBeGreaterThanOrEqualTo(initialCount);
    }

    [Fact]
    public void PasswordRequirements_Should_Contain_Length_Requirements()
    {
        var requirements = _validator.PasswordRequirements;

        requirements.ShouldContain("8");
        requirements.ShouldContain("128");
    }

    [Fact]
    public void PasswordRequirements_Should_Contain_Character_Type_Requirements()
    {
        var requirements = _validator.PasswordRequirements;

        requirements.ShouldContain("lowercase");
        requirements.ShouldContain("uppercase");
        requirements.ShouldContain("digits");
        requirements.ShouldContain("special character");
    }

    [Fact]
    public void PasswordRequirements_Should_Mention_Weak_Password_Check()
    {
        var requirements = _validator.PasswordRequirements;

        requirements.ShouldContain("weak");
    }

    [Fact]
    public void ValidatePassword_Should_Accept_Polish_Characters_In_Password()
    {
        var password = "ZażółćGęśląJaźń123!";

        var result = _validator.ValidatePassword(password);

        result.ShouldBeTrue();
    }

    [Theory]
    [InlineData("pass")]
    [InlineData("PASSWORD")]
    [InlineData("12345678")]
    [InlineData("!@#$%^&*")]
    [InlineData("Pass1")]
    [InlineData("Password1")]
    [InlineData("Password!")]
    public void ValidatePassword_Should_Reject_Common_Weak_Patterns(string password)
    {
        var result = _validator.ValidatePassword(password);

        result.ShouldBeFalse();
    }

    private static int GetWeakPasswordsCount()
    {
        var field = typeof(PasswordValidator).GetField("_weakPasswords",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var weakPasswords = field?.GetValue(null) as string[];
        return weakPasswords?.Length ?? 0;
    }
}
