namespace DealMatcher.Backend.Core.Aggregates.Conversation.Specifications;

public sealed class ConversationsByUserIdSpec : Specification<Conversation>
{
    public ConversationsByUserIdSpec(int userId)
    {
        Query
            .Where(c => c.SellerId == userId || c.BuyerId == userId)
            .Include(c => c.Offer)
            .Include(c => c.Buyer)
            .Include(c => c.Seller)
            .OrderByDescending(c => c.LastMessageAt);
    }
}
