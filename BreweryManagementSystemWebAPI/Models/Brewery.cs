// Models/Brewery.cs
namespace BreweryManagementSystemWebAPI.Models
{
    public class Brewery
    {
        public int? br_Id { get; set; }
        public string? br_Name { get; set; }
        public ICollection<Beer> Beers { get; set; }
    }
}