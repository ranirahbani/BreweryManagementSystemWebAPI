namespace BreweryManagementSystemWebAPI.DTOs
{
    public class QuoteRequestDto
    {
        public int? WholesalerId { get; set; }
        public string? WholesalerName { get; set; }
        public IEnumerable<BeerOrderDto> BeerOrderList { get; set; }
    }
}
