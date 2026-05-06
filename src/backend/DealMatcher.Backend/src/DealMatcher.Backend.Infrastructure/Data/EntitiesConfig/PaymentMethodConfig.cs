using DealMatcher.Backend.Core.Aggregates.Payment;

namespace DealMatcher.Backend.Infrastructure.Data.EntitiesConfig;

public sealed class PaymentMethodConfig : DealMatcherEntityBaseConfig<PaymentMethod>
{
    public override void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        base.Configure(builder);
        builder.ToTable($"{nameof(PaymentMethod)}s");

        builder.Property(pm => pm.StringId)
            .HasMaxLength(DataSchemaConstants.DeliveryMethodNameMaxLength)
            .IsRequired();

        builder.HasIndex(pm => pm.StringId)
            .IsUnique();

        builder.Property(pm => pm.Name)
            .HasMaxLength(DataSchemaConstants.PaymentMethodNameMaxLength)
            .IsRequired();

        builder.Property(pm => pm.Provider)
            .HasMaxLength(DataSchemaConstants.PaymentMethodProviderMaxLength)
            .IsRequired();

        builder.Property(pm => pm.Icon)
            .HasMaxLength(DataSchemaConstants.ImageUrlMaxLength);
    }
}
