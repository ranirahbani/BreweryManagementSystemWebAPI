// Services/WholesalerService.cs
using BreweryManagementSystemWebAPI.DTOs;
using BreweryManagementSystemWebAPI.Models;
using BreweryManagementSystemWebAPI.Repositories;

public class WholesalerService : IWholesalerService
{
    //private readonly IWholesalerRepository _wholesalerRepository;

    //public WholesalerService(IWholesalerRepository wholesalerRepository)
    //{
    //    _wholesalerRepository = wholesalerRepository;
    //}

    //public async Task<IEnumerable<WholesalerDto>> GetAllWholesalersAsync()
    //{
    //    var wholesalers = await _wholesalerRepository.GetAllAsync();
    //    return wholesalers.Select(w => new WholesalerDto { wh_Id = w.wh_Id, wh_Name = w.wh_Name });
    //}

    //public async Task<WholesalerDto> GetWholesalerByIdAsync(int id)
    //{
    //    var wholesaler = await _wholesalerRepository.GetByIdAsync(id);
    //    if (wholesaler == null)
    //        return null;

    //    return new WholesalerDto { wh_Id = wholesaler.wh_Id, wh_Name = wholesaler.wh_Name };
    //}

    //public async Task AddWholesalerAsync(WholesalerDto wholesalerDto)
    //{
    //    var wholesaler = new Wholesaler
    //    {
    //        wh_Name = wholesalerDto.wh_Name
    //    };

    //    await _wholesalerRepository.AddAsync(wholesaler);
    //}

    //public async Task UpdateWholesalerAsync(WholesalerDto wholesalerDto)
    //{
    //    var wholesaler = new Wholesaler
    //    {
    //        wh_Id = wholesalerDto.wh_Id,
    //        wh_Name = wholesalerDto.wh_Name
    //    };

    //    await _wholesalerRepository.UpdateAsync(wholesaler);
    //}

    //public async Task DeleteWholesalerAsync(int id)
    //{
    //    await _wholesalerRepository.DeleteAsync(id);
    //}
}
