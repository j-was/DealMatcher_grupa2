namespace DealMatcher.Backend.Core.Aggregates.Conversation;

public abstract class ConversationStatus(
    string name,
    string value) :
    SmartEnum<ConversationStatus, string>(name, value)
{
    public static readonly ConversationStatus Active = new ActiveConversationStatus();
    public static readonly ConversationStatus Closed = new ClosedMessageStatus();

    private sealed class ActiveConversationStatus() :
        ConversationStatus(nameof(ActiveConversationStatus), nameof(Active))
    {
    }

    private sealed class ClosedMessageStatus() :
        ConversationStatus(nameof(ClosedMessageStatus), nameof(Closed))
    {
    }
}
