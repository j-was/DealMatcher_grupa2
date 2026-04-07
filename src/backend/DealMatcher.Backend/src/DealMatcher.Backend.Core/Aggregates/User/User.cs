namespace DealMatcher.Backend.Core.Aggregates.User;


public sealed class User :
  DealMatcherEntityBase,
  IAggregateRoot
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Surname { get; private set; }
    public UserStatus Status { get; private set; }
    public string PasswordHash { get; private set; }


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

    public void SetNewHash(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

}
