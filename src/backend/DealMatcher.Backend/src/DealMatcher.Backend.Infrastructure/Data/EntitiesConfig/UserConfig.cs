namespace DealMatcher.Backend.Infrastructure.Data.EntitiesConfig;

public sealed class UserConfig : DealMatcherEntityBaseConfig<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        builder.ToTable($"{nameof(User)}s");

        builder.Property(u => u.Status)
            .HasConversion(s => s.Value, s => UserStatus.FromValue(s))
            .IsRequired();
        builder.Property(u => u.Name)
            .IsRequired();
        builder.Property(u => u.Surname)
            .IsRequired();
        builder.Property(u => u.Email)
            .IsRequired();
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(128);
    }
}
