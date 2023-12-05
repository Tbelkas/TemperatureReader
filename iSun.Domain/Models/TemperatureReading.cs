namespace iSun.Domain.Models;

public record TemperatureReading(string City, int Temperature, int Precipitation, int WindSpeed, string Summary);

