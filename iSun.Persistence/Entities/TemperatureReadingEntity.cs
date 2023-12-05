using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace iSun.Persistence.Entities;

[Table("TemperatureReadings")]
public class TemperatureReadingEntity : BaseAuditEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    
    // todo: city as separate entity
    public string City { get; set; }
    public int Temperature { get; set; }
    public int Precipitation { get; set; }
    public int WindSpeed { get; set; }
    public string Summary { get; set; }
}