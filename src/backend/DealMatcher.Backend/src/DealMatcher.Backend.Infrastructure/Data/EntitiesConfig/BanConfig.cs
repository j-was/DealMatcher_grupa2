namespace DealMatcher.Backend.Infrastructure.Data.EntitiesConfig;

public sealed class BanConfig : DealMatcherEntityBaseConfig<Ban>
{
    public override void Configure(EntityTypeBuilder<Ban> builder)
    {
        base.Configure(builder);
        builder.ToTable($"{nameof(Ban)}s");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(b => b.IssuedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.Property(b => b.Reason)
            .HasMaxLength(DataSchemaConstants.BanReasonMaxLength)
            .IsRequired();

        builder.Property(b => b.IssuedAt)
            .IsRequired();

        builder.Property(b => b.ExpiresAt);

        builder.Property(b => b.IsActive)
            .IsRequired();

        builder.HasIndex(b => b.UserId);
        builder.HasIndex(b => b.IssuedBy);
        builder.HasIndex(b => b.IsActive);
    }
}
