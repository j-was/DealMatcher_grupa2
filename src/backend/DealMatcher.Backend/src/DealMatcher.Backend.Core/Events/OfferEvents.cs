using DealMatcher.Backend.Core.Aggregates.Offer;

namespace DealMatcher.Backend.Core.Events;

public sealed class OfferCreatedEvent(int userId, int offerId, string title, string description, double price, int availability) : DomainEventBase
{
    public int UserId { get; init; } = userId;
    public int OfferId { get; init; } = offerId;
    public string Title { get; init; } = title;
    public string Description { get; init; } = description;
    public double Price { get; init; } = price;
    public int Availability { get; init; } = availability;
}

public sealed class OfferUpdatedEvent(int userId, int offerId, string title, string description, double price, int availability) : DomainEventBase
{
    public int UserId { get; init; } = userId;
    public int OfferId { get; init; } = offerId;
    public string Title { get; init; } = title;
    public string Description { get; init; } = description;
    public double Price { get; init; } = price;
    public int Availability { get; init; } = availability;
}

public sealed class OfferDeletedEvent(int userId, int offerId) : DomainEventBase
{
    public int UserId { get; init; } = userId;
    public int OfferId { get; init; } = offerId;
}

public sealed class OfferViewedEvent(int userId, int offerId) : DomainEventBase
{
    public int UserId { get; init; } = userId;
    public int OfferId { get; init; } = offerId;
}

public sealed class OfferPurchasedEvent(int userId, int offerId, int quantity, int remaining, int price) : DomainEventBase
{
    public int UserId { get; init; } = userId;
    public int OfferId { get; init; } = offerId;
    public int Quantity { get; init; } = quantity;
    public int Remaining { get; init; } = remaining;
    public int Price { get; init; } = price;
}

public sealed class OfferStatusChangedEvent(int userId, int offerId, OfferStatus oldStatus, OfferStatus newStatus) : DomainEventBase
{
    public int UserId { get; init; } = userId;
    public int OfferId { get; init; } = offerId;
    public OfferStatus OldStatus { get; init; } = oldStatus;
    public OfferStatus NewStatus { get; init; } = newStatus;
}
