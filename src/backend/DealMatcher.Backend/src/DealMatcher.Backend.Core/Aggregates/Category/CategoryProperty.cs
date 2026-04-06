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
    public int Id { get; set; }
    public required string Name { get; set; }
    public CategoryPropertyType Type { get; set; }
    public List<string>? Options { get; set; }
}
