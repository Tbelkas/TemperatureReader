using System.Net.Http.Json;
using iSun.Domain.Models;

namespace iSun.Domain.Services.External;

public class TemperatureService(HttpClient client) : ITemperatureService
{
    public async Task<Response<List<string>>> GetAvailableCities()
    {
        var response = await client.GetAsync("cities");
        if (!response.IsSuccessStatusCode)
        {
            return Response<List<string>>.Error("Something wrong happened");
        }

        var result = await response.Content.ReadFromJsonAsync<List<string>>();
        return Response<List<string>>.Success(result);
    }
    
    
    public async Task<Response<TemperatureReading>> GetWeatherReading(string city)
    {
        var response = await client.GetAsync($"weathers/{city}");
        if (!response.IsSuccessStatusCode)
        {
            return Response<TemperatureReading>.Error("Something wrong happened");
        }

        var result = await response.Content.ReadFromJsonAsync<TemperatureReading>();
        return result == null ? Response<TemperatureReading>.Error("Something wrong happened") : Response<TemperatureReading>.Success(result);
    }
}