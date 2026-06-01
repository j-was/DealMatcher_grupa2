namespace DealMatcher.Backend.Core.Aggregates.Offer;

public sealed record OfferProperty
{
    public string Name { get; }
    public string Value { get; }

    public OfferProperty(string name, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        if (name.Length < DataSchemaConstants.PropertyNameMinLength)
            throw new ArgumentException(
                $"Property name must be at least {DataSchemaConstants.PropertyNameMinLength} characters.");

        if (name.Length > DataSchemaConstants.PropertyNameMaxLength)
            throw new ArgumentException(
                $"Property name cannot exceed {DataSchemaConstants.PropertyNameMaxLength} characters.");

        if (value.Length < DataSchemaConstants.PropertyValueMinLength)
            throw new ArgumentException(
                $"Property value must be at least {DataSchemaConstants.PropertyValueMinLength} characters.");

        if (value.Length > DataSchemaConstants.PropertyValueMaxLength)
            throw new ArgumentException(
                $"Property value cannot exceed {DataSchemaConstants.PropertyValueMaxLength} characters.");

        Name = name.Trim();
        Value = value.Trim();
    }
}
