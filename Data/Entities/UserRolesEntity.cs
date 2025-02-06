using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class UserRolesEntity
{
    [Key]
    public int UserId { get; set; }

    [Required]
    public bool AdminRole { get; set; }

    [Required]
    public bool ProjectManagerRole { get; set; }


    public UserEntity User { get; set; } = null!;
    public ICollection<UserEntity> Users { get; set; } = [];
}
