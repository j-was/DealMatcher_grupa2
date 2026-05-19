namespace DealMatcher.Backend.UseCases.Features.Admin.DTOs;

public sealed record AdminOffersDTO
(
    IReadOnlyCollection<OfferDTO> Items,
    int Total,
    int Page,
    int Pages
);
