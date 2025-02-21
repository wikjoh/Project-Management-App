namespace Business.Models;

public class ProjectModelDetailed
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


    public CustomerModel? Customer { get; set; } = null;
    public UserModel? ProjectManager { get; set; } = null;
    public ServiceModelDetailed? Service { get; set; } = null;
    public ProjectStatusModel? Status { get; set; } = null;
}
