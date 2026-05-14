using ColisService.Helpers;
using ColisService.Models;

namespace ColisService.Interfaces;

public interface IColisService
{
    Task<PagedResult<Colis>> GetPagedAsync(SearchRequest request);
    Task<IEnumerable<Colis>> GetAllAsync();
    Task<IEnumerable<Colis>> GetByClientIdAsync(int clientId);
    Task<Colis?> GetByIdAsync(int id);
    Task<Colis> AddAsync(Colis colis);
    Task<bool> UpdateAsync(int id, Colis colis);
    Task<bool> DeleteAsync(int id);
}
