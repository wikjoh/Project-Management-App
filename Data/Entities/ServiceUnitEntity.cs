using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ServiceUnitEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(10)")]
    public string Unit { get; set; } = null!;



    public ICollection<ServiceEntity> Services { get; set; } = [];
}
