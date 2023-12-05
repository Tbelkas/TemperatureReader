using iSun.Domain.Handlers;
using iSun.Domain.Services;
using iSun.Domain.Services.External;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace iSun.Domain;

public static class DomainSetup
{
    public static void Setup(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddLogging();
        RegisterAppServices(serviceCollection);
        
        serviceCollection.AddTransient<AuthorizedHttpHandler>();

        serviceCollection.AddHttpClient(Options.DefaultName, x =>
        {
            x.BaseAddress = new Uri(configuration["ApiUrl"]!);
        });    

        serviceCollection.AddHttpClient<ITemperatureService, TemperatureService>(x =>
        {
            x.BaseAddress = new Uri(configuration["ApiUrl"]!);
        }).AddHttpMessageHandler<AuthorizedHttpHandler>();

        
    }

    
    private static void RegisterAppServices(IServiceCollection serviceCollection)
    {

        serviceCollection.AddTransient<ITemperatureService, TemperatureService>();
        serviceCollection.AddTransient<IAuthService, AuthService>();
        serviceCollection.AddSingleton<IContextService, ContextService>();
    }

}