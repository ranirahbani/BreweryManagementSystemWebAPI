using Microsoft.AspNetCore.Mvc;
using BreweryManagementSystemWebAPI.Services;
using BreweryManagementSystemWebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using BreweryManagementSystemWebAPI.DTOs;

namespace BreweryManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WholesalerStocksController : ControllerBase
    {
        private readonly IWholesalerStockService _wholesalerStockService;

        public WholesalerStocksController(IWholesalerStockService wholesalerStockService)
        {
            _wholesalerStockService = wholesalerStockService;
        }

        // FR4 - Add sale of an existing beer to a wholesaler
        [HttpPost("AddSale")]
        //public async Task<IActionResult> AddSaleToWholesalerAsync(int? wholesalerId, string? wholesalerName, int? beerId, string? beerName, int quantity)
        public async Task<IActionResult> AddSaleToWholesalerAsync([FromBody] WholesalerStockDto? wholesalerStockDto)
        {

            try
            {
                if (wholesalerStockDto == null)
                {
                    return BadRequest("WholesalerStockDto cannot be null.");
                }

                var wss = new WholesalerStock
                {
                    wh_Id = wholesalerStockDto.wh_Id ?? 0,
                    wh_Name = wholesalerStockDto.wh_Name ?? "",
                    be_Id = wholesalerStockDto.be_Id ?? 0,
                    be_Name = wholesalerStockDto.be_Name ?? "",
                    ws_Quantity = wholesalerStockDto.ws_Quantity ?? 0,

                };

                var Wholesaler = await _wholesalerStockService.AddSaleToWholesalerAsync(wss.wh_Id, wss.wh_Name, wss.be_Id, wss.be_Name, wss.ws_Quantity);
                return Ok(Wholesaler);
            }
            catch (Exception ex)
            {
                // Return the detailed Error message in the response
                return BadRequest(ex.Message);
            }
        }

        // FR5 - Update wholesaler stock
        [HttpPost("UpdateStock")]
        //public async Task<IActionResult> UpdateWholesalerStockAsync(int? wholesalerId, string? wholesalerName, int? beerId, string? beerName, int quantity)
        public async Task<IActionResult> UpdateWholesalerStockAsync([FromBody] WholesalerStockDto? wholesalerStockDto)
        {

            try
            {
                if (wholesalerStockDto == null)
                {
                    return BadRequest("WholesalerStockDto cannot be null.");
                }

                var wss = new WholesalerStock
                {
                    wh_Id = wholesalerStockDto.wh_Id ?? 0,
                    wh_Name = wholesalerStockDto.wh_Name ?? "",
                    be_Id = wholesalerStockDto.be_Id ?? 0,
                    be_Name = wholesalerStockDto.be_Name ?? "",
                    ws_Quantity = wholesalerStockDto.ws_Quantity ?? 0,

                };

                var Wholesaler = await _wholesalerStockService.UpdateWholesalerStockAsync(wss.wh_Id, wss.wh_Name, wss.be_Id, wss.be_Name, wss.ws_Quantity);
                return Ok(Wholesaler);
            }
            catch (Exception ex)
            {
                // Return the detailed Error message in the response
                return BadRequest(ex.Message);
            }
        }

        // FR6 - Request a quote from a wholesaler
        [HttpPost("GenerateQuote")]
        //public async Task<ActionResult<QuoteDto>> GenerateQuoteAsync(int? wholesalerId, string? wholesalerName, [FromBody] IEnumerable<BeerOrderDto> beerOrderList)
        public async Task<ActionResult<QuoteDto>> GenerateQuoteAsync([FromBody] QuoteRequestDto quoteRequest)
        {
            try
            {
                var quoteResponse = await _wholesalerStockService.GenerateQuoteAsync(quoteRequest.WholesalerId ?? 0, quoteRequest.WholesalerName ?? "", quoteRequest.BeerOrderList);
                return Ok(quoteResponse);
                //if (quoteResponse != null)
                //{
                //    return Ok(quoteResponse);
                //}
                //return BadRequest("Failed to generate quote.");
            }
            catch (Exception ex)
            {
                // Return the detailed Error message in the response
                return BadRequest(ex.Message);
            }
        }
    }
}
