namespace DealMatcher.Backend.UnitTests.Core.ConversationAggregate;

public class ConversationTests
{
    [Fact]
    public void Constructor_Should_Create_Conversation_With_Valid_Data()
    {
        var conversation = new Conversation(1, 2, 3, "Hello, is this available?");

        conversation.OfferId.ShouldBe(1);
        conversation.BuyerId.ShouldBe(2);
        conversation.SellerId.ShouldBe(3);
        conversation.LastMessage.ShouldBe("Hello, is this available?");
        conversation.Status.ShouldBe(ConversationStatus.Active);
        conversation.UnreadCount.ShouldBe(1);
        conversation.Messages.Count.ShouldBe(1);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_Should_Throw_When_OfferId_Is_Invalid(int invalidOfferId)
    {
        Should.Throw<ArgumentOutOfRangeException>(() =>
            new Conversation(invalidOfferId, 2, 3, "Hello"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_Should_Throw_When_BuyerId_Is_Invalid(int invalidBuyerId)
    {
        Should.Throw<ArgumentOutOfRangeException>(() =>
            new Conversation(1, invalidBuyerId, 3, "Hello"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_Should_Throw_When_SellerId_Is_Invalid(int invalidSellerId)
    {
        Should.Throw<ArgumentOutOfRangeException>(() =>
            new Conversation(1, 2, invalidSellerId, "Hello"));
    }

    [Fact]
    public void AddMessage_Should_Add_Message_And_Update_LastMessage()
    {
        var conversation = new Conversation(1, 2, 3, "Initial message");

        var message = conversation.AddMessage(3, "Yes, it's available!");

        conversation.Messages.Count.ShouldBe(2);
        conversation.LastMessage.ShouldBe("Yes, it's available!");
        conversation.UnreadCount.ShouldBe(2);
        message.SenderId.ShouldBe(3);
        message.Content.ShouldBe("Yes, it's available!");
    }

    [Fact]
    public void AddMessage_Should_Throw_When_Conversation_Is_Closed()
    {
        var conversation = new Conversation(1, 2, 3, "Hello");
        conversation.CloseConversation();

        Should.Throw<InvalidOperationException>(() =>
                conversation.AddMessage(3, "Can't send this"))
            .Message.ShouldContain("Cannot add new message to closed conversation");
    }

    [Fact]
    public void AddMessage_Should_Throw_When_Sender_Not_In_Conversation()
    {
        var conversation = new Conversation(1, 2, 3, "Hello");

        Should.Throw<ArgumentException>(() =>
                conversation.AddMessage(999, "Invalid sender"))
            .Message.ShouldContain("Sender must belong to conversation");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void AddMessage_Should_Throw_When_Content_Is_Empty(string invalidContent)
    {
        var conversation = new Conversation(1, 2, 3, "Hello");

        Should.Throw<ArgumentException>(() =>
            conversation.AddMessage(3, invalidContent));
    }

    [Fact]
    public void MarkAsRead_Should_Reset_UnreadCount()
    {
        var conversation = new Conversation(1, 2, 3, "Hello");
        conversation.AddMessage(3, "Reply");

        conversation.MarkAsRead();

        conversation.UnreadCount.ShouldBe(0);
    }

    [Fact]
    public void CloseConversation_Should_Set_Status_To_Closed()
    {
        var conversation = new Conversation(1, 2, 3, "Hello");

        conversation.CloseConversation();

        conversation.Status.ShouldBe(ConversationStatus.Closed);
    }

    [Fact]
    public void Constructor_Should_Trim_InitialMessage()
    {
        var conversation = new Conversation(1, 2, 3, "  Hello there  ");

        conversation.LastMessage.ShouldBe("Hello there");
        conversation.Messages[0].Content.ShouldBe("Hello there");
    }
}
