namespace DealMatcher.Backend.Core.Aggregates.Admin;

public sealed record ActivityDetail
{
    public string Name { get; set; }
    public string Value { get; set; }

    public ActivityDetail(string name, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Name = name.Trim();
        Value = value.Trim();
    }
}


