using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectAPI.Models
{
    public class Package
    {
        public int PackageID { get; set; }

        // Foreign keys (matching your DB columns exactly)
        public int CustomerId { get; set; }
        public int? CourierId { get; set; }

        public string TrackingNumber { get; set; } = null!;

        public double Cost { get; set; }
        public double RateperKG { get; set; } = 200.0; // Default rate, or remove if using CostService
        public double Weight { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string? DeliveryAddress { get; set; }
        
        // === Navigation Properties (THIS IS WHAT WAS MISSING) ===
        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; } = null!;

        [ForeignKey(nameof(CourierId))]
        public virtual Courier? Courier { get; set; }  // Nullable because CourierId is nullable
    }
}