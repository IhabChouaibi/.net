using LivraisonService.Models;

namespace LivraisonService.Interfaces;

public interface IAdresseLivraisonRepository : IGenericRepository<AdresseLivraison>
{
    Task<PagedResult<AdresseLivraison>> GetPagedAsync(string searchTerm, string sortBy, string sortDirection, int page, int pageSize);
}
