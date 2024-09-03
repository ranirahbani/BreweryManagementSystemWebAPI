namespace BreweryManagementSystemWebAPI.Models
{
    public class Beer
    {
        public int? be_Id { get; set; }
        public string? be_Name { get; set; }
        public decimal? be_AlcoholContent { get; set; }
        public decimal? be_Price { get; set; }
        public int? br_Id { get; set; }
        public string? br_Name { get; set; }

        public Brewery Brewery { get; set; }
        public ICollection<WholesalerStock> WholesalerStocks { get; set; }
    }
}
