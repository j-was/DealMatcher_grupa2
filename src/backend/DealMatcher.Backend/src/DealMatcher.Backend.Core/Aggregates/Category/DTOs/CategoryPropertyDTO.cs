namespace DealMatcher.Backend.Core.Aggregates.Category.DTOs;

public sealed record CategoryPropertyDTO(
    int Id,
    string Name,
    CategoryPropertyType Type,
    List<string>? Options
    );
