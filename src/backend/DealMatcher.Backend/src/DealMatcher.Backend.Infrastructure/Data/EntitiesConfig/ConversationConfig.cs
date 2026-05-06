using DealMatcher.Backend.Core.Aggregates.Conversation;

namespace DealMatcher.Backend.Infrastructure.Data.EntitiesConfig;

public sealed class ConversationConfig
    : DealMatcherEntityBaseConfig<Conversation>
{
    public override void Configure(EntityTypeBuilder<Conversation> builder)
    {
        base.Configure(builder);

        builder.ToTable($"{nameof(Conversation)}s");

        builder.HasOne<Offer>()
            .WithMany()
            .HasForeignKey(c => c.OfferId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(c => c.BuyerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(c => c.SellerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.Property(c => c.LastMessage)
            .IsRequired();

        builder.Property(c => c.LastMessageAt)
            .IsRequired();

        builder.Property(c => c.UnreadCount)
            .IsRequired();

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.HasMany(c => c.Messages)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.OfferId);
        builder.HasIndex(c => c.BuyerId);
        builder.HasIndex(c => c.SellerId);

        builder.HasIndex(c => new
        {
            c.OfferId,
            c.BuyerId,
            c.SellerId
        }).IsUnique();
    }
}
