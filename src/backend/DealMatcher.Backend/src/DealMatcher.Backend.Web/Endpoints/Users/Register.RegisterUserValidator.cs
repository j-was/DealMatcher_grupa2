using DealMatcher.Backend.Core.Interfaces;
using Org.BouncyCastle.Crypto.Engines;

namespace DealMatcher.Backend.Web.Endpoints.Users;

public sealed class RegisterUserValidator(IPasswordValidator passwordValidator) : Validator<RegisterUserRequest>
{
    public RegisterUserValidator() : this(null!)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Name)
            .NotEmpty();
        RuleFor(x => x.Surname)
            .NotEmpty();
        RuleFor(x => x.Password)
            .NotEmpty()
            .Must(ValidatePassword)
            .WithMessage(GetRequirements());
    }
    private bool ValidatePassword(string password)
    {
        return passwordValidator.ValidatePassword(password);
    }

    private string GetRequirements()
    {
        return passwordValidator.PasswordRequirements;
    }

}
