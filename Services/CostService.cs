namespace ProjectAPI.Services
{
    public static class CostService
    {
        // You can change this value anytime (e.g., via config later)
        public static double RatePerKg { get; set; } = 200.0;

        /// <summary>
        /// Calculates the total cost based on package weight
        /// </summary>
        /// <param name="weight">Weight in KG</param>
        /// <returns>Total cost in currency</returns>
        public static double CalculateCost(double weight)
        {
            if (weight <= 0)
                return 0;

            return weight * RatePerKg;
        }
    }
}