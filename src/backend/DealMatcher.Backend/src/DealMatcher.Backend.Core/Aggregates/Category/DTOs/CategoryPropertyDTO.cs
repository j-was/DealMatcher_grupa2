namespace DealMatcher.Backend.Core.Aggregates.Category.DTOs;

public sealed record CategoryPropertyDTO(
    string Name,
    string Type,
    List<string>? Options
    );
