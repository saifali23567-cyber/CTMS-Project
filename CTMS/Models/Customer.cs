namespace ProjectAPI.Models;
public class Customer
{
    public int CustomerId { get; set; }

    public int ContactId { get; set; }  // ← lowercase d

    public string PasswordHash { get; set; } = null!;

    public virtual Contact Contact { get; set; } = null!;
}