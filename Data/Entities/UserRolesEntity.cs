using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class UserRolesEntity
{
    [Key]
    public int UserId { get; set; }
    [Key]
    public int RoleId { get; set; }



    public UserEntity User { get; set; } = null!;
    public RolesEntity Role { get; set; } = null!;
}