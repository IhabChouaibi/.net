using PaiementService.Models;

namespace PaiementService.Interfaces;

public interface IPaiementRepository : IGenericRepository<Paiement>
{
    Task<PagedResult<Paiement>> GetPagedAsync(string searchTerm, string sortBy, string sortDirection, int page, int pageSize);
}
