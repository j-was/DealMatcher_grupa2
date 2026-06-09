namespace DealMatcher.Backend.Core.Aggregates.Offer;

public sealed record OfferProperty
{
    public string PropertyId { get; }
    public string Value { get; }

    public OfferProperty(string propertyId, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyId);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        if (propertyId.Length < DataSchemaConstants.PropertyNameMinLength)
            throw new ArgumentException(
                $"Property name must be at least {DataSchemaConstants.PropertyNameMinLength} characters.");

        if (propertyId.Length > DataSchemaConstants.PropertyNameMaxLength)
            throw new ArgumentException(
                $"Property name cannot exceed {DataSchemaConstants.PropertyNameMaxLength} characters.");

        if (value.Length < DataSchemaConstants.PropertyValueMinLength)
            throw new ArgumentException(
                $"Property value must be at least {DataSchemaConstants.PropertyValueMinLength} characters.");

        if (value.Length > DataSchemaConstants.PropertyValueMaxLength)
            throw new ArgumentException(
                $"Property value cannot exceed {DataSchemaConstants.PropertyValueMaxLength} characters.");

        PropertyId = propertyId.Trim();
        Value = value.Trim();
    }

#pragma warning disable CS8618
    private OfferProperty()
    {
        /* EF */
    }
#pragma warning restore CS8618
}
