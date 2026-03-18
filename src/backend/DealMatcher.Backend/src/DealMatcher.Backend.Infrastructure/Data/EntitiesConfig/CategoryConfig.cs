namespace DealMatcher.Backend.Infrastructure.Data.EntitiesConfig;

public sealed class CategoryConfig : DealMatcherEntityBaseConfig<Category>
{
  public override void Configure(EntityTypeBuilder<Category> builder)
  {
    base.Configure(builder);
    builder.ToTable($"{nameof(Category)}s");
  }
}
