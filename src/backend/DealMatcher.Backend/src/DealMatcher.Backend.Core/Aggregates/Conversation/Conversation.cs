namespace DealMatcher.Backend.Core.Aggregates.Conversation;

public sealed class Conversation: DealMatcherEntityBase, IAggregateRoot
{
public int OfferId { get; private set; }
public int SellerId { get; private set; }
public int BuyerId { get; private set; }
public ConversationStatus Status { get; private set; }

}
