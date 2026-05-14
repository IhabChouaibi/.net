using ClientService.Models;

namespace ClientService.Interfaces;

public interface IClientRepository : IGenericRepository<Client>
{
    Task<PagedResult<Client>> GetPagedAsync(string searchTerm, string sortBy, string sortDirection, int page, int pageSize);
    Task<Client?> GetByCompteIdAsync(int compteId);
}
