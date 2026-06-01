namespace DealMatcher.Backend.Infrastructure.Authentication;

public partial class PasswordValidator : IPasswordValidator
{
    private const int MinLength = DataSchemaConstants.UserPasswordMinLength;
    private const int MaxLength = 128;
    private static string[] _weakPasswords = ["Password123!", "Admin123!", "Qwerty123!"];

    public static void AddWeakPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        _weakPasswords = [.. _weakPasswords, password];
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

        if (!UppercaseRegex().IsMatch(password))
        {
            return false;
        }

        if (!LowercaseRegex().IsMatch(password))
        {
            return false;
        }

        if (!DigitRegex().IsMatch(password))
        {
            return false;
        }

        if (!SpecialCharacterRegex().IsMatch(password))
        {
            return false;
        }

        if (_weakPasswords.Contains(password))
        {
            return false;
        }

        return true;
    }

    [GeneratedRegex(@"[A-Z]")]
    private static partial Regex UppercaseRegex();

    [GeneratedRegex(@"[a-z]")]
    private static partial Regex LowercaseRegex();

    [GeneratedRegex(@"[0-9]")]
    private static partial Regex DigitRegex();

    [GeneratedRegex(@"[!@#$%^&*(),.?""':{}|<>]")]
    private static partial Regex SpecialCharacterRegex();
}
