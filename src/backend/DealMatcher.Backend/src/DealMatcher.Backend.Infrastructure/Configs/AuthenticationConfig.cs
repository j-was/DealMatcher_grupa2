using DealMatcher.Backend.Infrastructure.Authentication;

namespace DealMatcher.Backend.Infrastructure.Configs;

public static class AuthenticationConfig
{
    public static IServiceCollection AddAuthenticationServices(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger)
    {
        services.AddScoped<ITokenProvider, JwtTokenProvider>();

        var jwtSettings = configuration.GetSection("Authentication:Jwt");
        var secretKey = jwtSettings["SecretKey"]
                        ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var issuer = jwtSettings["Issuer"]
                     ?? throw new InvalidOperationException("JWT Issuer not configured");
        var audience = jwtSettings["Audience"]
                       ?? throw new InvalidOperationException("JWT Audience not configured");
        _ = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(5),
            RequireExpirationTime = true
        };

        logger.LogInformation("Authentication services registered");

        return services;
    }
}
