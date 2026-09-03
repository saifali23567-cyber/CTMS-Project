using System;

namespace ProjectAPI.Services
{
    public static class TrackingService
    {
        /// <summary>
        /// Generates a unique tracking number like: TRK-A1B2C3D4
        /// Uses first 8 characters of a new GUID (without hyphens), uppercase
        /// </summary>
        /// <returns>Tracking number string</returns>
        public static string GenerateTrackingNumber()
        {
            // Generate new GUID, remove hyphens, take first 8 chars, uppercase
            string guidPart = Guid.NewGuid()
                .ToString("N")     // "N" format = no hyphens
                .Substring(0, 8)   // First 8 characters
                .ToUpper();

            return "TRK-" + guidPart;
        }
    }
}