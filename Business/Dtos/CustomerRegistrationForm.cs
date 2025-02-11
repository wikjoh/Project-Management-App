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


    public CustomerPhoneNumberForm PhoneNumber { get; set; } = null!;
}
