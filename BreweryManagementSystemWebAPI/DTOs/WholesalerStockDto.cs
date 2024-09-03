namespace BreweryManagementSystemWebAPI.DTOs
{
    public class WholesalerStockDto
    {
        public int? wh_Id { get; set; }
        public string? wh_Name { get; set; }
        public int? be_Id { get; set; }
        public string? be_Name { get; set; }
        public int? ws_Quantity { get; set; }
    }
}
