using BreweryManagementSystemWebAPI.DTOs;
using BreweryManagementSystemWebAPI.Models;

namespace BreweryManagementSystemWebAPI.Repositories
{
    public interface IWholesalerStockRepository
    {
        Task<IEnumerable<WholesalerStock>> AddSaleToWholesalerAsync(int? fk_wh_Id, string? wh_Name, int? fk_be_Id, string? be_Name, int? ws_Quantity);
        Task<IEnumerable<WholesalerStock>> UpdateWholesalerStockAsync(int? fk_wh_Id, string? wh_Name, int? fk_be_Id, string? be_Name, int? ws_Quantity);
        Task<IEnumerable<Quote>> GenerateQuoteAsync(int? fk_wh_Id, string? wh_Name, IEnumerable<BeerOrderDto> beerOrderList);
    }

}
