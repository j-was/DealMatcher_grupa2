using DealMatcher.Backend.Core.Aggregates.User;

namespace DealMatcher.Backend.Core.Events;

public sealed class UserCreatedEvent(int userId, string name, string surname) : DomainEventBase
{
    public int UserId { get; } = userId;
    public string Name { get; } = name;
    public string Surname { get; } = surname;
}

public sealed class UserUpdatedEvent(int userId, string name, string surname) : DomainEventBase
{
    public int UserId { get; } = userId;
    public string Name { get; } = name;
    public string Surname { get; } = surname;
}

public sealed class UserDeletedEvent(int userId) : DomainEventBase
{
    public int UserId { get; } = userId;
}

public sealed class UserLoggedInEvent(int userId) : DomainEventBase
{
    public int UserId { get; } = userId;
}

public sealed class UserStatusChangeEvent(int userId, UserStatus oldStatus, UserStatus newStatus) : DomainEventBase
{
    public int UserId { get; } = userId;
    public UserStatus OldStatus { get; } = oldStatus;
    public UserStatus NewStatus { get; } = newStatus;
}
