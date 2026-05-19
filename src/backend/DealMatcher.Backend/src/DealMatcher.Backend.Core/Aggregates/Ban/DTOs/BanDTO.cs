namespace DealMatcher.Backend.Core.Aggregates.Ban.DTOs;

public sealed record BanDTO(int Id, int UserId,
    string Reason, int IssuedBy, DateTime IssuedAt, DateTime? ExpiresAt, bool IsActive);
