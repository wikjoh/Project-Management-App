using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class ProjectStatusModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}
