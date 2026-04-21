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
            .HasMaxLength(DataSchemaConstants.UserNameMaxLength)
            .IsRequired();
        builder.Property(u => u.Surname)
            .HasMaxLength(DataSchemaConstants.UserSurnameMaxLength)
            .IsRequired();
        builder.Property(u => u.Email)
            .HasMaxLength(DataSchemaConstants.UserEmailMaxLength)
            .IsRequired();
        builder.Property(u => u.PasswordHash)
            .HasMaxLength(DataSchemaConstants.UserPasswordHashLength)
            .IsRequired();

        builder.HasIndex(u => u.Status);
        builder.HasIndex(u => u.Email);
    }
}
