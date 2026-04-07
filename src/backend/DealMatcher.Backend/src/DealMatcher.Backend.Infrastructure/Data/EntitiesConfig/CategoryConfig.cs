namespace DealMatcher.Backend.Infrastructure.Data.EntitiesConfig;

public sealed class CategoryConfig : DealMatcherEntityBaseConfig<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);
        builder.ToTable($"{nameof(Category)}s");

        builder.OwnsMany(c => c.Properties, prop =>
        {
            prop.ToTable("CategoryProperties");
            prop.WithOwner().HasForeignKey("Categoryd");
            prop.HasIndex(p => p.Name);
            prop.Property(p => p.Name).HasMaxLength(OfferConstants.PropertyNameMaxLength).IsRequired();
        });
    }
}
