using GotraysApp.Helper;
using GotraysApp.Services;

namespace GotraysApp.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGotraysApp(this IServiceCollection services)
    {
        services.AddScoped<AppService>();
        services.AddScoped<ChatProductService>();
        services.AddScoped<StorageService>();
        services.AddScoped<UserService>();
        services.AddScoped<GotraysModule>();
        services.AddScoped<ApiClientHelper>();
        return services;
    }

}
