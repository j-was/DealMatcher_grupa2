namespace DealMatcher.Backend.UnitTests.Core.ActivityAggregate;

public class ActionTypeTests
{
    [Fact]
    public void ActionType_Values_ShouldBeCorrect()
    {
        ActionType.Create.Value.ShouldBe("Create");
        ActionType.Update.Value.ShouldBe("Update");
        ActionType.Delete.Value.ShouldBe("Delete");
        ActionType.View.Value.ShouldBe("View");
        ActionType.Purchase.Value.ShouldBe("Purchase");
        ActionType.Status_Change.Value.ShouldBe("Status_Change");
        ActionType.Login.Value.ShouldBe("Login");
        ActionType.Logout.Value.ShouldBe("Logout");
    }

    [Fact]
    public void ActionType_FromValue_ShouldReturnCorrectType()
    {
        var create = ActionType.FromValue("Create");
        var update = ActionType.FromValue("Update");
        var delete = ActionType.FromValue("Delete");
        var view = ActionType.FromValue("View");
        var purchase = ActionType.FromValue("Purchase");
        var statusChange = ActionType.FromValue("Status_Change");
        var login = ActionType.FromValue("Login");
        var logout = ActionType.FromValue("Logout");

        create.ShouldBe(ActionType.Create);
        update.ShouldBe(ActionType.Update);
        delete.ShouldBe(ActionType.Delete);
        view.ShouldBe(ActionType.View);
        purchase.ShouldBe(ActionType.Purchase);
        statusChange.ShouldBe(ActionType.Status_Change);
        login.ShouldBe(ActionType.Login);
        logout.ShouldBe(ActionType.Logout);
    }

    [Fact]
    public void ActionType_FromName_ShouldReturnCorrectType()
    {
        ActionType.FromName("CreateActionType").ShouldBe(ActionType.Create);
        ActionType.FromName("UpdateActionType").ShouldBe(ActionType.Update);
        ActionType.FromName("DeleteActionType").ShouldBe(ActionType.Delete);
        ActionType.FromName("ViewActionType").ShouldBe(ActionType.View);
        ActionType.FromName("PurchaseActionType").ShouldBe(ActionType.Purchase);
        ActionType.FromName("StatusChangeActionType").ShouldBe(ActionType.Status_Change);
        ActionType.FromName("LoginActionType").ShouldBe(ActionType.Login);
        ActionType.FromName("LogoutActionType").ShouldBe(ActionType.Logout);
    }
}
