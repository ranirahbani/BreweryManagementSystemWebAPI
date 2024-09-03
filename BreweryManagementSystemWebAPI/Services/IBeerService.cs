using BreweryManagementSystemWebAPI.DTOs;

namespace BreweryManagementSystemWebAPI.Services
{
    public interface IBeerService
    {
        Task<IEnumerable<BeerDto>> ListBeersByBreweryAsync(int? breweryId, string? breweryName);
        Task<IEnumerable<BeerDto>> AddBeerAsync(string? name, decimal? alcoholContent, decimal? price, int? breweryId, string? breweryName);
        Task<IEnumerable<BeerDto>> DeleteBeerAsync(int? beerId, string? beerName);
    }


}
