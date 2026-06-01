namespace DealMatcher.Backend.Core.Aggregates.Conversation;

public class Conversation : DealMatcherEntityBase,
    IAggregateRoot
{
    public int OfferId { get; private set; }
    public int BuyerId { get; }
    public int SellerId { get; }
    public Offer.Offer Offer { get; private set; } = null!;
    public User.User Buyer { get; private set; } = null!;
    public User.User Seller { get; private set; } = null!;

    public string LastMessage { get; private set; }
    public DateTime LastMessageAt { get; private set; }
    public int UnreadCount { get; private set; }
    public ConversationStatus Status { get; private set; }
    public List<Message> Messages { get; } = [];

    public Conversation(
        int offerId,
        int buyerId,
        int sellerId,
        string initialMessage
    )
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(offerId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(buyerId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sellerId);

        OfferId = offerId;
        BuyerId = buyerId;
        SellerId = sellerId;

        LastMessage = initialMessage;
        LastMessageAt = DateTime.UtcNow;

        Status = ConversationStatus.Active;

        Messages = [];
        AddMessage(buyerId, initialMessage);
    }

#pragma warning disable CS8618
    private Conversation()
    {
        /* EF */
    }
#pragma warning restore CS8618

    public Message AddMessage(int senderId, string content)
    {
        if (Status == ConversationStatus.Closed)
        {
            throw new InvalidOperationException("Cannot add new message to closed conversation");
        }

        if (senderId != BuyerId && senderId != SellerId)
            throw new ArgumentException(
                "Sender must belong to conversation.");

        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        var message = new Message(Id, senderId, content.Trim());

        Messages.Add(message);

        LastMessage = message.Content;
        LastMessageAt = message.CreatedAt;

        UnreadCount++;

        return message;
    }

    public void MarkAsRead()
    {
        UnreadCount = 0;
    }

    public void CloseConversation()
    {
        Status = ConversationStatus.Closed;
    }
}
