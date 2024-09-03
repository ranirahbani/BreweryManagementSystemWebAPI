using BreweryManagementSystemWebAPI.Data;
using BreweryManagementSystemWebAPI.Models;
using BreweryManagementSystemWebAPI.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

public class BeerRepository : IBeerRepository
{
    private readonly ApplicationDbContext _context;

    public BeerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Beer>> ListBeersByBreweryAsync(int? breweryId, string breweryName)
    {
        try
        {
            var beers = await _context.Beers
                .FromSqlRaw("EXEC spBE_ListBeersByBrewery @fk_br_Id, @br_Name",
                            new SqlParameter("@fk_br_Id", breweryId ?? (object)DBNull.Value),
                            new SqlParameter("@br_Name", breweryName ?? (object)DBNull.Value))
                .ToListAsync();
            return beers;
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

    public async Task<IEnumerable<Beer>> AddBeerAsync(string? name, decimal? alcoholContent, decimal? price, int? breweryId, string? breweryName)
    {
        try
        {
            var beers = await _context.Beers
                .FromSqlRaw("EXEC spBE_AddBeer @be_Name, @be_AlcoholContent, @be_Price, @fk_br_Id, @br_Name",
                        new SqlParameter("@be_Name", name),
                        new SqlParameter("@be_AlcoholContent", alcoholContent),
                        new SqlParameter("@be_Price", price),
                        new SqlParameter("@fk_br_Id", breweryId ?? (object)DBNull.Value),
                        new SqlParameter("@br_Name", breweryName ?? (object)DBNull.Value))
                .ToListAsync();
            return beers;
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

    public async Task<IEnumerable<Beer>> DeleteBeerAsync(int? beerId, string? beerName)
    {
      try
        {
            var beers = await _context.Beers
               .FromSqlRaw("EXEC spBE_DeleteBeer @be_Id, @be_Name",
                new SqlParameter("@be_Id", beerId ?? (object)DBNull.Value),
                new SqlParameter("@be_Name", beerName ?? (object)DBNull.Value))
            .ToListAsync();
            return beers;
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
}
