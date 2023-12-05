using iSun.Persistence.Entities;

namespace iSun.Persistence.Repositories;

public interface ITemperatureRepository
{
    Task InsertTemperatureReadings(IEnumerable<TemperatureReadingEntity> readings);
}