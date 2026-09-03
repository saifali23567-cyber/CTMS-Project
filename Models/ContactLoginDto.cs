namespace ProjectAPI.Models
{
    public class ContactLoginDto
    {
        public int ContactId { get; set; }
        public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = null!;
    }
}