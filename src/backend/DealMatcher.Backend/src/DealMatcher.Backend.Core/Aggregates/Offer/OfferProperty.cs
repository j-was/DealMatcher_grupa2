namespace DealMatcher.Backend.Core.Aggregates.Offer;

public sealed record OfferProperty
{
    public int CategoryPropertyId { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }

    public OfferProperty(
        int categoryPropertyId,
        string name,
        string value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(categoryPropertyId);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        CategoryPropertyId = categoryPropertyId;
        Name = name.Trim();
        Value = value.Trim();
    }
}
