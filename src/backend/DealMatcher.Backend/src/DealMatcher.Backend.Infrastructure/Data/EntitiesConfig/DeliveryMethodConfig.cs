using DealMatcher.Backend.Core.Aggregates.Payment;

namespace DealMatcher.Backend.Infrastructure.Data.EntitiesConfig;

public sealed class DeliveryMethodConfig : DealMatcherEntityBaseConfig<DeliveryMethod>
{
    public override void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
        base.Configure(builder);
        builder.ToTable($"{nameof(DeliveryMethod)}s");

        builder.Property(dm => dm.StringId)
            .HasMaxLength(DataSchemaConstants.DeliveryMethodNameMaxLength)
            .IsRequired();

        builder.HasIndex(dm => dm.StringId)
            .IsUnique();

        builder.Property(dm => dm.Name)
            .HasMaxLength(DataSchemaConstants.DeliveryMethodNameMaxLength)
            .IsRequired();

        builder.Property(dm => dm.Description)
            .HasMaxLength(DataSchemaConstants.DeliveryMethodDescriptionMaxLength)
            .IsRequired();

        builder.Property(dm => dm.Price)
            .IsRequired();

        builder.Property(dm => dm.EstimatedDays)
            .IsRequired();
    }
}
