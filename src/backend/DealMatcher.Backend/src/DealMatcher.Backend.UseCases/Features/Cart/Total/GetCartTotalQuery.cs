using Ardalis.Result;
using DealMatcher.Backend.Core.Aggregates.Cart.DTOs;
using MediatR;

namespace DealMatcher.Backend.UseCases.Features.Cart.Total;

public sealed record GetCartTotalQuery(int UserId) : IRequest<Result<CartTotalDTO>>;
