using BreweryManagementSystemWebAPI.DTOs;

namespace BreweryManagementSystemWebAPI.Services
{
    public interface IWholesalerStockService
    {
        Task<IEnumerable<WholesalerStockDto>> AddSaleToWholesalerAsync(int? wholesalerId, string? wholesalerName, int? beerId, string? beerName, int? quantity);
        Task<IEnumerable<WholesalerStockDto>> UpdateWholesalerStockAsync(int? wholesalerId, string? wholesalerName, int? beerId, string? beerName, int? quantity);
        Task<IEnumerable<QuoteDto>> GenerateQuoteAsync(int? wholesalerId, string? wholesalerName, IEnumerable<BeerOrderDto> beerOrderList);
    }
}
