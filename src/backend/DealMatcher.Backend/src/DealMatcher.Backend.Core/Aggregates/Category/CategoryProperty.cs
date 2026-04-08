namespace DealMatcher.Backend.Core.Aggregates.Category;


public enum CategoryPropertyType
{
    TEXT,
    NUMBER,
    BOOLEAN,
    SELECT
}
public sealed record CategoryProperty
{
    public string Name { get; set; }
    public CategoryPropertyType Type { get; set; }
    public List<string>? Options { get; set; }
    public CategoryProperty(string Name, CategoryPropertyType Type, List<string>? Options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Name);
        if (Type == CategoryPropertyType.SELECT && (Options == null || Options.Count == 0))
            throw new ArgumentException("Options must be provided for SELECT type properties.");
        this.Name = Name.Trim();
        this.Type = Type;
        this.Options = Options;
    }
}
