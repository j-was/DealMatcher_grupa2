namespace DealMatcher.Backend.Core.Aggregates.Offer;

public sealed record OfferProperty
{
    public string Name { get; set; }
    public string Value { get; set; }

    public OfferProperty(
        string name,
        string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Name = name.Trim();
        Value = value.Trim();
    }
}
