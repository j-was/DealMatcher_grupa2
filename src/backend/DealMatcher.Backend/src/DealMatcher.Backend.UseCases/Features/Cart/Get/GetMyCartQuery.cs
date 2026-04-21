namespace DealMatcher.Backend.UseCases.Features.Cart.Get;

public sealed record GetMyCartQuery(int UserId) : IRequest<Result<List<CartItemDTO>>>;
