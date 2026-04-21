namespace DealMatcher.Backend.Core.Aggregates.Category;

public abstract class CategoryPropertyType(
    string name,
    string value) :
    SmartEnum<CategoryPropertyType, string>(name, value)
{
    public static readonly CategoryPropertyType Text = new TextPropertyType();
    public static readonly CategoryPropertyType Number = new NumberPropertyType();
    public static readonly CategoryPropertyType Boolean = new BooleanPropertyType();
    public static readonly CategoryPropertyType Select = new SelectPropertyType();

    private sealed class SelectPropertyType() :
        CategoryPropertyType(nameof(SelectPropertyType), nameof(Select))
    {
    }

    private sealed class TextPropertyType() :
        CategoryPropertyType(nameof(TextPropertyType), nameof(Text))
    {
    }

    private sealed class NumberPropertyType() :
        CategoryPropertyType(nameof(NumberPropertyType), nameof(Number))
    {
    }

    private sealed class BooleanPropertyType() :
        CategoryPropertyType(nameof(BooleanPropertyType), nameof(Boolean))
    {
    }
}
