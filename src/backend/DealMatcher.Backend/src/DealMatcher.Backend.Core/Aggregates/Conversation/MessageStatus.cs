namespace DealMatcher.Backend.Core.Aggregates.Conversation;

public abstract class MessageStatus(
    string name,
    string value) :
    SmartEnum<MessageStatus, string>(name, value)
{
    public static readonly MessageStatus Sent = new SentMessageStatus();
    public static readonly MessageStatus Delivered = new DeliveredMessageStatus();
    public static readonly MessageStatus Read = new ReadMessageStatus();

    private sealed class SentMessageStatus() :
        MessageStatus(nameof(SentMessageStatus), nameof(Sent))
    {
    }

    private sealed class DeliveredMessageStatus() :
        MessageStatus(nameof(DeliveredMessageStatus), nameof(Delivered))
    {
    }

    private sealed class ReadMessageStatus() :
        MessageStatus(nameof(ReadMessageStatus), nameof(Read))
    {
    }
}
