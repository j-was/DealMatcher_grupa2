namespace DealMatcher.Backend.UnitTests.Core.UserAggregate;

public class UserTests
{
    [Fact]
    public void CreateUser_WithValidData_ShouldCreateActiveUser()
    {
        var email = "john.doe@example.com";
        var name = "John";
        var surname = "Doe";

        var user = new User(email, name, surname);

        user.Email.ShouldBe(email);
        user.Name.ShouldBe(name);
        user.Surname.ShouldBe(surname);
        user.Status.ShouldBe(UserStatus.Active);
        user.PasswordHash.ShouldBeEmpty();
    }

    [Fact]
    public void CreateUser_WithEmptySurname_ShouldCreateUserWithEmptySurname()
    {
        var email = "john@example.com";
        var name = "John";
        var surname = "";

        var user = new User(email, name, surname);

        user.Surname.ShouldBeEmpty();
        user.Status.ShouldBe(UserStatus.Active);
    }

    [Fact]
    public void SetNewHash_WithValidHash_ShouldUpdatePasswordHash()
    {
        var user = new User("test@example.com", "Test", "User");
        var newHash = "hashed_password_123";

        user.SetNewHash(newHash);

        user.PasswordHash.ShouldBe(newHash);
    }

    [Fact]
    public void UserStatus_Values_ShouldBeCorrect()
    {
        UserStatus.Active.Value.ShouldBe("Active");
        UserStatus.Inactive.Value.ShouldBe("Inactive");
        UserStatus.Banned.Value.ShouldBe("Banned");
    }

    [Fact]
    public void UserStatus_FromValue_ShouldReturnCorrectStatus()
    {
        var active = UserStatus.FromValue("Active");
        var inactive = UserStatus.FromValue("Inactive");
        var banned = UserStatus.FromValue("Banned");

        active.ShouldBe(UserStatus.Active);
        inactive.ShouldBe(UserStatus.Inactive);
        banned.ShouldBe(UserStatus.Banned);
    }
}
