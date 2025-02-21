namespace Business.Models;

public class ServiceModelDetailed
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int UnitId { get; set; }

    public string? Unit { get; init; } = null;
}
