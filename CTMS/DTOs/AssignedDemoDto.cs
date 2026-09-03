namespace ProjectAPI.DTOs
{
    public class AssignedDemoDto
{
    public string TrackingNumber { get; set; } = string.Empty;
    public decimal Weight { get; set; }
    public string DeliveryAddress { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CustomerName { get; set; } = string.Empty;
}
}