namespace DealMatcher.Backend.Core.Aggregates.Ban.DTOs;

public sealed class CreateBanDTO
{
    public int UserId {get; init;}
    public string Reason { get; init; } = string.Empty;
    public DateTime? ExpiresAt { get; init; }
}