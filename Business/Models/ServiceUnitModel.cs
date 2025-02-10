using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class ServiceUnitModel
{
    public int Id { get; set; }

    public string Unit { get; set; } = null!;
}
