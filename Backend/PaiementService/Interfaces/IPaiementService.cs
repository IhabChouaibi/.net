using PaiementService.Helpers;
using PaiementService.Models;

namespace PaiementService.Interfaces;

public interface IPaiementService
{
    Task<PagedResult<Paiement>> GetPagedAsync(SearchRequest request);
    Task<IEnumerable<Paiement>> GetAllAsync();
    Task<Paiement?> GetByIdAsync(int id);
    Task<Paiement> AddAsync(Paiement paiement);
    Task<bool> UpdateAsync(int id, Paiement paiement);
    Task<bool> DeleteAsync(int id);
}
