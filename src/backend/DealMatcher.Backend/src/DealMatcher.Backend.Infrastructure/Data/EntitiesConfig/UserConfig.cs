namespace DealMatcher.Backend.Infrastructure.Data.EntitiesConfig;

public sealed class UserConfig : DealMatcherEntityBaseConfig<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        builder.ToTable($"{nameof(User)}s");
    }
}
