// Repositories/WholesalerRepository.cs
using BreweryManagementSystemWebAPI.Data;
using BreweryManagementSystemWebAPI.Models;
using BreweryManagementSystemWebAPI.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


public class WholesalerRepository : IWholesalerRepository
{
    private readonly ApplicationDbContext _context;

    public WholesalerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Wholesaler>> GetAllAsync()
    {
        return await _context.Wholesalers.FromSqlRaw("EXEC spBE_ListWholesalers").ToListAsync();
    }

    public async Task<Wholesaler> GetByIdAsync(int id)
    {
        var parameter = new SqlParameter("@wh_Id", id);
        return await _context.Wholesalers
            .FromSqlRaw("EXEC spBE_GetWholesalerById @wh_Id", parameter)
            .FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(Wholesaler wholesaler)
    {
        var parameter = new SqlParameter("@wh_Name", wholesaler.wh_Name);
        return await _context.Database.ExecuteSqlRawAsync("EXEC spBE_AddWholesaler @wh_Name", parameter);
    }

    public async Task UpdateAsync(Wholesaler wholesaler)
    {
        var parameters = new[]
        {
            new SqlParameter("@wh_Id", wholesaler.wh_Id),
            new SqlParameter("@wh_Name", wholesaler.wh_Name)
        };

        await _context.Database.ExecuteSqlRawAsync("EXEC spBE_UpdateWholesaler @wh_Id, @wh_Name", parameters);
    }

    public async Task DeleteAsync(int id)
    {
        var parameter = new SqlParameter("@wh_Id", id);
        await _context.Database.ExecuteSqlRawAsync("EXEC spBE_DeleteWholesaler @wh_Id", parameter);
    }
}
