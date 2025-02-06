using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class CustomerPhoneNumberEntity
{
    [Required]
    public int CustomerId { get; set; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    public bool IsWorkNumber { get; set; }

    [Required]
    public bool IsCellNumber { get; set; }

    [Required]
    public bool IsHomeNumber { get; set; }



    public CustomerEntity Customer { get; set; } = null!;
}
