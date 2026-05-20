using MediatR;

namespace DealMatcher.Backend.Infrastructure.ActivityLogging;

public sealed class ActivityEventsHandler(IActivityLogger activityLogger) :
    INotificationHandler<OfferCreatedEvent>,
    INotificationHandler<OfferUpdatedEvent>,
    INotificationHandler<OfferDeletedEvent>,
    INotificationHandler<OfferViewedEvent>,
    INotificationHandler<OfferPurchasedEvent>,
    INotificationHandler<OfferStatusChangedEvent>,
    INotificationHandler<UserCreatedEvent>,
    INotificationHandler<UserUpdatedEvent>,
    INotificationHandler<UserDeletedEvent>,
    INotificationHandler<UserLoggedInEvent>,
    INotificationHandler<UserStatusChangeEvent>
{
    private readonly IActivityLogger _activityLogger = activityLogger;

    public Task Handle(OfferCreatedEvent notification, CancellationToken cancellationToken)
    {
        return _activityLogger.LogOfferActivityAsync(
            notification.UserId,
            ActionType.Create,
            notification.OfferId,
            [
                new("Title", notification.Title),
                new("Description", notification.Description),
                new("Price", notification.Price.ToString()),
                new("Availability", notification.Availability.ToString())
            ],
            cancellationToken: cancellationToken);
    }

    public Task Handle(OfferUpdatedEvent notification, CancellationToken cancellationToken)
    {
        return _activityLogger.LogOfferActivityAsync(
            notification.UserId,
            ActionType.Update,
            notification.OfferId,
            [
                new("Title", notification.Title),
                new("Description", notification.Description),
                new("Price", notification.Price.ToString()),
                new("Availability", notification.Availability.ToString())
            ],
            cancellationToken: cancellationToken);
    }

    public Task Handle(OfferDeletedEvent notification, CancellationToken cancellationToken)
    {
        return _activityLogger.LogOfferActivityAsync(
            notification.UserId,
            ActionType.Delete,
            notification.OfferId,
            [],
            cancellationToken: cancellationToken);
    }

    public Task Handle(OfferViewedEvent notification, CancellationToken cancellationToken)
    {
        return _activityLogger.LogOfferActivityAsync(
            notification.UserId,
            ActionType.View,
            notification.OfferId,
            [],
            cancellationToken: cancellationToken);
    }

    public Task Handle(OfferPurchasedEvent notification, CancellationToken cancellationToken)
    {
        return _activityLogger.LogOfferActivityAsync(
            notification.UserId,
            ActionType.Purchase,
            notification.OfferId,
            [
                new("Quantity", notification.Quantity.ToString()),
                new("Remaining", notification.Remaining.ToString()),
                new("Price", notification.Price.ToString())
            ],
            cancellationToken: cancellationToken);
    }

    public Task Handle(OfferStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        return _activityLogger.LogOfferActivityAsync(
            notification.UserId,
            ActionType.Status_Change,
            notification.OfferId,
            [
                new("OldStatus", notification.OldStatus.ToString()),
                new("NewStatus", notification.NewStatus.ToString())
            ],
            cancellationToken: cancellationToken);
    }

    public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        return _activityLogger.LogUserActivityAsync(
            notification.UserId,
            ActionType.Create,
            [
                new("Name", notification.Name),
                new("Surname", notification.Surname)
            ],
            cancellationToken: cancellationToken);
    }

    public Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
    {
        return _activityLogger.LogUserActivityAsync(
            notification.UserId,
            ActionType.Update,
            [
                new("Name", notification.Name),
                new("Surname", notification.Surname)
            ],
            cancellationToken: cancellationToken);
    }

    public Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
    {
        return _activityLogger.LogUserActivityAsync(
            notification.UserId,
            ActionType.Delete,
            [],
            cancellationToken: cancellationToken);
    }

    public Task Handle(UserLoggedInEvent notification, CancellationToken cancellationToken)
    {
        return _activityLogger.LogUserActivityAsync(
            notification.UserId,
            ActionType.Login,
            [],
            cancellationToken: cancellationToken);
    }

    public Task Handle(UserStatusChangeEvent notification, CancellationToken cancellationToken)
    {
        return _activityLogger.LogUserActivityAsync(
            notification.UserId,
            ActionType.Status_Change,
            [
                new("OldStatus", notification.OldStatus.ToString()),
                new("NewStatus", notification.NewStatus.ToString())
            ],
            cancellationToken: cancellationToken);
    }
}
