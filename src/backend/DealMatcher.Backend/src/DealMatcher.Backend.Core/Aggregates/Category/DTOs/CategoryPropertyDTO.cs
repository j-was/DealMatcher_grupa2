namespace DealMatcher.Backend.Core.Aggregates.Category.DTOs;

public sealed record CategoryPropertyDTO(
    int Id,
    string Name,
    string Type,
    List<string>? Options
    );
