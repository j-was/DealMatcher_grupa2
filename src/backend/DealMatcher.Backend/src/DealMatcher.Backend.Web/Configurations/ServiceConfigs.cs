using DealMatcher.Backend.Core.Interfaces;
using DealMatcher.Backend.Infrastructure;
using DealMatcher.Backend.Infrastructure.Configs;
using DealMatcher.Backend.Infrastructure.Email;

namespace DealMatcher.Backend.Web.Configurations;

public static class ServiceConfigs
{
    public static IServiceCollection AddServiceConfigs(this IServiceCollection services, Microsoft.Extensions.Logging.ILogger logger, WebApplicationBuilder builder)
    {
        services.AddInfrastructureServices(builder.Configuration, logger)
                .AddMediatrConfigs();

        logger.LogInformation("{Project} services registered", "Mediatr and Email Sender");

        return services;
    }


}
