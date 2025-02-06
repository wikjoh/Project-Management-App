using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Index(nameof(EmailAddress), IsUnique = true)]
public class CustomerEntity
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(100)")]
    public string? FirstName { get; set; }

    [Column(TypeName = "nvarchar(100)")]
    public string? LastName { get; set; }

    [Column(TypeName = "nvarchar(200)")]
    public string? CompanyName { get; set; }

    [Column(TypeName = "nvarchar(200)")]
    public string DisplayName { get; private set; } = null!; // Computed column for storing combined firstname + lastname or companyname depending on IsCompany bit

    [Required]
    [EmailAddress]
    [Column(TypeName = "varchar(200)")]
    public string EmailAddress { get; set; } = null!;

    [Required]
    public bool IsCompany { get; set; }



    public ICollection<CustomerPhoneNumberEntity> PhoneNumbers { get; set; } = [];

    public ICollection<ProjectEntity> Projects { get; set; } = [];
}
