namespace DealMatcher.Backend.Core.Aggregates.Conversation;

public class Message : DealMatcherEntityBase,
    IAggregateRoot
{
    public int ConversationId { get; private set; }
    public int SenderId { get; private set; }
    public string Content { get; private set; }
    public MessageStatus Status { get; private set; }

    public Message(
        int conversationId,
        int senderId,
        string content
    )
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(senderId);
        ConversationId = conversationId;
        SenderId = senderId;
        Content = content;
        Status = MessageStatus.SENT;
    }

#pragma warning disable CS8618
    private Message()
    {
        /* EF */
    }
#pragma warning restore CS8618

    public void MarkAsDelivered()
    {
        if (Status == MessageStatus.SENT)
        {
            Status = MessageStatus.DELIVERED;
        }
    }
    public void MarkAsRead()
    {
        Status = MessageStatus.READ;
    }
}
