namespace DealMatcher.Backend.UseCases.Features.Ban.DeleteBan;

public sealed record DeleteBanCommand(int AdminId, int BanId) : ICommand<Result>;
