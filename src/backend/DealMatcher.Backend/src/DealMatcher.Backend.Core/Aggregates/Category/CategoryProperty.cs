namespace DealMatcher.Backend.Core.Aggregates.Category;

public sealed record CategoryProperty
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description = null;
}
