using DealMatcher.Backend.Infrastructure.Data;

namespace DealMatcher.Backend.Infrastructure.Configs;

public static class DatabaseConfig
{
  public static void AddApplicationDbContext(this IServiceCollection services, string connectionString) =>
    services.AddDbContext<AppDbContext>(options =>
         options.UseSqlServer(connectionString));

}
