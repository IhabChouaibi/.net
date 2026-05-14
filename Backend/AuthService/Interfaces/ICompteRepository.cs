using AuthService.Models;

namespace AuthService.Interfaces;

public interface ICompteRepository : IGenericRepository<Compte>
{
    Task<Compte?> GetByLoginAsync(string login);
    Task<PagedResult<Compte>> GetPagedAsync(string searchTerm, string status, int page, int pageSize);
}
