using BreweryManagementSystemWebAPI.Data;
using BreweryManagementSystemWebAPI.DTOs;
using BreweryManagementSystemWebAPI.Models;
using BreweryManagementSystemWebAPI.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

public class WholesalerStockRepository : IWholesalerStockRepository
{
    private readonly ApplicationDbContext _context;

    public WholesalerStockRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WholesalerStock>> AddSaleToWholesalerAsync(int? fk_wh_Id, string? wh_Name, int? fk_be_Id, string? be_Name, int? ws_Quantity)
    {
        try {
            var parameters = new[]
            {
                new SqlParameter("@fk_wh_Id", fk_wh_Id ?? (object)DBNull.Value),
                new SqlParameter("@wh_Name", wh_Name ?? (object)DBNull.Value),
                new SqlParameter("@fk_be_Id", fk_be_Id ?? (object)DBNull.Value),
                new SqlParameter("@be_Name", be_Name ?? (object)DBNull.Value),
                new SqlParameter("@ws_Quantity", ws_Quantity)
            };

            var result = await _context.WholesalerStocks
                .FromSqlRaw("EXEC spBE_AddSaleToWholesaler @fk_wh_Id, @wh_Name, @fk_be_Id, @be_Name, @ws_Quantity", parameters)
                .ToListAsync();

            return result;
        }
        catch (SqlException ex)
        {
            // Log the error if needed
            // _logger.LogError(ex, "Error occurred while generating quote.");

            // Throw an exception with the Error message
            throw new Exception($"Error: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            throw new Exception($"An error occurred: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<WholesalerStock>> UpdateWholesalerStockAsync(int? fk_wh_Id, string? wh_Name, int? fk_be_Id, string? be_Name, int? ws_Quantity)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@fk_wh_Id", fk_wh_Id ?? (object)DBNull.Value),
                new SqlParameter("@wh_Name", wh_Name ?? (object)DBNull.Value),
                new SqlParameter("@fk_be_Id", fk_be_Id ?? (object)DBNull.Value),
                new SqlParameter("@be_Name", be_Name ?? (object)DBNull.Value),
                new SqlParameter("@ws_Quantity", ws_Quantity)
            };

            var result = await _context.WholesalerStocks
                .FromSqlRaw("EXEC spBE_UpdateWholesalerStock @fk_wh_Id, @wh_Name, @fk_be_Id, @be_Name, @ws_Quantity", parameters)
                .ToListAsync();

            return result;
        }
        catch (SqlException ex)
        {
            // Log the error if needed
            // _logger.LogError(ex, "Error occurred while generating quote.");

            // Throw an exception with the Error message
            throw new Exception($"Error: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            throw new Exception($"An error occurred: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Quote>> GenerateQuoteAsync(int? fk_wh_Id, string? wh_Name, IEnumerable<BeerOrderDto> beerOrderList)
    {
        try
        {
            var table = CreateBeerOrderTable(beerOrderList);

            var parameters = new[]
            {
                new SqlParameter("@fk_wh_Id", fk_wh_Id ?? (object)DBNull.Value),
                new SqlParameter("@wh_Name", wh_Name ?? (object)DBNull.Value),
                new SqlParameter("@tblBeerOrderListType", SqlDbType.Structured)
                {
                    TypeName = "dbo.BeerOrderListType", 
                    Value = table
                }
            };

            var result = await _context.Quotes
                .FromSqlRaw("EXEC spBE_GenerateQuote @fk_wh_Id, @wh_Name, @tblBeerOrderListType", parameters)
                .ToListAsync();

            return result;

        }
        catch (SqlException ex)
        {
            // Log the error if needed
            // _logger.LogError(ex, "Error occurred while generating quote.");

            // Throw an exception with the Error message
            throw new Exception($"Error: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            throw new Exception($"An error occurred: {ex.Message}", ex);
        }
    }

    private DataTable CreateBeerOrderTable(IEnumerable<BeerOrderDto> beerOrderList)
    {
        var table = new DataTable();
        table.Columns.Add("BeerId", typeof(int));
        table.Columns.Add("Quantity", typeof(int));

        foreach (var order in beerOrderList)
        {
            table.Rows.Add(order.BeerId, order.Quantity);
        }

        return table;
    }
}
