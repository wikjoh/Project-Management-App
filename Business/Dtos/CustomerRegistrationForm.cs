using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class CustomerRegistrationForm
{
    [Required]
    public bool IsCompany { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? CompanyName { get; set; }

    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = null!;


    [Required]
    [RegularExpression(@"^\d{5,20}$", ErrorMessage = "Phone number must be at least 5 digits and at most 20 digits")]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    public bool IsWorkNumber { get; set; }

    [Required]
    public bool IsCellNumber { get; set; }

    [Required]
    public bool IsHomeNumber { get; set; }
}
