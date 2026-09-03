namespace ProjectAPI.DTOs
{
    public class AllPackagesDemoDto
    {
        public string TrackingNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;     // ← PascalCase
        public string CourierName { get; set; } = string.Empty;      // ← PascalCase
        public decimal Weight { get; set; }
        public string DeliveryAddress { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}