namespace Business.Models;

public class ProjectModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? StartDate { get; set; } = null;

    public DateTime? EndDate { get; set; } = null;

    public int ProjectManagerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public int CustomerId { get; set; }

    public int ServiceId { get; set; }

    public int? ServiceQuantity { get; set; } = null;

    public decimal? TotalPrice { get; set; } = null;

    public int StatusId { get; set; }
}
