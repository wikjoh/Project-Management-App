namespace Business.Models;

public class ProjectStatusModelDetailed
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public List<ProjectModel>? Projects { get; set; } = null;
}
