namespace DealMatcher.Backend.Infrastructure.Data.EntitiesConfig;

public class ActivityRecordConfig : DealMatcherEntityBaseConfig<ActivityRecord>
{
    public override void Configure(EntityTypeBuilder<ActivityRecord> builder)
    {
        base.Configure(builder);
        builder.ToTable($"{nameof(ActivityRecord)}s");

        builder.HasOne<User>()
          .WithMany()
          .HasForeignKey(a => a.UserId)
          .OnDelete(DeleteBehavior.Restrict)
          .IsRequired();

        builder.HasOne<Offer>()
            .WithMany()
            .HasForeignKey(a => a.OfferId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(a => a.Action)
          .HasConversion(t => t.Value, t => ActionType.FromValue(t))
          .IsRequired();

        builder.OwnsMany(a => a.Details, d =>
        {
            d.ToTable("ActivityRecordDetails");
            d.WithOwner().HasForeignKey("ActivityId");
            d.HasIndex(p => p.Name);
            d.Property(p => p.Name).IsRequired();
            d.Property(p => p.Value).IsRequired();
        });

        builder.Navigation(a => a.Details)
          .UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.HasIndex(a => a.UserId);
        builder.HasIndex(a => a.OfferId);
    }
}
