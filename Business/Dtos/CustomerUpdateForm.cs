using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class CustomerUpdateForm
{
    [Required]
    public int Id { get; set; }

    [Required]
    public bool IsCompany { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? CompanyName { get; set; }

    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = null!;
}
