using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;

namespace api.silisync.Network.Extensions;

public static class InfrastructureServices
{
    public static void AddNetworkServices(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json",
                true, true)
            .AddUserSecrets<Program>(optional: true)
            .AddEnvironmentVariables();

        builder.Services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services));

        SetupSiliSyncApi(builder, configuration);

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

    private static void SetupSiliSyncApi(WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddControllers();
        builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        
        SetupAuthentication(builder, configuration);
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
    
    private static void SetupAuthentication(WebApplicationBuilder builder, IConfiguration configuration)
    { 
        var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SSyncApi:JwtSecret")!);
        builder.Services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }; 
            });
    }
}