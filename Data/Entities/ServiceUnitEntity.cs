using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ServiceUnitEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Unit { get; set; } = null!;



    public ICollection<ServiceEntity> Services { get; set; } = [];
}
