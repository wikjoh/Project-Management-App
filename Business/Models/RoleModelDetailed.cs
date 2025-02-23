namespace Business.Models;

public class RoleModelDetailed
{
    public int Id { get; set; }

    public string Role { get; set; } = null!;

    public List<UserModel>? Users { get; set; } = null;
}
