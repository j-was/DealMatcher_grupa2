using DealMatcher.Backend.UseCases.Features.Admin.DTOs;

namespace DealMatcher.Backend.UseCases.Features.Admin.GetOffers;

public sealed record GetOffersQuery(int Page, int Limit, string? Status, int UserId) : IQuery<Result<AdminOffersDTO>>;
