namespace DealMatcher.Backend.Core.Aggregates.Admin;

public sealed record ActivityDetail
{
    public string Name { get; }
    public string Value { get; }

    public ActivityDetail(string name, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Name = name.Trim();
        Value = value.Trim();
    }
}
