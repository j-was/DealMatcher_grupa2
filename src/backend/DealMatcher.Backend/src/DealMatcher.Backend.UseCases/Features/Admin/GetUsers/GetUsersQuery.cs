using DealMatcher.Backend.UseCases.Features.Ban.DTOs;

namespace DealMatcher.Backend.UseCases.Features.Ban.GetUsers;

public sealed record GetUsersQuery(int Page, int Limit, string? Status, int UserId) : IQuery<Result<AdminUsersDTO>>;
