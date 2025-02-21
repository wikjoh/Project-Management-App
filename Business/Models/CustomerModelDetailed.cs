namespace Business.Models;

public class CustomerModelDetailed
{
    public int Id { get; set; }

    public bool IsCompany { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? CompanyName { get; set; }

    public string DisplayName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public List<CustomerPhoneNumberModel> PhoneNumbers { get; set; } = [];

    public List<ProjectModel> Projects { get; set; } = [];
}
