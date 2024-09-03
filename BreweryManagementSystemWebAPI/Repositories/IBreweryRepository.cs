using BreweryManagementSystemWebAPI.Models;

namespace BreweryManagementSystemWebAPI.Repositories
{
    public interface IBreweryRepository
    {
        Task<IEnumerable<Brewery>> GetAllAsync();
        Task<Brewery> GetByIdAsync(int id);
        Task<int> AddAsync(Brewery brewery);
        Task UpdateAsync(Brewery brewery);
        Task DeleteAsync(int id);
    }
}
