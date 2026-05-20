namespace DealMatcher.Backend.Core.Aggregates.Admin.DTOs;

public sealed record ActivityRecordDTO(
    int Id,
    int UserId,
    int? OfferId,
    string Action,
    Dictionary<string, string> Details,
    string IpAddress,
    DateTime CreatedAt);
