using DealMatcher.Backend.UseCases.Features.Cart.DTOs;
namespace DealMatcher.Backend.UseCases.Features.Cart.Total;

public sealed record GetCartTotalQuery(int UserId) : IRequest<Result<CartTotalDTO>>;
