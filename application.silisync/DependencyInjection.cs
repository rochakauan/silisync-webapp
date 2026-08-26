using application.silisync.Interfaces.Application;
using application.silisync.UseCases;
using application.silisync.UseCases.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace application.silisync;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductApplication, ProductApplication>();
        services.AddScoped<IAuthApi, AuthApi>();
        services.AddScoped<IUserApplication, UserApplication>();
        
        return services;
    }
}