namespace DealMatcher.Backend.Web.Endpoints.Categories.Properties;

public class GetPropertiesValidator : Validator<GetPropertiesRequest>
{
    public GetPropertiesValidator()
    {
        RuleFor(x => x.CategoryName)
            .NotEmpty();
    }
}
