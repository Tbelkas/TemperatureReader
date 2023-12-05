using iSun.Domain.Models;

namespace iSun.Domain.Services.External;

public interface ITemperatureService
{
    Task<Response<List<string>>> GetAvailableCities();
    Task<Response<TemperatureReading>> GetWeatherReading(string city);
}