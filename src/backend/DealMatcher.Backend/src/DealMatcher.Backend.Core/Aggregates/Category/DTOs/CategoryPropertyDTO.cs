namespace DealMatcher.Backend.Core.Aggregates.Category.DTOs;

public sealed record CategoryPropertyDTO(
    string Name,
    CategoryPropertyType Type,
    List<string>? Options
    );
