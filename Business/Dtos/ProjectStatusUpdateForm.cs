using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class ProjectStatusUpdateForm
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "Status name must be at least 2 characters long.")]
    [MaxLength(50, ErrorMessage = "Status name cannot exceed 50 characters.")]
    public string Name { get; set; } = null!;
}
