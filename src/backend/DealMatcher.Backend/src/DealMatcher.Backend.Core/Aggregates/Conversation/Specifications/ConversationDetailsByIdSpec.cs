namespace DealMatcher.Backend.Core.Aggregates.Conversation.Specifications;

public sealed class ConversationDetailsByIdSpec : SingleResultSpecification<Conversation>
{
    public ConversationDetailsByIdSpec(int conversationId)
    {
        Query.Where(c => c.Id == conversationId).Include(c => c.Messages);
    }
}