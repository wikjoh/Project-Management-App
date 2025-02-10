using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class ProjectRegistrationForm
{
    [Required]
    public string Name { get; set; } = null!;

    public DateTime? StartDate { get; set; } = null;

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

    public decimal? TotalPrice { get; set; } = null;

    [Required]
    public int StatusId { get; set; }
}
