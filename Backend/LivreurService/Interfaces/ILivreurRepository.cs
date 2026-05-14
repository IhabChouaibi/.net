using LivreurService.Models;

namespace LivreurService.Interfaces;

public interface ILivreurRepository : IGenericRepository<Livreur>
{
    Task<PagedResult<Livreur>> GetPagedAsync(string searchTerm, string sortBy, string sortDirection, int page, int pageSize);
}
