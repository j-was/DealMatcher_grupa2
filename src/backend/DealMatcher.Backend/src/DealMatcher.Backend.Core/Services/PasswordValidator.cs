using DealMatcher.Backend.Core.Interfaces;

namespace DealMatcher.Backend.Core.Services;

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

    public bool ValidatePassword(string password, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(password))
        {
            errorMessage = "Password cannot be empty";
            return false;
        }

        if (password.Length < MinLength)
        {
            errorMessage = $"Password must be at least {MinLength} characters long";
            return false;
        }

        if (password.Length > MaxLength)
        {
            errorMessage = $"Password cannot exceed {MaxLength} characters";
            return false;
        }

        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            errorMessage = "Password must contain at least one uppercase letter";
            return false;
        }

        if (!Regex.IsMatch(password, @"[a-z]"))
        {
            errorMessage = "Password must contain at least one lowercase letter";
            return false;
        }

        if (!Regex.IsMatch(password, @"[0-9]"))
        {
            errorMessage = "Password must contain at least one number";
            return false;
        }

        if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]"))
        {
            errorMessage = "Password must contain at least one special character";
            return false;
        }

        if (WeakPasswords.Contains(password))
        {
            errorMessage = "Password is too common. Please choose a stronger password";
            return false;
        }

        return true;
    }
}
