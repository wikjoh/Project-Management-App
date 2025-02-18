using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class UserUpdateForm
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "First name must be at least 2 characters long.")]
    [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
    public string FirstName { get; set; } = null!;

    [Required]
    [MinLength(2, ErrorMessage = "Last name must be at least 2 characters long.")]
    [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    [MaxLength(200, ErrorMessage = "Email address cannot exceed 200 characters.")]
    public string EmailAddress { get; set; } = null!;

    [Required]
    public IEnumerable<int> RoleIds { get; set; } = [];
}
