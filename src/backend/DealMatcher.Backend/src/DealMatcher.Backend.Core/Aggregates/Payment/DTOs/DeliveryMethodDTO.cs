namespace DealMatcher.Backend.Core.Aggregates.Payment.DTOs;

public sealed record DeliveryMethodDTO(
    string Id,
    string Name,
    string Description,
    double Price,
    int EstimatedDays
);
