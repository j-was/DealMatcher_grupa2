namespace DealMatcher.Backend.Core.Aggregates.Conversation.Specifications;

public sealed class ConversationsByUserIdSpec : Specification<Conversation>
{
    public ConversationsByUserIdSpec(int userId)
    {
        Query.Where(c => c.SellerId == userId || c.BuyerId == userId).OrderByDescending(c => c.LastMessageAt);
    }
}
