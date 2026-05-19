namespace DealMatcher.Backend.Core.Aggregates.User.DTOs;

public sealed record UserDetailsDTO(
    int Id,
    string Email,
    string Name,
    string Surname,
    string Status,
    DateTime CreatedAt,
    int TotalOffers,
    int TotalSales,
    int TotalPurchases,
    DateTime LastActive
    );
