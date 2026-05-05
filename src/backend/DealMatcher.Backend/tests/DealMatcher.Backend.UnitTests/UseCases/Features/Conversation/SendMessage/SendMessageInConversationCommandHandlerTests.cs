using DealMatcher.Backend.UseCases.Features.Conversation.SendMessage;


namespace DealMatcher.Backend.UnitTests.UseCases.Features.Conversation.SendMessage;

public class SendMessageInConversationCommandHandlerTests
{
    private readonly IRepository<ConversationEntity> _conversationRepository;
    private readonly IMapper _mapper;
    private readonly SendMessageInConversationCommandHandler _handler;

    public SendMessageInConversationCommandHandlerTests()
    {
        _conversationRepository = Substitute.For<IRepository<ConversationEntity>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new SendMessageInConversationCommandHandler(_conversationRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenConversationDoesNotExist()
    {
        _conversationRepository
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns((ConversationEntity?)null);

        var result = await _handler.Handle(new SendMessageInConversationCommand(44, 2, "Hej"), CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.Errors.ShouldContain("Conversation not found");

        await _conversationRepository.Received(1)
            .GetByIdAsync(44, Arg.Any<CancellationToken>());

        await _conversationRepository.DidNotReceiveWithAnyArgs()
            .UpdateAsync(default!, default);
        await _conversationRepository.DidNotReceiveWithAnyArgs()
            .SaveChangesAsync(default);
    }

    [Fact]
    public async Task Handle_ShouldCreateMessage_WhenConversationExists()
    {
        var conversation = CreateConversationEntity(44, 2, 7, "start");

        _conversationRepository
            .GetByIdAsync(44, Arg.Any<CancellationToken>())
            .Returns(conversation);

        _mapper
            .Map<MessageDTO>(Arg.Any<Message>())
            .Returns(default(MessageDTO));

        var result = await _handler.Handle(new SendMessageInConversationCommand(44, 2, "Nowa wiadomość"), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Status.ShouldBe(ResultStatus.Created);

        await _conversationRepository.Received(1)
            .UpdateAsync(conversation, Arg.Any<CancellationToken>());
        await _conversationRepository.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());

        _mapper.Received(1)
            .Map<MessageDTO>(Arg.Any<Message>());
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
