using ClientService.Helpers;
using ClientService.Models;

namespace ClientService.Interfaces;

public interface IClientService
{
    Task<PagedResult<Client>> GetPagedAsync(SearchRequest request);
    Task<IEnumerable<Client>> GetAllAsync();
    Task<Client?> GetByIdAsync(int id);
    Task<Client?> GetByCompteIdAsync(int compteId);
    Task<Client> RegisterAsync(Client client);
    Task<Client> AddAsync(Client client);
    Task<bool> UpdateAsync(int id, Client client);
    Task<bool> UpdateByCompteIdAsync(int compteId, Client client);
    Task<bool> DeleteAsync(int id);
}
