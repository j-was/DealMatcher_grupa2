using DealMatcher.Backend.Core.Aggregates.Ban.DTOs;

namespace DealMatcher.Backend.UseCases.Features.Ban.CreateBan;

public sealed record CreateBanCommand(int UserId, CreateBanDTO Ban)
    : ICommand<Result<BanDTO>>;
