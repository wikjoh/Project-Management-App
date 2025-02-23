namespace Business.Models;

public class ServiceUnitModelDetailed
{
    public int Id { get; set; }

    public string Unit { get; set; } = null!;

    public List<ServiceModel>? Services { get; set; } = null;
}
