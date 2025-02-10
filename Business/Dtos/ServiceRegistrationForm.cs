using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class ServiceRegistrationForm
{
    [Required]
    [MinLength(2, ErrorMessage = "Service name must be at least 2 characters long.")]
    [MaxLength(100, ErrorMessage = "Service name cannot exceed 100 characters.")]
    public string Name { get; set; } = null!;

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int UnitId { get; set; }
}
