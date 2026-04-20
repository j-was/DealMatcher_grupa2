namespace DealMatcher.Backend.Web.Endpoints.Users;

public sealed class MeUpdateValidator : Validator<MeUpdateRequest>
{
    public MeUpdateValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Surname).NotEmpty();
    }
}
