namespace ProjectAPI.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int UserId { get; set; }      // ContactId
        public int RoleId { get; set; }      // CustomerID / CourierID / AdminID
    }
}