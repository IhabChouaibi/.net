using ColisService.Models;

namespace ColisService.Interfaces;

public interface IColisRepository : IGenericRepository<Colis>
{
    Task<PagedResult<Colis>> GetPagedAsync(string searchTerm, string status, string sortBy, string sortDirection, int page, int pageSize);
    Task<IEnumerable<Colis>> GetByClientIdAsync(int clientId);
}
