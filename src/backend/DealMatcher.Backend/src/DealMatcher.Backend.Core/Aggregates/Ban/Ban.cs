namespace DealMatcher.Backend.Core.Aggregates.Ban;

public sealed class Ban : DealMatcherEntityBase, IAggregateRoot
{
    public int UserId { get; private set; }
    public string Reason { get; private set; }
    public int IssuedBy { get; private set; }
    public DateTime IssuedAt { get; private set; }
    public DateTime? ExpiresAt { get; private set; }
    public bool IsActive { get; private set; }

    public Ban(int userId, string reason, int issuedBy, DateTime? expiresAt)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(issuedBy);
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);

        if (expiresAt.HasValue && expiresAt.Value <= DateTime.UtcNow)
        {
            throw new ArgumentException("Ban expiration date must be in the future.");
        }

        UserId = userId;
        Reason = reason.Trim();
        IssuedBy = issuedBy;
        IssuedAt = DateTime.UtcNow;
        ExpiresAt = expiresAt;
        IsActive = true;
    }

#pragma warning disable CS8618
    private Ban()
    {
        /* EF */
    }
#pragma warning restore CS8618
}
