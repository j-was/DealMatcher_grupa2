using DealMatcher.Backend.UseCases.Features.Ban.DTOs;

namespace DealMatcher.Backend.UseCases.Features.Ban.GetOffers;

public sealed record GetOffersQuery(int Page, int Limit, string? Status, int UserId) : IQuery<Result<AdminOffersDTO>>;
