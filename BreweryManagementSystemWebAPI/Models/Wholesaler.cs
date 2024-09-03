namespace BreweryManagementSystemWebAPI.Models
{
    public class Wholesaler
    {
        public int? wh_Id { get; set; }
        public string? wh_Name { get; set; }
        public ICollection<WholesalerStock> WholesalerStocks { get; set; }
    }
}
