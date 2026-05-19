namespace DealMatcher.Backend.Core.Aggregates.Admin;

public abstract class ActionType(
    string name,
    string value) :
    SmartEnum<ActionType, string>(name, value)
{
    public static readonly ActionType Create = new CreateActionType();
    public static readonly ActionType Update = new UpdateActionType();
    public static readonly ActionType Delete = new DeleteActionType();
    public static readonly ActionType View = new ViewActionType();
    public static readonly ActionType Purchase = new PurchaseActionType();
    public static readonly ActionType Status_Change = new StatusChangeActionType();
    public static readonly ActionType Login = new LoginActionType();
    public static readonly ActionType Logout = new LogoutActionType();

    private sealed class StatusChangeActionType() :
        ActionType(nameof(StatusChangeActionType), nameof(Status_Change))
    {
    }

    private sealed class LoginActionType() :
        ActionType(nameof(LoginActionType), nameof(Login))
    {
    }

    private sealed class LogoutActionType() :
        ActionType(nameof(LogoutActionType), nameof(Logout))
    {
    }


    private sealed class PurchaseActionType() :
        ActionType(nameof(PurchaseActionType), nameof(Purchase))
    {
    }

    private sealed class ViewActionType() :
        ActionType(nameof(ViewActionType), nameof(View))
    {
    }

    private sealed class CreateActionType() :
        ActionType(nameof(CreateActionType), nameof(Create))
    {
    }

    private sealed class UpdateActionType() :
        ActionType(nameof(UpdateActionType), nameof(Update))
    {
    }

    private sealed class DeleteActionType() :
        ActionType(nameof(DeleteActionType), nameof(Delete))
    {
    }
}
