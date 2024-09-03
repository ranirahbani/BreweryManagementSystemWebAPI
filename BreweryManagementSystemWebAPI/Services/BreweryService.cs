// Services/BreweryService.cs
using BreweryManagementSystemWebAPI.DTOs;
using BreweryManagementSystemWebAPI.Models;
using BreweryManagementSystemWebAPI.Repositories;
using BreweryManagementSystemWebAPI.Services;

public class BreweryService : IBreweryService
{
    //private readonly IBreweryRepository _breweryRepository;

    //public BreweryService(IBreweryRepository breweryRepository)
    //{
    //    _breweryRepository = breweryRepository;
    //}

    //public async Task<IEnumerable<BreweryDto>> GetAllBreweriesAsync()
    //{
    //    var breweries = await _breweryRepository.GetAllAsync();
    //    return breweries.Select(b => new BreweryDto { br_Id = b.br_Id, br_Name = b.br_Name });
    //}

    //public async Task<BreweryDto> GetBreweryByIdAsync(int id)
    //{
    //    var brewery = await _breweryRepository.GetByIdAsync(id);
    //    if (brewery == null)
    //        return null;

    //    return new BreweryDto { br_Id = brewery.br_Id, br_Name = brewery.br_Name };
    //}

    //public async Task AddBreweryAsync(BreweryDto breweryDto)
    //{
    //    var brewery = new Brewery
    //    {
    //        br_Name = breweryDto.br_Name
    //    };

    //    await _breweryRepository.AddAsync(brewery);
    //}

    //public async Task UpdateBreweryAsync(BreweryDto breweryDto)
    //{
    //    var brewery = new Brewery
    //    {
    //        br_Id = breweryDto.br_Id,
    //        br_Name = breweryDto.br_Name
    //    };

    //    await _breweryRepository.UpdateAsync(brewery);
    //}

    //public async Task DeleteBreweryAsync(int id)
    //{
    //    await _breweryRepository.DeleteAsync(id);
    //}
}

