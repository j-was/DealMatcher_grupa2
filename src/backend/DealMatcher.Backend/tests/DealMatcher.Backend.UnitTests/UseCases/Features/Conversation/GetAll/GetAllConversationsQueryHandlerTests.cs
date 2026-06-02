namespace DealMatcher.Backend.UnitTests.UseCases.Features.Conversation.GetAll;

public class GetAllConversationsQueryHandlerTests
{
    private readonly IRepository<ConversationEntity> _conversationRepository;
    private readonly IMapper _mapper;
    private readonly GetAllConversationsQueryHandler _handler;

    public GetAllConversationsQueryHandlerTests()
    {
        _conversationRepository = Substitute.For<IRepository<ConversationEntity>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetAllConversationsQueryHandler(_conversationRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnConversations_ForGivenUserId()
    {
        var token = CancellationToken.None;
        var conversations = new List<ConversationEntity>
        {
            CreateConversationEntity(11, 2, 3, "Cześć"), CreateConversationEntity(12, 2, 4, "Drugie")
        };

        _conversationRepository
            .ListAsync(Arg.Any<ConversationsByUserIdSpec>(), token)
            .Returns(conversations);

        _mapper
            .Map<List<ConversationDTO>>(conversations)
            .Returns([]);

        var result = await _handler.Handle(new GetAllConversationsQuery(2), token);

        result.IsSuccess.ShouldBeTrue();
        result.Status.ShouldBe(ResultStatus.Ok);

        await _conversationRepository.Received(1)
            .ListAsync(Arg.Any<ConversationsByUserIdSpec>(), token);

        _mapper.Received(1)
            .Map<List<ConversationDTO>>(conversations);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        using var cts = new CancellationTokenSource();

        _conversationRepository
            .ListAsync(Arg.Any<ConversationsByUserIdSpec>(), cts.Token)
            .Returns([]);

        _mapper
            .Map<List<ConversationDTO>>(Arg.Any<List<ConversationEntity>>())
            .Returns([]);

        await _handler.Handle(new GetAllConversationsQuery(99), cts.Token);

        await _conversationRepository.Received(1)
            .ListAsync(Arg.Any<ConversationsByUserIdSpec>(), cts.Token);
    }

    private static ConversationEntity CreateConversationEntity(int offerId, int buyerId, int sellerId, string message)
    {
        return (ConversationEntity)Activator.CreateInstance(
            typeof(ConversationEntity),
            offerId,
            buyerId,
            sellerId,
            message)!;
    }
}
