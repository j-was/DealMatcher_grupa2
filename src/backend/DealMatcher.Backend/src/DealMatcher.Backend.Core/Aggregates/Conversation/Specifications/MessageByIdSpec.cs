namespace DealMatcher.Backend.Core.Aggregates.Conversation.Specifications;

public sealed class MessageByIdSpec : SingleResultSpecification<Message>
{
    public MessageByIdSpec(int messageId)
    {
        Query.Where(c => c.Id == messageId);
    }
}
