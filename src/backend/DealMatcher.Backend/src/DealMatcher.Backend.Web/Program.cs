using System.Text;
using DealMatcher.Backend.Web.Configurations;
using DealMatcher.Backend.Web.Realtime;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DealMatcher.Backend.Web;

public sealed class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var logger = Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        logger.Information("Starting web host");

        builder.AddLoggerConfigs();

        var appLogger = new SerilogLoggerFactory(logger)
            .CreateLogger<Program>();
        try
        {
            builder.Services.AddServiceConfigs(appLogger, builder);
            builder.Services.AddSignalR();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtSection = builder.Configuration.GetSection("Authentication:Jwt");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSection["SecretKey"]!)),
                        ValidateIssuer = true,
                        ValidIssuer = jwtSection["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = jwtSection["Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(5)
                    };
                });
            builder.Services.AddAuthorization();
            builder.Services.AddFastEndpoints()
                .SwaggerDocument(o =>
                {
                    o.DocumentSettings = s =>
                    {
                        s.Title = "DealMatcher API";
                        s.Version = "1";
                    };
                    o.ShortSchemaNames = true;
                    o.MaxEndpointVersion = 1;
                })
                .AddCommandMiddleware(c =>
                {
                    c.Register(typeof(CommandLogger<,>));
                });

            var frontendOrigin = builder.Configuration.GetValue<string>("FrontendOrigin")
                                 ?? "http://localhost:4200";

            var additionalFrontends = builder.Configuration.GetValue<string>("AdditionalFrontends") ?? "";
            var additionalOrigins = string.IsNullOrWhiteSpace(additionalFrontends)
                ? Array.Empty<string>()
                : [.. additionalFrontends.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(o => o.Trim())];

            var allOrigins = new[] { frontendOrigin }.Concat(additionalOrigins).ToArray();

            builder.Services.AddCors(options =>
            {
                // options.AddPolicy("AllowFrontend", policy =>
                // {
                //     policy.WithOrigins(frontendOrigin)
                //         .AllowAnyHeader()
                //         .AllowAnyMethod()
                //         .AllowCredentials()
                //         .WithExposedHeaders("Content-Disposition", "Location");
                // });
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(allOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders("Content-Disposition", "Location");
                });
            });

            var app = builder.Build();

            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHub<ConversationHub>("/hubs/conversations");

            await app.UseAppMiddlewareAndSeedDatabase();

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            return;
        }
    }
}
