namespace DealMatcher.Backend.Core.Aggregates.Payment.DTOs;

public sealed record PaymentMethodDTO(
    string Id,
    string Name,
    string Provider,
    string Icon
);
