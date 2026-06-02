namespace DealMatcher.Backend.Core.Aggregates.Conversation.Specifications;

public sealed class ConversationByIdWithUsersSpec : SingleResultSpecification<Conversation>
{
    public ConversationByIdWithUsersSpec(int conversationId)
    {
        Query
            .Where(c => c.Id == conversationId)
            .Include(c => c.Offer)
            .Include(c => c.Buyer)
            .Include(c => c.Seller);
    }
}
