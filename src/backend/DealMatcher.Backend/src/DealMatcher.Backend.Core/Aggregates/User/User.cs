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
        ValidateEmail(email);
        ValidateName(name);
        ValidateSurname(surname);

        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
        Surname = surname.Trim();
        Status = UserStatus.Active;
        PasswordHash = "";
    }

#pragma warning disable CS8618
    private User()
    {
        /* EF */
    }
#pragma warning restore CS8618

    private static void ValidateEmail(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        if (email.Length > DataSchemaConstants.UserEmailMaxLength)
            throw new ArgumentException($"Email cannot exceed {DataSchemaConstants.UserEmailMaxLength} characters.");

        // Basic email format validation
        if (!email.Contains('@') || !email.Contains('.'))
            throw new ArgumentException("Invalid email format.");
    }

    private static void ValidateName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (name.Length > DataSchemaConstants.UserNameMaxLength)
            throw new ArgumentException($"Name cannot exceed {DataSchemaConstants.UserNameMaxLength} characters.");
    }

    private static void ValidateSurname(string surname)
    {
        if (string.IsNullOrWhiteSpace(surname)) return;

        if (surname.Length > DataSchemaConstants.UserSurnameMaxLength)
            throw new ArgumentException($"Surname cannot exceed {DataSchemaConstants.UserSurnameMaxLength} characters.");
    }

    public void SetNewHash(string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        // if (passwordHash.Length != DataSchemaConstants.UserPasswordHashLength)
        //     throw new ArgumentException($"Password hash must be exactly {DataSchemaConstants.UserPasswordHashLength} characters.");

        PasswordHash = passwordHash;
    }

    public void UpdateProfile(string name, string surname)
    {
        ValidateName(name);
        ValidateSurname(surname);

        Name = name.Trim();
        Surname = surname.Trim();
    }

    public void UpdateStatus(UserStatus status)
    {
        Status = status;
    }
}
