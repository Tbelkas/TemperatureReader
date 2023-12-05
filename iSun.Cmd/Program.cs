using System.Reflection;
using CommandLine;
using iSun.Cmd.Extensions;
using iSun.Cmd.Services;
using iSun.Domain;
using iSun.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace iSun.Cmd;

class Program
{

    static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets(Assembly.GetExecutingAssembly()) // technically, the connection string should be a secret as well, but wanted to keep the project as easy to launch as possible.
            .Build();
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                PersistenceSetup.Setup(services, configuration);
                DomainSetup.Setup(services, configuration);
                services.Setup(configuration);
            })
            .Build();

        Parser.Default.ParseArguments<CommandLineOptions>(args)
            .WithParsed(o =>
            {
                var queryingService = host.Services.GetService<IQueryingService>();

                var task = queryingService!.StartQuerying(o.Cities.ToList());
                task.Wait();
            });
     
        host.Run();
    }
    
}
