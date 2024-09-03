using BreweryManagementSystemWebAPI.Data;
using BreweryManagementSystemWebAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace BreweryManagementSystemWebAPI.Repositories
{
    public class BreweryRepository : IBreweryRepository
    {
        private readonly ApplicationDbContext _context;

        public BreweryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Brewery>> GetAllAsync()
        {
            return await _context.Breweries.FromSqlRaw("EXEC spBE_ListBreweries").ToListAsync();
        }

        public async Task<Brewery> GetByIdAsync(int id)
        {
            var parameter = new SqlParameter("@br_Id", id);
            return await _context.Breweries
                .FromSqlRaw("EXEC spBE_GetBreweryById @br_Id", parameter)
                .FirstOrDefaultAsync();
        }

        public async Task<int> AddAsync(Brewery brewery)
        {
            var parameter = new SqlParameter("@br_Name", brewery.br_Name);
            return await _context.Database.ExecuteSqlRawAsync("EXEC spBE_AddBrewery @br_Name", parameter);
        }

        public async Task UpdateAsync(Brewery brewery)
        {
            var parameters = new[]
            {
            new SqlParameter("@br_Id", brewery.br_Id),
            new SqlParameter("@br_Name", brewery.br_Name)
        };

            await _context.Database.ExecuteSqlRawAsync("EXEC spBE_UpdateBrewery @br_Id, @br_Name", parameters);
        }

        public async Task DeleteAsync(int id)
        {
            var parameter = new SqlParameter("@br_Id", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC spBE_DeleteBrewery @br_Id", parameter);
        }
    }
}
