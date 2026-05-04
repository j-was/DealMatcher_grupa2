using DealMatcher.Backend.Core.Aggregates.Conversation;

namespace DealMatcher.Backend.Infrastructure.Data.EntitiesConfig;

public sealed class MessageConfig
    : DealMatcherEntityBaseConfig<Message>
{
    public override void Configure(EntityTypeBuilder<Message> builder)
    {
        base.Configure(builder);

        builder.ToTable($"{nameof(Message)}s");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne<Conversation>()
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(m => m.Content)
            .IsRequired();

        builder.Property(m => m.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.HasIndex(m => m.SenderId);
        builder.HasIndex(m => m.CreatedAt);
    }
}
