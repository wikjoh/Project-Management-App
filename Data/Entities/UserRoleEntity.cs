using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class UserRoleEntity
{
    [Key]
    public int UserId { get; set; }
    [Key]
    public int RoleId { get; set; }



    public UserEntity User { get; set; } = null!;
    public RoleEntity Role { get; set; } = null!;
}