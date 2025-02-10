using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class ServiceUnitRegistrationForm
{
    [Required]
    [MinLength(1, ErrorMessage = "Unit must be at least 1 character long.")]
    [MaxLength(10, ErrorMessage = "Unit cannot exceed 10 characters.")]
    public string Unit { get; set; } = null!;
}
