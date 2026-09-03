namespace ProjectAPI.DTOs
{
    public class CreatePackageDto
    {
        public double Weight { get; set; }
        public string DeliveryAddress { get; set; } = string.Empty;
    }
}