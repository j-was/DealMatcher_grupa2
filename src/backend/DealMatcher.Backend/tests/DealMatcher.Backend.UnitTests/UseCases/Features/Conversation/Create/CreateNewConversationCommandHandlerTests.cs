namespace DealMatcher.Backend.UnitTests.UseCases.Features.Conversation.Create;

public class CreateNewConversationCommandHandlerTests
{
    private readonly IRepository<ConversationEntity> _conversationRepository;
    private readonly IRepository<OfferEntity> _offerRepository;
    private readonly IMapper _mapper;
    private readonly IRequestHandler<CreateNewConversationCommand, Result<ConversationDTO>> _handler;

    public CreateNewConversationCommandHandlerTests()
    {
        _conversationRepository = Substitute.For<IRepository<ConversationEntity>>();
        _offerRepository = Substitute.For<IRepository<OfferEntity>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateNewConversationCommandHandler(_conversationRepository, _offerRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenOfferDoesNotExist()
    {
        _offerRepository
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns((OfferEntity?)null);

        var result = await _handler.Handle(new CreateNewConversationCommand(10, 2, "Hej"), CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.Errors.ShouldContain("Offer not found");

        await _offerRepository.Received(1)
            .GetByIdAsync(10, Arg.Any<CancellationToken>());

        await _conversationRepository.DidNotReceiveWithAnyArgs()
            .AddAsync(default!, default);
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenBuyerIsSeller()
    {
        var offer = CreateOfferEntity(sellerId: 2);

        _offerRepository
            .GetByIdAsync(10, Arg.Any<CancellationToken>())
            .Returns(offer);

        var result = await _handler.Handle(new CreateNewConversationCommand(10, 2, "Hej"), CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);
        result.Errors.ShouldContain("Cannot create conversation with yourself");

        await _conversationRepository.DidNotReceiveWithAnyArgs()
            .FirstOrDefaultAsync(default!, default);
        await _conversationRepository.DidNotReceiveWithAnyArgs()
            .AddAsync(default!, default);
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenConversationAlreadyExists()
    {
        var offer = CreateOfferEntity(sellerId: 7);
        var existingConversation = CreateConversationEntity(10, 2, 7, "już istnieje");

        _offerRepository
            .GetByIdAsync(10, Arg.Any<CancellationToken>())
            .Returns(offer);

        _conversationRepository
            .FirstOrDefaultAsync(Arg.Any<ExistingConversationSpec>(), Arg.Any<CancellationToken>())
            .Returns(existingConversation);

        var result = await _handler.Handle(new CreateNewConversationCommand(10, 2, "Hej"), CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Conflict);
        result.Errors.ShouldContain("Conversation for this offer already exists");

        await _conversationRepository.DidNotReceive()
            .AddAsync(Arg.Any<ConversationEntity>(), Arg.Any<CancellationToken>());
        await _conversationRepository.DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldCreateConversation_WhenRequestIsValid()
    {
        var offer = CreateOfferEntity(sellerId: 7);
        var createdConversation = CreateConversationEntity(10, 2, 7, "Hej");

        _offerRepository
            .GetByIdAsync(10, Arg.Any<CancellationToken>())
            .Returns(offer);

        _conversationRepository
            .FirstOrDefaultAsync(Arg.Any<ExistingConversationSpec>(), Arg.Any<CancellationToken>())
            .Returns((ConversationEntity?)null);

        _conversationRepository
            .FirstOrDefaultAsync(Arg.Any<ConversationByIdWithUsersSpec>(), Arg.Any<CancellationToken>())
            .Returns(createdConversation);

        _mapper
            .Map<ConversationDTO>(Arg.Any<ConversationEntity>())
            .Returns(default(ConversationDTO));

        var result = await _handler.Handle(new CreateNewConversationCommand(10, 2, "Hej"), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Status.ShouldBe(ResultStatus.Created);

        await _conversationRepository.Received(1)
            .AddAsync(Arg.Any<ConversationEntity>(), Arg.Any<CancellationToken>());
        await _conversationRepository.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());

        _mapper.Received(1)
            .Map<ConversationDTO>(Arg.Any<ConversationEntity>());
    }

    private static OfferEntity CreateOfferEntity(int sellerId)
    {
        var offer = Activator.CreateInstance(typeof(OfferEntity), nonPublic: true) as OfferEntity
                    ?? throw new InvalidOperationException("Nie udało się utworzyć OfferEntity.");

        var type = typeof(OfferEntity);
        var property = type.GetProperty(nameof(OfferEntity.SellerId),
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (property?.SetMethod is not null)
        {
            property.SetValue(offer, sellerId);
            return offer;
        }

        var field = type.GetField($"<{nameof(OfferEntity.SellerId)}>k__BackingField",
                        BindingFlags.Instance | BindingFlags.NonPublic) ??
                    throw new InvalidOperationException("Nie udało się ustawić SellerId.");
        field.SetValue(offer, sellerId);
        return offer;
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

    private static void SetMemberValue(object target, string memberName, object value)
    {
        var type = target.GetType();

        var property =
            type.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (property is not null)
        {
            var setter = property.GetSetMethod(nonPublic: true);
            if (setter is not null)
            {
                setter.Invoke(target, [value]);
                return;
            }
        }

        var field = type.GetField($"<{memberName}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)
                    ?? type.GetField(memberName, BindingFlags.Instance | BindingFlags.NonPublic);

        field.ShouldNotBeNull($"Nie udało się ustawić pola ani właściwości '{memberName}' na typie {type.Name}.");
        field!.SetValue(target, value);
    }
}
