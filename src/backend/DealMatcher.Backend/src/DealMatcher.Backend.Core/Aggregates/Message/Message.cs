namespace DealMatcher.Backend.Core.Aggregates.Message;

public sealed class Message: DealMatcherEntityBase, IAggregateRoot
{
    public int SenderId { get; private set; }
    public string Content {get; private set;}
    public MessageStatus Status {get; private set;}

#pragma warning disable CS8618
    private Message() { /* EF */ }
#pragma warning restore CS8618

    public Message(int senderId, string content)
    {
        SenderId = senderId;
        Content = content;
        Status = MessageStatus.Sent;
    }

    public bool SetDelivered()
    {
        Status = MessageStatus.Delivered;
        return true;
    }

    public bool SetRead()
    {
        Status = MessageStatus.Read;
        return true;
    }
}
