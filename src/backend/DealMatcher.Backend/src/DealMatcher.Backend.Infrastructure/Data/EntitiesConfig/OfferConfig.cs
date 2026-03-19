namespace DealMatcher.Backend.Infrastructure.Data.EntitiesConfig;

public sealed class OfferConfig : DealMatcherEntityBaseConfig<Offer>
{
    public override void Configure(EntityTypeBuilder<Offer> builder)
    {
        base.Configure(builder);
        builder.ToTable($"{nameof(Offer)}s");

        builder.HasOne<User>()
          .WithOne()
          .HasForeignKey<Offer>(o => o.SellerId)
          .OnDelete(DeleteBehavior.Restrict)
          .IsRequired();

        builder.HasOne<User>()
          .WithOne()
          .HasForeignKey<Offer>(o => o.SellerId)
          .OnDelete(DeleteBehavior.Restrict)
          .IsRequired();

        builder.Property(o => o.Status)
          .HasConversion(s => s.Value, s => OfferStatus.FromValue(s))
          .IsRequired();

        builder.OwnsMany(o => o.Properties, item =>
        {
            item.Property(i => i.Name).IsRequired();
            item.Property(i => i.Value).HasColumnType("");
        });

        builder.PrimitiveCollection(o => o.Tags)
          .UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.PrimitiveCollection(o => o.ImageUrls)
          .UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.OwnsMany(o => o.Properties, prop =>
        {
            prop.ToTable("OfferProperties");
            prop.WithOwner().HasForeignKey("OfferId");
            prop.HasIndex(p => p.Name);
            prop.Property(p => p.Name).HasMaxLength(200);
            prop.Property(p => p.Value).HasMaxLength(500);
        });

        builder.Navigation(o => o.Properties)
          .UsePropertyAccessMode(PropertyAccessMode.Property);
    }
}
