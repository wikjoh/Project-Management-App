using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class CustomerPhoneNumberModel
{
    public int CustomerId { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public bool IsWorkNumber { get; set; }

    public bool IsCellNumber { get; set; }

    public bool IsHomeNumber { get; set; }
}
