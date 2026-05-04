namespace DealMatcher.Backend.Core.Aggregates.Conversation.Specifications;

public sealed class ExistingConversationSpec : SingleResultSpecification<Conversation>
{
    public ExistingConversationSpec(int offerId, int sellerId, int buyerId)
    {
        Query.Where(c => (c.OfferId == offerId) && c.SellerId == sellerId && c.BuyerId == buyerId);
    }
}