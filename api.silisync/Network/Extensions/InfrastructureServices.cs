using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using Serilog;

namespace api.silisync.Network.Extensions;

public static class InfrastructureServices
{
    public static void AddNetworkServices(this WebApplicationBuilder builder)
    {
        builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json",
                true, true)
            .AddEnvironmentVariables();

        builder.Services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services));

        SetupSiliSyncApi(builder);

        builder.Services.AddHttpClient("MercadoLibre", client => 
            { client.DefaultRequestHeaders.Add("Accept", "application/json"); });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();
    }

    public static void UseCustomMiddlewares(this WebApplication app)
    {
        SerilogRequestLogging(app);
        SetupCors(app);
        
        app.MapGet("/", () => Results.Redirect("/scalar/v1"));

        SetupApiDocumentation(app);
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();
    }

    private static void SetupSiliSyncApi(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

        builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddIdentityCookies();
        builder.Services.AddAuthorization();
        builder.Services.AddCors();
    }
    
    private static void SerilogRequestLogging(WebApplication app)
        => app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate =
                "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            options.GetLevel = (_, _, ex) =>
                ex != null ? Serilog.Events.LogEventLevel.Error : Serilog.Events.LogEventLevel.Debug;
        }); 

    private static void SetupApiDocumentation(WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return;
        
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("SiliSync API")
                .WithTheme(ScalarTheme.DeepSpace)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
    }

    private static void SetupCors(WebApplication app)
        => app.UseCors(x => x
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:4200", "https://localhost:4200"));
}