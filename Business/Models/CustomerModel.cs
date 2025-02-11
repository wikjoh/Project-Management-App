namespace Business.Models;

public class CustomerModel
{
    public int Id { get; set; }

    public bool IsCompany { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? CompanyName { get; set; }

    public string DisplayName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;
}
