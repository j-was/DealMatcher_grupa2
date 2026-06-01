namespace DealMatcher.Backend.Infrastructure.Data.EntitiesConfig;

public sealed class CartItemConfig : DealMatcherEntityBaseConfig<CartItem>
{
    public override void Configure(EntityTypeBuilder<CartItem> builder)
    {
        base.Configure(builder);
        builder.ToTable($"{nameof(CartItem)}s");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.Property(c => c.Quantity)
            .IsRequired();

        builder.HasOne<Offer>()
            .WithMany()
            .HasForeignKey(c => c.OfferId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.Property(c => c.AddedAt)
            .IsRequired();

        builder.HasIndex(c => c.UserId);
    }
}
