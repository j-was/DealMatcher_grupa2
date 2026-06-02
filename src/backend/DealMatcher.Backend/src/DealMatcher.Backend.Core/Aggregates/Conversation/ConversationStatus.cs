namespace DealMatcher.Backend.Core.Aggregates.Conversation;

public abstract class ConversationStatus(
    string name,
    string value) :
    SmartEnum<ConversationStatus, string>(name, value)
{
    public static readonly ConversationStatus Closed = new ClosedConversationStatus();
    public static readonly ConversationStatus Active = new ActiveConversationStatus();

    private sealed class ClosedConversationStatus() :
        ConversationStatus(nameof(ClosedConversationStatus), nameof(Closed))
    {
    }

    private sealed class ActiveConversationStatus() :
        ConversationStatus(nameof(ActiveConversationStatus), nameof(Active))
    {
    }
}
