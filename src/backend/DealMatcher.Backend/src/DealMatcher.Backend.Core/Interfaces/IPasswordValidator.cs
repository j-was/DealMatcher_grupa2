namespace DealMatcher.Backend.Core.Interfaces;

public interface IPasswordValidator
{
    public bool ValidatePassword(string password);
    public string PasswordRequirements { get; }
}
