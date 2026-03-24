namespace DealMatcher.Backend.Web.Configurations;

public static class MediatrConfigs
{
    public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
    {
        var mediatRAssemblies = new[]
          {
        Assembly.GetAssembly(typeof(Program)),
        Assembly.GetAssembly(typeof(DealMatcher.Backend.UseCases.Features.Offer.Get.GetOfferByIdQuery))
      };

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
                .AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

        return services;
    }
}
