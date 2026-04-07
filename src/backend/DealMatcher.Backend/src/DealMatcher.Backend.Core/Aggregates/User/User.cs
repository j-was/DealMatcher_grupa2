namespace DealMatcher.Backend.Core.Aggregates.User;


public sealed class User :
  DealMatcherEntityBase,
  IAggregateRoot
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Surname { get; private set; }
    public UserStatus Status { get; private set; }
    private string PasswordHash { get; set; }


    public User(string email, string name, string surname = "")
    {
        Name = name;
        Email = email;
        Surname = surname;
        Status = UserStatus.Active;
        PasswordHash = "";
    }

#pragma warning disable CS8618
    private User()
    {
        /* EF */
    }
#pragma warning restore CS8618

    public bool AuthenticatePassword(string password)
    {
        PasswordHash = password;
        return false;
    }

    public bool HashPassword(string password)
    {
        PasswordHash = BCrypt.HashPassword(password, BCrypt.GenerateSalt(12));
        return false;
    }

}
