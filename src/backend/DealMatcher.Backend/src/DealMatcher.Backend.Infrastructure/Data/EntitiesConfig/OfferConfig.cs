namespace DealMatcher.Backend.Infrastructure.Data.EntitiesConfig;

public sealed class OfferConfig : DealMatcherEntityBaseConfig<Offer>
{
    public override void Configure(EntityTypeBuilder<Offer> builder)
    {
        base.Configure(builder);
        builder.ToTable($"{nameof(Offer)}s");

        builder.HasOne<User>()
          .WithMany()
          .HasForeignKey(o => o.SellerId)
          .OnDelete(DeleteBehavior.Restrict)
          .IsRequired();

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(o => o.CategoryId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.Property(o => o.Title)
            .HasMaxLength(DataSchemaConstants.TitleMaxLength)
            .IsRequired();

        builder.Property(o => o.Description)
            .HasMaxLength(DataSchemaConstants.DescriptionMaxLength)
            .IsRequired();

        builder.Property(o => o.Price)
            .IsRequired();

        builder.Property(o => o.Availability)
            .IsRequired();

        builder.Property(o => o.UpdatedAt)
            .IsRequired();

        builder.Property(o => o.Status)
          .HasConversion(s => s.Value, s => OfferStatus.FromValue(s))
          .IsRequired();

        builder.PrimitiveCollection(o => o.Tags)
          .ElementType(t => t.HasMaxLength(DataSchemaConstants.TagMaxLength))
          .UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.PrimitiveCollection(o => o.ImageUrls)
          .ElementType(t => t.HasMaxLength(DataSchemaConstants.ImageUrlMaxLength))
          .UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.OwnsMany(o => o.Properties, prop =>
        {
            prop.ToTable("OfferProperties");
            prop.WithOwner().HasForeignKey("OfferId");
            prop.HasIndex(p => p.Name);
            prop.Property(p => p.Name).HasMaxLength(DataSchemaConstants.PropertyNameMaxLength).IsRequired();
            prop.Property(p => p.Value).HasMaxLength(DataSchemaConstants.PropertyValueMaxLength).IsRequired();
        });

        builder.Navigation(o => o.Properties)
          .UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.HasIndex(o => o.SellerId);
        builder.HasIndex(o => o.CategoryId);
        builder.HasIndex(o => o.Status);
        builder.HasIndex(o => o.Price);
    }
}
