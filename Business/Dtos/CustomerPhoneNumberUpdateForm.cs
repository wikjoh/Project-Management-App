using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class CustomerPhoneNumberUpdateForm
{
    [Required]
    public int CustomerId { get; set; }

    [Required]
    [RegularExpression(@"^\d{5,20}$", ErrorMessage = "Phone number must be at least 5 digits and at most 20 digits")]
    public string PhoneNumber { get; set; } = null!;

    public string? ExistingPhoneNumber { get; set; } = null;

    [Required]
    public bool IsWorkNumber { get; set; }

    [Required]
    public bool IsCellNumber { get; set; }

    [Required]
    public bool IsHomeNumber { get; set; }
}
