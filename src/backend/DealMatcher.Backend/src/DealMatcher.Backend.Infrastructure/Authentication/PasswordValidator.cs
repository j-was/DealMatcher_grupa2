namespace DealMatcher.Backend.Infrastructure.Authentication;

public class PasswordValidator: IPasswordValidator
{
    private const int MinLength = 8;
    private const int MaxLength = 128;
    private static readonly string[] WeakPasswords ={ "Password123!", "Admin123!", "Qwerty123!" };

    public void AddWeakPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return;
        }
        WeakPasswords.Append(password);
    }

    public string PasswordRequirements { get; } =
        $"Password must have between {MinLength} and {MaxLength} characters. " +
        $"Must contain lowercase and uppercase letters, digits and special character. " +
        $"Must not be too commonly weak.";

    public bool ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        if (password.Length < MinLength)
        {
            return false;
        }

        if (password.Length > MaxLength)
        {
            return false;
        }

        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            return false;
        }

        if (!Regex.IsMatch(password, @"[a-z]"))
        {
            return false;
        }

        if (!Regex.IsMatch(password, @"[0-9]"))
        {
            return false;
        }

        if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]"))
        {
            return false;
        }

        if (WeakPasswords.Contains(password))
        {
            return false;
        }

        return true;
    }
}
