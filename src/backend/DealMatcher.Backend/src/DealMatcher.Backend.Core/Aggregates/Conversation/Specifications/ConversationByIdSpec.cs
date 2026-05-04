namespace DealMatcher.Backend.Core.Aggregates.Conversation.Specifications;

public sealed class ConversationByIdSpec : SingleResultSpecification<Conversation>
{
    public ConversationByIdSpec(int conversationId)
    {
        Query.Where(c => c.Id == conversationId);
    }
}