namespace DealMatcher.Backend.Core.Aggregates.User.DTOs;

public sealed record UserDTO(
    int Id,
    string Email,
    string Name,
    string Surname,
    string Status,
    DateTime CreatedAt
);
