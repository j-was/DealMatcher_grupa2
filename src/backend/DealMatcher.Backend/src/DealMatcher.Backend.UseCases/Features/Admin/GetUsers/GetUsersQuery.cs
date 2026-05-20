using DealMatcher.Backend.UseCases.Features.Admin.DTOs;

namespace DealMatcher.Backend.UseCases.Features.Admin.GetUsers;

public sealed record GetUsersQuery(int Page, int Limit, string? Status, int UserId) : IQuery<Result<AdminUsersDTO>>;
