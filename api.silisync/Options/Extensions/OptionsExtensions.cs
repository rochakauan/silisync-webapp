using api.silisync.Exceptions;
using api.silisync.Options.Abstractions;
using Microsoft.Extensions.Options;

namespace api.silisync.Options.Extensions;

public static class OptionsExtensions
{
    private static OptionsBuilder<T> AddConfiguredOptions<T>(
        this IServiceCollection services,
        IConfiguration config,
        Action<OptionsBuilder<T>>? configure = null) 
            where T : class, IApiOptions
                => services.AddConfiguredOptions(config, T.SectionName, configure);

    private static OptionsBuilder<T> AddConfiguredOptions<T>(
        this IServiceCollection services,
        IConfiguration config,
        string sectionName,
        Action<OptionsBuilder<T>>? configure = null)
        where T : class, IApiOptions
    {
        var section = config.GetSection(sectionName);
        
        if (!section.Exists())
            throw new ConfigurationSectionNotFoundException(T.SectionName, typeof(T));
        
        var builder = services
            .AddOptions<T>()
            .Bind(config.GetRequiredSection(sectionName))
            .ValidateOnStart();
        
        configure?.Invoke(builder);
        
        return builder;
    }

    public static IServiceCollection AddSiliSyncApiOptions(this IServiceCollection services, IConfiguration config)
    {
        services.AddConfiguredOptions<SSyncApiOptions>(config, options =>
        {
            options.Validate(o => !string.IsNullOrWhiteSpace(o.JwtSecret),
                "JwtKey not set.");
        });
        
        return services;
    }

    public static IServiceCollection AddMeliApiOptions(this IServiceCollection services, IConfiguration config)
    {
        services.AddConfiguredOptions<MeliApiOptions>(config, options =>
        {
            options.Validate(o => !string.IsNullOrWhiteSpace(o.AppId),
                "AppId not set.");
            
            options.Validate(o => !string.IsNullOrWhiteSpace(o.ClientSecret),
                "ClientSecret not set.");
            
            options.Validate(o => !string.IsNullOrWhiteSpace(o.RedirectUri), 
                "RedirectUri not set.");
        });
        
        return services;
    }
}