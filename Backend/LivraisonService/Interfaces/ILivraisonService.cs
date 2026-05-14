using LivraisonService.Helpers;
using LivraisonService.Models;

namespace LivraisonService.Interfaces;

public interface ILivraisonService
{
    Task<PagedResult<AdresseLivraison>> GetPagedAsync(SearchRequest request);
    Task<IEnumerable<AdresseLivraison>> GetAllAsync();
    Task<AdresseLivraison?> GetByIdAsync(int id);
    Task<AdresseLivraison> AddAsync(AdresseLivraison adresseLivraison);
    Task<bool> UpdateAsync(int id, AdresseLivraison adresseLivraison);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<StatutLivraison>> GetStatutsAsync();
    Task<StatutLivraison?> GetStatutByIdAsync(int id);
    Task<StatutLivraison> AddStatutAsync(StatutLivraison statutLivraison);
    Task<bool> UpdateStatutAsync(int id, StatutLivraison statutLivraison);
    Task<bool> DeleteStatutAsync(int id);
}
