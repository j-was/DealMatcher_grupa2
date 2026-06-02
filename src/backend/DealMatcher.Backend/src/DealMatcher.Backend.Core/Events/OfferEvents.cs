using DealMatcher.Backend.Core.Aggregates.Offer;

namespace DealMatcher.Backend.Core.Events;

public sealed class OfferCreatedEvent(
    int userId,
    int offerId,
    string title,
    string description,
    decimal price,
    int availability) : DomainEventBase
{
    public int UserId { get; } = userId;
    public int OfferId { get; } = offerId;
    public string Title { get; } = title;
    public string Description { get; } = description;
    public decimal Price { get; } = price;
    public int Availability { get; } = availability;
}

public sealed class OfferUpdatedEvent(
    int userId,
    int offerId,
    string title,
    string description,
    decimal price,
    int availability) : DomainEventBase
{
    public int UserId { get; } = userId;
    public int OfferId { get; } = offerId;
    public string Title { get; } = title;
    public string Description { get; } = description;
    public decimal Price { get; } = price;
    public int Availability { get; } = availability;
}

public sealed class OfferDeletedEvent(int userId, int offerId) : DomainEventBase
{
    public int UserId { get; } = userId;
    public int OfferId { get; } = offerId;
}

public sealed class OfferViewedEvent(int userId, int offerId) : DomainEventBase
{
    public int UserId { get; } = userId;
    public int OfferId { get; } = offerId;
}

public sealed class OfferPurchasedEvent(int userId, int offerId, int quantity, int remaining, decimal price)
    : DomainEventBase
{
    public int UserId { get; } = userId;
    public int OfferId { get; } = offerId;
    public int Quantity { get; } = quantity;
    public int Remaining { get; } = remaining;
    public decimal Price { get; } = price;
}

public sealed class OfferStatusChangedEvent(int userId, int offerId, OfferStatus oldStatus, OfferStatus newStatus)
    : DomainEventBase
{
    public int UserId { get; } = userId;
    public int OfferId { get; } = offerId;
    public OfferStatus OldStatus { get; } = oldStatus;
    public OfferStatus NewStatus { get; } = newStatus;
}
