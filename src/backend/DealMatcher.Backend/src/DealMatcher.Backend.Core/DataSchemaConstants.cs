namespace DealMatcher.Backend.Core;

public class DataSchemaConstants
{
    public const int DefaultNameLength = 100;
    public const int DefaultDescriptionLength = 500;

    public const int UserNameMaxLength = 100;
    public const int UserSurnameMaxLength = 100;
    public const int UserEmailMaxLength = 256;
    public const int UserPasswordHashLength = 128;
    public const int UserPasswordMinLength = 8;

    public const int TitleMaxLength = 200;
    public const int TitleMinLength = 3;
    public const int DescriptionMaxLength = 2000;
    public const int DescriptionMinLength = 0;
    public const int TagMaxLength = 50;
    public const int MaxTagsCount = 20;
    public const int ImageUrlMaxLength = 500;
    public const int MaxImagesCount = 10;
    public const int MinImagesCount = 0;
    public const decimal PriceMinValue = 0.01m;
    public const decimal PriceMaxValue = 999999.99m;
    public const int AvailabilityMinValue = 0;
    public const int AvailabilityMaxValue = 999999;

    public const int CategoryNameMaxLength = 100;
    public const int CategoryNameMinLength = 2;
    public const int CategoryDescriptionMaxLength = 500;
    public const int MaxCategoryProperties = 50;

    public const int PropertyNameMaxLength = 100;
    public const int PropertyNameMinLength = 1;
    public const int PropertyValueMaxLength = 500;
    public const int PropertyValueMinLength = 0;
    public const int PropertyOptionMaxLength = 200;
    public const int MaxPropertyOptions = 100;

    public const int CartItemQuantityMinValue = 1;
    public const int CartItemQuantityMaxValue = 999;

    public const int PaymentMethodNameMaxLength = 100;
    public const int PaymentMethodProviderMaxLength = 100;
    public const int DeliveryMethodNameMaxLength = 100;
    public const int DeliveryMethodDescriptionMaxLength = 1000;
}
