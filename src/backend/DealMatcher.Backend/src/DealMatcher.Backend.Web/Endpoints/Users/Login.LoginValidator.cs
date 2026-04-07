namespace DealMatcher.Backend.Web.Endpoints.Users;

public sealed class LoginValidator:Validator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
