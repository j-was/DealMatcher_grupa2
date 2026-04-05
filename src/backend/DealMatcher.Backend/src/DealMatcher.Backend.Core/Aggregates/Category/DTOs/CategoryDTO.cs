namespace DealMatcher.Backend.Core.Aggregates.Category.DTOs;

public sealed record CategoryDTO(
    int Id,
    string Name,
    string Description,
    List<CategoryPropertyDTO> Properties
    );

