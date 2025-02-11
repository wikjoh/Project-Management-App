namespace Business.Models;

public class UserModel
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;
}
