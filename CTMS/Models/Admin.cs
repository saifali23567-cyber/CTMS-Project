namespace ProjectAPI.Models;
public class Admin
{
    public int AdminId { get; set; }

    public int ContactId { get; set; }  // ← lowercase d

    public string PasswordHash { get; set; } = null!;

    public virtual Contact Contact { get; set; } = null!;
}