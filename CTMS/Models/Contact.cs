namespace ProjectAPI.Models;
public class Contact
{
    public int ContactId { get; set; }  // ← Must be ContactId (lowercase d)

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public long? Phone { get; set; }

    public string? Address { get; set; }
}