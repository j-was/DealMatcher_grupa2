namespace DealMatcher.Backend.Infrastructure.Data.EntitiesConfig;

public class DealMatcherEntityBaseConfig<T> : IEntityTypeConfiguration<T> where T : DealMatcherEntityBase
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .UseIdentityColumn();

        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.DeletedAt).IsRequired(false);

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();
    }
}
