namespace DealMatcher.Backend.Web.Endpoints.Offers;

public sealed class UpdateOfferValidator : Validator<UpdateOfferRequest>
{
    public UpdateOfferValidator()
    {
        When(x => x.Title != null, () =>
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title cannot be empty.")
                .MinimumLength(DataSchemaConstants.TitleMinLength)
                .WithMessage($"Title must be at least {DataSchemaConstants.TitleMinLength} characters.")
                .MaximumLength(DataSchemaConstants.TitleMaxLength)
                .WithMessage($"Title cannot exceed {DataSchemaConstants.TitleMaxLength} characters.");
        });

        When(x => x.Description != null, () =>
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description cannot be empty.")
                .MinimumLength(DataSchemaConstants.DescriptionMinLength)
                .WithMessage($"Description must be at least {DataSchemaConstants.DescriptionMinLength} characters.")
                .MaximumLength(DataSchemaConstants.DescriptionMaxLength)
                .WithMessage($"Description cannot exceed {DataSchemaConstants.DescriptionMaxLength} characters.");
        });

        When(x => x.Price.HasValue, () =>
        {
            RuleFor(x => x.Price!.Value)
                .GreaterThanOrEqualTo((double)DataSchemaConstants.PriceMinValue)
                .WithMessage($"Price must be at least {DataSchemaConstants.PriceMinValue:C}.")
                .LessThanOrEqualTo((double)DataSchemaConstants.PriceMaxValue)
                .WithMessage($"Price cannot exceed {DataSchemaConstants.PriceMaxValue:C}.");
        });

        When(x => x.Availability.HasValue, () =>
        {
            RuleFor(x => x.Availability!.Value)
                .InclusiveBetween(DataSchemaConstants.AvailabilityMinValue, DataSchemaConstants.AvailabilityMaxValue)
                .WithMessage($"Availability must be between {DataSchemaConstants.AvailabilityMinValue} and {DataSchemaConstants.AvailabilityMaxValue}.");
        });

        When(x => x.CategoryId.HasValue, () =>
        {
            RuleFor(x => x.CategoryId!.Value)
                .GreaterThan(0)
                .WithMessage("Category ID must be greater than 0.");
        });

        When(x => x.Tags != null, () =>
        {
            RuleFor(x => x.Tags)
                .Must(tags => tags!.Count <= DataSchemaConstants.MaxTagsCount)
                .WithMessage($"Cannot exceed {DataSchemaConstants.MaxTagsCount} tags.");

            RuleForEach(x => x.Tags)
                .NotEmpty()
                .WithMessage("Tag cannot be empty.")
                .MaximumLength(DataSchemaConstants.TagMaxLength)
                .WithMessage($"Tag cannot exceed {DataSchemaConstants.TagMaxLength} characters.");
        });

        When(x => x.Properties != null, () =>
        {
            RuleFor(x => x.Properties)
                .Must(properties => properties!.Count <= DataSchemaConstants.MaxCategoryProperties)
                .WithMessage($"Cannot exceed {DataSchemaConstants.MaxCategoryProperties} properties.");

            RuleForEach(x => x.Properties)
                .Must(kvp => !string.IsNullOrWhiteSpace(kvp.Key))
                .WithMessage("Property name cannot be empty.")
                .Must(kvp => kvp.Key.Length >= DataSchemaConstants.PropertyNameMinLength)
                .WithMessage($"Property name must be at least {DataSchemaConstants.PropertyNameMinLength} characters.")
                .Must(kvp => kvp.Key.Length <= DataSchemaConstants.PropertyNameMaxLength)
                .WithMessage($"Property name cannot exceed {DataSchemaConstants.PropertyNameMaxLength} characters.")
                .Must(kvp => !string.IsNullOrWhiteSpace(kvp.Value))
                .WithMessage("Property value cannot be empty.")
                .Must(kvp => kvp.Value.Length >= DataSchemaConstants.PropertyValueMinLength)
                .WithMessage($"Property value must be at least {DataSchemaConstants.PropertyValueMinLength} characters.")
                .Must(kvp => kvp.Value.Length <= DataSchemaConstants.PropertyValueMaxLength)
                .WithMessage($"Property value cannot exceed {DataSchemaConstants.PropertyValueMaxLength} characters.");
        });
    }
}
