namespace Business.Models;

public class UserModelDetailed
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public List<RoleModel> Roles { get; set; } = [];
    public List<ProjectModel> Projects { get; set; } = [];
}
