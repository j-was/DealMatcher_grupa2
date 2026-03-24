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

        builder.Property(o => o.Status)
          .HasConversion(s => s.Value, s => OfferStatus.FromValue(s))
          .IsRequired();

        builder.OwnsMany(o => o.Properties, pb =>
        {
            pb.ToTable("OfferProperties");
            pb.WithOwner().HasForeignKey("OfferId");
            pb.Property(p => p.Name).IsRequired();
            pb.Property(p => p.Value).IsRequired();
        });

        builder.PrimitiveCollection(o => o.Tags)
            .UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.PrimitiveCollection(o => o.ImageUrls)
          .UsePropertyAccessMode(PropertyAccessMode.Property);
    }
}
