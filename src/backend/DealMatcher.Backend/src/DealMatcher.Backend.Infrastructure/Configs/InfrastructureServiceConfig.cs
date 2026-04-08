using DealMatcher.Backend.Infrastructure.Authentication;
using DealMatcher.Backend.Infrastructure.Data;
using DealMatcher.Backend.Infrastructure.Images;

namespace DealMatcher.Backend.Infrastructure.Configs;

public static class InfrastructureServiceConfig
{
    public static IServiceCollection AddInfrastructureServices(
      this IServiceCollection services,
      ConfigurationManager config,
      ILogger logger)
    {
        string? connectionString;
        try
        {
            connectionString = config.GetConnectionString("DefaultConnection");
            Guard.Against.Null(connectionString);
        }
        catch
        {
            logger.LogError("Default connection string was not defined in the environment");
            throw;
        }
        services.AddApplicationDbContext(connectionString);
        services.AddAutoMapperConfigs();

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
          .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddScoped<IImageService, AzureBlobImageService>();

        services.AddScoped<IPasswordHashService, BcryptHashService>();
        services.AddScoped<IPasswordValidator, PasswordValidator>();
        services.AddScoped<ITokenProvider, JwtTokenProvider>();
        services.AddAuthenticationServices(config, logger);

        logger.LogInformation("{Project} services registered", "Infrastructure");


        return services;
    }
}
