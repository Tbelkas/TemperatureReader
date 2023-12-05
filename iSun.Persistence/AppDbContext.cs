using iSun.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace iSun.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<TemperatureReadingEntity> TemperatureReadings { get; init; } = null!;
    public AppDbContext()
    {
        
    }
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        ApplyAuditInformation();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInformation()
    {
        var entities = ChangeTracker.Entries().Where(x => x is { Entity: BaseAuditEntity, State: EntityState.Added });
        foreach (var entityEntry in entities)
        {
            var entity = (BaseAuditEntity) entityEntry.Entity;
            entity.CreatedDate = DateTime.Now;
        }
    }
}