using System.ComponentModel.DataAnnotations;

namespace iSun.Persistence.Entities;

public abstract class BaseAuditEntity
{
    [Required]
    public DateTime CreatedDate { get; set; }
}