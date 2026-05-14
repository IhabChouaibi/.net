using Microsoft.EntityFrameworkCore;
using PaiementService.Data;
using PaiementService.Interfaces;
using PaiementService.Models;

namespace PaiementService.Repositories;

public class PaiementRepository : GenericRepository<Paiement>, IPaiementRepository
{
    public PaiementRepository(PaiementDbContext context)
        : base(context)
    {
    }

    public async Task<PagedResult<Paiement>> GetPagedAsync(string searchTerm, string sortBy, string sortDirection, int page, int pageSize)
    {
        var query = Context.Paiements.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x =>
                x.ModePaiement.Contains(searchTerm) ||
                x.ColisId.ToString().Contains(searchTerm) ||
                x.Montant.ToString().Contains(searchTerm));
        }

        query = (sortBy.ToLower(), sortDirection.ToLower()) switch
        {
            ("montant", "desc") => query.OrderByDescending(x => x.Montant),
            ("montant", _) => query.OrderBy(x => x.Montant),
            ("modepaiement", "desc") => query.OrderByDescending(x => x.ModePaiement),
            ("modepaiement", _) => query.OrderBy(x => x.ModePaiement),
            (_, "asc") => query.OrderBy(x => x.DatePaiement),
            _ => query.OrderByDescending(x => x.DatePaiement)
        };

        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<Paiement>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
