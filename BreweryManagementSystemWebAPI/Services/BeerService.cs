using BreweryManagementSystemWebAPI.DTOs;
using BreweryManagementSystemWebAPI.Models;
using BreweryManagementSystemWebAPI.Repositories;
using BreweryManagementSystemWebAPI.Services;

public class BeerService : IBeerService
{
    private readonly IBeerRepository _beerRepository;

    public BeerService(IBeerRepository beerRepository)
    {
        _beerRepository = beerRepository;
    }

    public async Task<IEnumerable<BeerDto>> ListBeersByBreweryAsync(int? breweryId, string? breweryName)
    {
        var beers = await _beerRepository.ListBeersByBreweryAsync(breweryId, breweryName);
        return beers.Select(b => new BeerDto
        {
            be_Id = b.be_Id,
            be_Name = b.be_Name,
            be_AlcoholContent = b.be_AlcoholContent,
            be_Price = b.be_Price,
            br_Id = b.br_Id,
            br_Name = b.br_Name,
        });
    }

    public async Task<IEnumerable<BeerDto>> AddBeerAsync(string? name, decimal? alcoholContent, decimal? price, int? breweryId, string? breweryName)
    {
        var beer = await _beerRepository.AddBeerAsync(name, alcoholContent, price, breweryId, breweryName);
        return beer.Select(b => new BeerDto
        {
            be_Id = b.be_Id,
            be_Name = b.be_Name,
            be_AlcoholContent = b.be_AlcoholContent,
            be_Price = b.be_Price,
            br_Id = b.br_Id,
            br_Name = b.br_Name,
        });
    }

    public async Task<IEnumerable<BeerDto>> DeleteBeerAsync(int? beerId, string? beerName)
    {
        var beer = await _beerRepository.DeleteBeerAsync(beerId, beerName);
        return beer.Select(b => new BeerDto
        {
            be_Id = b.be_Id,
            be_Name = b.be_Name,
            be_AlcoholContent = 0,
            be_Price = 0,
            br_Id = 0,
            br_Name = "",
        });
    }
}
