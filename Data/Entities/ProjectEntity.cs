using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime? StartDate { get; set; } = null;

    [Column(TypeName = "date")]
    public DateTime? EndDate { get; set; } = null;

    [Required]
    public int ProjectManagerId { get; set; }

    [Required]
    public string CustomerName { get; set; } = null!;

    [Required]
    public int CustomerId { get; set; }

    [Required]
    public int ServiceId { get; set; }

    public int? ServiceQuantity { get; set; } = null;

    [Column(TypeName = "decimal(18,2)")]
    public decimal? TotalPrice { get; set; } = null;

    [Required]
    public int StatusId { get; set; }



    public CustomerEntity Customer { get; set; } = null!;
    public UserEntity ProjectManager { get; set; } = null!;
    public ServiceEntity Service { get; set; } = null!;
    public ProjectStatusEntity Status { get; set; } = null!;
}
