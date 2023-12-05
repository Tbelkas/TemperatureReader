using iSun.Cmd.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace iSun.Cmd.Extensions;

public static class ServiceCollectionExtensions
{
    public static void Setup(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddLogging();
        serviceCollection.RegisterAppServices();

        serviceCollection.AddSingleton(configuration);
    }

    
    private static void RegisterAppServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IQueryingService, QueryingService>();
    }
    
}