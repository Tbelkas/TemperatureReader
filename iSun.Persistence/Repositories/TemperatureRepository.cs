using iSun.Persistence.Entities;

namespace iSun.Persistence.Repositories;

public class TemperatureRepository(AppDbContext dbContext)  : ITemperatureRepository
{
    public async Task InsertTemperatureReadings(IEnumerable<TemperatureReadingEntity> readings)
    {
        dbContext.TemperatureReadings.AddRange(readings);
        await dbContext.SaveChangesAsync();
    }
}