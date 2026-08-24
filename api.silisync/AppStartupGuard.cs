using api.silisync.Exceptions;
using Microsoft.Extensions.Options;
using Serilog;

namespace api.silisync;

public static class AppStartupGuard
{
    public static async Task<int> RunAfterValidatesAsync(Func<Task<int>> startupAction)
    {
        try
        {
            return await startupAction();
        }   
        catch (ConfigurationSectionNotFoundException ex)
        {
            Log.Fatal("Missing configuration section {SectionName} required by {OptionsType}",
                ex.SectionName, ex.OptionsType);
            return 1;
        }

        catch (OptionsValidationException ex)
        {
            Log.Fatal("Invalid configuration: {Failures}", string.Join("; ", ex.Failures));
            return 1;
        }

        catch (Exception ex)
        {
            try
            {
                Log.Fatal(ex, "An unhandled exception occurred during the initialization of the application: {Message}",
                    ex.Message);
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine($"Fatal Error during shutdown (ServiceProvider Disposed): {ex.Message}");
            }

            return 1;
        }
        finally{
            Log.Information("Application shut down gracefully.");
            await Log.CloseAndFlushAsync();
        }
    }
}