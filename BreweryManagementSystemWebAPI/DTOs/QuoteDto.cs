namespace BreweryManagementSystemWebAPI.DTOs
{
    public class QuoteDto
    {
        public int TotalQuantity { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
