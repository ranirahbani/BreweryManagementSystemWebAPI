using BreweryManagementSystemWebAPI.Models;

namespace BreweryManagementSystemWebAPI.Repositories
{
    public interface IBeerRepository
    {
        Task<IEnumerable<Beer>> ListBeersByBreweryAsync(int? breweryId, string? breweryName);
        Task<IEnumerable<Beer>> AddBeerAsync(string? name, decimal? alcoholContent, decimal? price, int? breweryId, string? breweryName);
        Task<IEnumerable<Beer>> DeleteBeerAsync(int? beerId, string? beerName);
    }
}
