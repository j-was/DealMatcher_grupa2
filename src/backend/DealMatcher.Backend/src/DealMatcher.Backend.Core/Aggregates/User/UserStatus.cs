namespace DealMatcher.Backend.Core.Aggregates.User;

public abstract class UserStatus(
    string name,
    string value) : SmartEnum<UserStatus, string>(name, value)
{
    public static readonly UserStatus Active = new ActiveUserStatus();
    public static readonly UserStatus Inactive = new InactiveUserStatus();
    public static readonly UserStatus BannedUserStatus = new PromotedOfferStatus();

    private sealed class ActiveUserStatus() :
        UserStatus(nameof(ActiveUserStatus), nameof(Active))
    {
    }

    private sealed class InactiveUserStatus() :
        UserStatus(nameof(InactiveUserStatus), nameof(Inactive))
    {
    }

    private sealed class PromotedOfferStatus() :
        UserStatus(nameof(PromotedOfferStatus), nameof(BannedUserStatus))
    {
    }
}
