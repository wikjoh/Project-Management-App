using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;

namespace Data.Entities;

[Index(nameof(Unit), IsUnique = true)]
public class ServiceUnitEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(10)")]
    public string Unit { get; set; } = null!;



    public ICollection<ServiceEntity> Services { get; set; } = [];
}
