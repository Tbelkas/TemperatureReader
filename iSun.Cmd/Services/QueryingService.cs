using iSun.Domain.Models;
using iSun.Domain.Services;
using iSun.Domain.Services.External;
using iSun.Persistence.Entities;
using iSun.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace iSun.Cmd.Services;

public class QueryingService(
    IConfiguration configuration,
    IContextService contextService,
    ITemperatureService temperatureService,
    IAuthService authService,
    ILogger<IQueryingService> logger,
    ITemperatureRepository temperatureRepository) : IQueryingService
{
    public async Task<IResponse> StartQuerying(List<string> cities)
    {
        var refreshAuthResponse = await RefreshAuthToken();
        if (!refreshAuthResponse.isSuccess)
        {
            logger.LogError("Token refresh failed. {errors}", refreshAuthResponse.errorMessages);
            return refreshAuthResponse;
        }

        var availableCities = await temperatureService.GetAvailableCities();
        if (!availableCities.isSuccess)
        {
            logger.LogError("Available city querying failed. {errors}", availableCities.errorMessages);
            return availableCities;
        }

        var notSupportedCities = cities.Where(x => !availableCities.Data.Contains(x)).ToList();
        if (notSupportedCities.Any())
        {
            var notSupportedCitiesString = string.Join(", ", notSupportedCities);
            logger.LogError("Service doesn't provide information for the following cities {notSupportedCitiesString}", notSupportedCitiesString);
            return Response.Error($"Service doesn't provide information for the following cities {notSupportedCitiesString}");
        }

        var waitInterval = int.Parse(configuration["TemperatureReadingIntervalInSeconds"]!);
        var temperatureReadings = new List<TemperatureReading>(cities.Count);
        
        // As there is no need for a graceful shutdown, an infinite loop is solution that seems to fit this use case
        // If we would need to return the values, we could use yields to have a generator that would pass the data upstream
        while (true)
        {
            temperatureReadings.Clear();
            
            // This could be parallelized. Decided to keep it sequential, just because it's not really clear from the task whether that's preferred and I don't want to bring
            // another layer of code just to show that I know how to do task.whenall();
            foreach (var city in cities)
            {
                var cityResult = await temperatureService.GetWeatherReading(city);
                if (!cityResult.isSuccess)
                {
                    logger.LogError("Service doesn't provide information for the following cities {cityResult}", cityResult.errorMessages);
                    return cityResult;
                }
                    
                Console.WriteLine(cityResult.Data.ToString());
                temperatureReadings.Add(cityResult.Data);
            }

            var temperatureReadingEntities = temperatureReadings.Select(x => new TemperatureReadingEntity
            {
                City = x.City,
                Precipitation = x.Precipitation,
                Summary = x.Summary,
                Temperature = x.Temperature,
                WindSpeed = x.WindSpeed
            });

            await temperatureRepository.InsertTemperatureReadings(temperatureReadingEntities);
            Thread.Sleep(waitInterval * 1000);
        }
    }

    private async Task<Response> RefreshAuthToken()
    {
        var credentials = configuration.GetSection("ApiCredentials");
        var result = await authService.GetToken(credentials["Username"], credentials["Password"]);
        if (!result.isSuccess)
        {
            return Response.Error("Authentication fail");
        }
        
        contextService.Token = result.Data.Token;
        return Response.Success();
    }
}