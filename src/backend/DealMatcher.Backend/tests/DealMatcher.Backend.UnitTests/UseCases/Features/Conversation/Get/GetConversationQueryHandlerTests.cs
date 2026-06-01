namespace DealMatcher.Backend.UnitTests.UseCases.Features.Conversation.Get;

public class GetConversationQueryHandlerTests
{
    private readonly IRepository<ConversationEntity> _conversationRepository;
    private readonly IMapper _mapper;
    private readonly IRequestHandler<GetConversationQuery, Result<ConversationDetailsDTO>> _handler;

    public GetConversationQueryHandlerTests()
    {
        _conversationRepository = Substitute.For<IRepository<ConversationEntity>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetConversationQueryHandler(_conversationRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenConversationDoesNotExist()
    {
        _conversationRepository
            .FirstOrDefaultAsync(Arg.Any<ConversationDetailsByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns((ConversationEntity?)null);

        var result = await _handler.Handle(new GetConversationQuery(123), CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.Errors.ShouldContain("Conversation details not found");

        await _conversationRepository.Received(1)
            .FirstOrDefaultAsync(Arg.Any<ConversationDetailsByIdSpec>(), Arg.Any<CancellationToken>());

        _mapper.DidNotReceive()
            .Map<ConversationDetailsDTO>(Arg.Any<ConversationEntity>());
    }

    [Fact]
    public async Task Handle_ShouldReturnConversationDetails_WhenConversationExists()
    {
        var conversation = CreateConversationEntity(12, 2, 5, "Pierwsza wiadomość");

        _conversationRepository
            .FirstOrDefaultAsync(Arg.Any<ConversationDetailsByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(conversation);

        _mapper
            .Map<ConversationDetailsDTO>(conversation)
            .Returns(default(ConversationDetailsDTO));

        var result = await _handler.Handle(new GetConversationQuery(12), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Status.ShouldBe(ResultStatus.Ok);

        await _conversationRepository.Received(1)
            .FirstOrDefaultAsync(Arg.Any<ConversationDetailsByIdSpec>(), Arg.Any<CancellationToken>());

        _mapper.Received(1)
            .Map<ConversationDetailsDTO>(conversation);
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
