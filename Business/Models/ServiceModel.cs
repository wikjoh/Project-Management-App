using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class ServiceModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int UnitId { get; set; }
}
