using BreweryManagementSystemWebAPI.DTOs;
using BreweryManagementSystemWebAPI.Models;
using BreweryManagementSystemWebAPI.Repositories;
using BreweryManagementSystemWebAPI.Services;

public class WholesalerStockService : IWholesalerStockService
{
    private readonly IWholesalerStockRepository _wholesalerStockRepository;

    public WholesalerStockService(IWholesalerStockRepository WholesalerStockRepository)
    {
        _wholesalerStockRepository = WholesalerStockRepository;
    }

    public async Task<IEnumerable<WholesalerStockDto>> AddSaleToWholesalerAsync(int? fk_wh_Id, string? wh_Name, int? fk_be_Id, string? be_Name, int? ws_Quantity)
    {
        var sale = await _wholesalerStockRepository.AddSaleToWholesalerAsync(fk_wh_Id, wh_Name, fk_be_Id, be_Name, ws_Quantity);
        return sale.Select(s => new WholesalerStockDto
        {
            wh_Id = s.wh_Id,
            wh_Name = s.wh_Name,
            be_Id = s.be_Id,
            be_Name = s.be_Name,
            ws_Quantity = s.ws_Quantity
        });
    }

    public async Task<IEnumerable<WholesalerStockDto>> UpdateWholesalerStockAsync(int? fk_wh_Id, string? wh_Name, int? fk_be_Id, string? be_Name, int? ws_Quantity)
    {
        var sale = await _wholesalerStockRepository.UpdateWholesalerStockAsync(fk_wh_Id, wh_Name, fk_be_Id, be_Name, ws_Quantity);
        return sale.Select(s => new WholesalerStockDto
        {
            wh_Id = s.wh_Id,
            wh_Name = s.wh_Name,
            be_Id = s.be_Id,
            be_Name = s.be_Name,
            ws_Quantity = s.ws_Quantity
        });
    }

    public async Task<IEnumerable<QuoteDto>> GenerateQuoteAsync(int? fk_wh_Id, string? wh_Name, IEnumerable<BeerOrderDto> beerOrderList)
    {
        var quote = await _wholesalerStockRepository.GenerateQuoteAsync(fk_wh_Id, wh_Name, beerOrderList);
        return quote.Select(q => new QuoteDto
        {
            TotalQuantity = q.TotalQuantity,
            DiscountPercentage = q.DiscountPercentage,
            TotalAmount = q.TotalAmount
        });
    }
}
