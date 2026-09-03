namespace ProjectAPI.DTOs
{
    public class MyPackagesDemoDto
{
    public string TrackingNumber { get; set; } = string.Empty;
    public decimal Weight { get; set; }
    public string DeliveryAddress { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
}