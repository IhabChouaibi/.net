using LivreurService.Helpers;
using LivreurService.Models;

namespace LivreurService.Interfaces;

public interface ILivreurService
{
    Task<PagedResult<Livreur>> GetPagedAsync(SearchRequest request);
    Task<IEnumerable<Livreur>> GetAllAsync();
    Task<Livreur?> GetByIdAsync(int id);
    Task<Livreur> AddAsync(Livreur livreur);
    Task<bool> UpdateAsync(int id, Livreur livreur);
    Task<bool> DeleteAsync(int id);
}
