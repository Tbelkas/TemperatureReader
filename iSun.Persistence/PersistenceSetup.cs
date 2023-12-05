using iSun.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace iSun.Persistence;

public static class PersistenceSetup
{
    public static void Setup(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnectionString");
        serviceCollection.AddDbContext<AppDbContext>(options => { options.UseSqlServer(connectionString); });

        RegisterRepositories(serviceCollection);
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddTransient<ITemperatureRepository, TemperatureRepository>();
    }
}