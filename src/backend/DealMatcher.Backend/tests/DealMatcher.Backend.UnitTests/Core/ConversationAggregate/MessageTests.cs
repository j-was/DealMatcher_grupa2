namespace DealMatcher.Backend.UnitTests.Core.ConversationAggregate;

public class MessageTests
{
    [Fact]
    public void Constructor_Should_Create_Message_With_Valid_Data()
    {
        var message = new Message(1, 2, "Hello!");

        message.ConversationId.ShouldBe(1);
        message.SenderId.ShouldBe(2);
        message.Content.ShouldBe("Hello!");
        message.Status.ShouldBe(MessageStatus.Sent);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_Should_Throw_When_SenderId_Is_Invalid(int invalidSenderId)
    {
        Should.Throw<ArgumentOutOfRangeException>(() =>
            new Message(1, invalidSenderId, "Hello"));
    }

    [Fact]
    public void MarkAsDelivered_Should_Update_Status_From_Sent()
    {
        var message = new Message(1, 2, "Hello");

        message.MarkAsDelivered();

        message.Status.ShouldBe(MessageStatus.Delivered);
    }

    [Fact]
    public void MarkAsDelivered_Should_Not_Change_Status_If_Not_Sent()
    {
        var message = new Message(1, 2, "Hello");
        message.MarkAsRead();

        message.MarkAsDelivered();

        message.Status.ShouldBe(MessageStatus.Read);
    }

    [Fact]
    public void MarkAsRead_Should_Set_Status_To_Read()
    {
        var message = new Message(1, 2, "Hello");

        message.MarkAsRead();

        message.Status.ShouldBe(MessageStatus.Read);
    }
}
