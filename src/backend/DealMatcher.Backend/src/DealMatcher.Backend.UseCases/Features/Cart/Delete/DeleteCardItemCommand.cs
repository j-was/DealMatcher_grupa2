namespace DealMatcher.Backend.UseCases.Features.Cart.Delete;

public sealed record DeleteCartItemCommand(int UserId, int CartItemId) : IRequest<Result>;
