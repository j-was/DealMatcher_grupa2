namespace DealMatcher.Backend.UnitTests.UseCases.Mapping;

public class ConversationProfileTests
{
    private readonly IMapper _mapper;

    public ConversationProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ConversationProfile>();
        }, new SerilogLoggerFactory());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Configuration_IsValid()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ConversationProfile>();
        }, new SerilogLoggerFactory());

        configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_MessageEntityToMessageDTO_MapsAllPropertiesCorrectly()
    {
        var message = new MessageEntity(1, 2, "Hello!");
        typeof(MessageEntity).GetProperty("Id")!.SetValue(message, 10);

        var dto = _mapper.Map<MessageDTO>(message);

        dto.Id.ShouldBe(10);
        dto.SenderId.ShouldBe(2);
        dto.Content.ShouldBe("Hello!");
        dto.Status.ShouldBe("SENT");
    }

    [Fact]
    public void Map_ConversationEntityToConversationDTO_MapsBasicPropertiesCorrectly()
    {
        var conversation = CreateConversationEntity(1, 2, 3, "Initial message");
        typeof(ConversationEntity).GetProperty("Id")!.SetValue(conversation, 100);

        var dto = _mapper.Map<ConversationDTO>(conversation);

        dto.Id.ShouldBe(100);
        dto.OfferId.ShouldBe(1);
        dto.BuyerId.ShouldBe(2);
        dto.SellerId.ShouldBe(3);
        dto.LastMessage.ShouldBe("Initial message");
        dto.UnreadCount.ShouldBe(1);
    }

    [Fact]
    public void Map_ConversationEntityToConversationDTO_MapsStatusToUpperCaseString()
    {
        var conversation = CreateConversationEntity(1, 2, 3, "Hello");

        var dto = _mapper.Map<ConversationDTO>(conversation);

        dto.Status.ShouldBe("ACTIVE");
    }

    [Fact]
    public void Map_ConversationEntityToConversationDTO_MapsAllStatusValuesCorrectly()
    {
        var conversation = CreateConversationEntity(1, 2, 3, "Hello");
        conversation.CloseConversation();

        var dto = _mapper.Map<ConversationDTO>(conversation);

        dto.Status.ShouldBe("CLOSED");
    }

    [Fact]
    public void Map_ConversationEntityToConversationDetailsDTO_MapsBasicPropertiesCorrectly()
    {
        var conversation = CreateConversationEntity(1, 2, 3, "Initial message");
        typeof(ConversationEntity).GetProperty("Id")!.SetValue(conversation, 100);

        var dto = _mapper.Map<ConversationDetailsDTO>(conversation);

        dto.Id.ShouldBe(100);
        dto.OfferId.ShouldBe(1);
        dto.BuyerId.ShouldBe(2);
        dto.SellerId.ShouldBe(3);
        dto.LastMessage.ShouldBe("Initial message");
        dto.UnreadCount.ShouldBe(1);
    }

    [Fact]
    public void Map_ConversationEntityToConversationDetailsDTO_MapsStatusToUpperCaseString()
    {
        var conversation = CreateConversationEntity(1, 2, 3, "Hello");

        var dto = _mapper.Map<ConversationDetailsDTO>(conversation);

        dto.Status.ShouldBe("ACTIVE");
    }

    [Fact]
    public void Map_ConversationEntityToConversationDetailsDTO_MapsMessagesCorrectly()
    {
        var conversation = CreateConversationEntity(1, 2, 3, "First message");
        conversation.AddMessage(3, "Second message");
        conversation.AddMessage(2, "Third message");

        var dto = _mapper.Map<ConversationDetailsDTO>(conversation);

        dto.Messages.ShouldNotBeNull();
        dto.Messages.Count.ShouldBe(3);
        dto.Messages[0].Content.ShouldBe("First message");
        dto.Messages[1].Content.ShouldBe("Second message");
        dto.Messages[2].Content.ShouldBe("Third message");
    }

    [Fact]
    public void Map_ConversationEntityToConversationDetailsDTO_MapsMessageStatusCorrectly()
    {
        var conversation = CreateConversationEntity(1, 2, 3, "Hello");
        var message = conversation.Messages[0];
        message.MarkAsDelivered();

        var dto = _mapper.Map<ConversationDetailsDTO>(conversation);

        dto.Messages[0].Status.ShouldBe("DELIVERED");
    }

    [Fact]
    public void Map_ConversationEntityToConversationDetailsDTO_WithNoMessages_MapsEmptyList()
    {
        var conversation = CreateConversationEntity(1, 2, 3, "Hello");
        conversation.Messages.Clear();

        var dto = _mapper.Map<ConversationDetailsDTO>(conversation);

        dto.Messages.ShouldNotBeNull();
        dto.Messages.Count.ShouldBe(0);
    }

    [Fact]
    public void Map_ConversationEntityToConversationDTO_LastMessageAt_ShouldBeSetCorrectly()
    {
        var conversation = CreateConversationEntity(1, 2, 3, "Hello");
        var beforeUpdate = conversation.LastMessageAt;

        var dto = _mapper.Map<ConversationDTO>(conversation);

        dto.LastMessageAt.ShouldBe(beforeUpdate);
    }

    private static ConversationEntity CreateConversationEntity(int offerId, int buyerId, int sellerId, string initialMessage)
    {
        return new ConversationEntity(offerId, buyerId, sellerId, initialMessage);
    }
}
