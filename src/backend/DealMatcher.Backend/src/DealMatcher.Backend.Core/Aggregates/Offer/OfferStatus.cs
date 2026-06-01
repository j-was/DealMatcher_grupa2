namespace DealMatcher.Backend.Core.Aggregates.Offer;

public abstract class OfferStatus(
    string name,
    string value) :
    SmartEnum<OfferStatus, string>(name, value)
{
    public static readonly OfferStatus Draft = new DraftOfferStatus();
    public static readonly OfferStatus Active = new ActiveOfferStatus();
    public static readonly OfferStatus Promoted = new PromotedOfferStatus();
    public static readonly OfferStatus Sold = new SoldOfferStatus();
    public static readonly OfferStatus Deleted = new DeletedOfferStatus();

    private sealed class DeletedOfferStatus() :
        OfferStatus(nameof(DeletedOfferStatus), nameof(Deleted))
    {
    }

    private sealed class SoldOfferStatus() :
        OfferStatus(nameof(SoldOfferStatus), nameof(Sold))
    {
    }

    private sealed class DraftOfferStatus() :
        OfferStatus(nameof(DraftOfferStatus), nameof(Draft))
    {
    }

    private sealed class ActiveOfferStatus() :
        OfferStatus(nameof(ActiveOfferStatus), nameof(Active))
    {
    }

    private sealed class PromotedOfferStatus() :
        OfferStatus(nameof(PromotedOfferStatus), nameof(Promoted))
    {
    }
}
