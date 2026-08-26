using api.silisync;
using api.silisync.Network.Extensions;
using api.silisync.Options.Extensions;
using application.silisync;
using persistence.silisync;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

return await AppStartupGuard.RunAfterValidatesAsync(async () =>
{
    Log.Information("Attempting to initialize the application...");
    
    var builder = WebApplication.CreateBuilder(args);

    builder.AddNetworkServices(builder.Configuration);
    
    builder.Services
        .AddSiliSyncApiOptions(builder.Configuration)
        .AddMeliApiOptions(builder.Configuration);
    
    builder.Services.AddPersistence(builder.Configuration);
    builder.Services.AddApplication();

    var app = builder.Build();
    Log.Information("Web Application built. Now attempting to start the Web API...");
    
    app.UseCustomMiddlewares();
    Log.Information("Everything set up. Running the application...");
    
    try
    {
        await app.RunAsync();
    }
    catch (Exception ex) when (ex is OperationCanceledException or HostAbortedException)
    {
        Log.Information("Application shutdown initialized via terminal/host.");
    }
    
    return 0;
});

