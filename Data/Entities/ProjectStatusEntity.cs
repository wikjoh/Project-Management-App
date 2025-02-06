using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ProjectStatusEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;



    public ICollection<ProjectEntity> Projects { get; set; } = [];
}
