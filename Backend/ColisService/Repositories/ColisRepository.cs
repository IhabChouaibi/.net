using ColisService.Data;
using ColisService.Helpers;
using ColisService.Interfaces;
using ColisService.Models;
using Microsoft.EntityFrameworkCore;

namespace ColisService.Repositories;

public class ColisRepository : GenericRepository<Colis>, IColisRepository
{
    public ColisRepository(ColisDbContext context)
        : base(context)
    {
    }

    public async Task<PagedResult<Colis>> GetPagedAsync(string searchTerm, string status, string sortBy, string sortDirection, int page, int pageSize)
    {
        var query = Context.Colis.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x =>
                x.Libelle.Contains(searchTerm) ||
                x.ClientId.ToString().Contains(searchTerm) ||
                x.LivreurId.ToString().Contains(searchTerm) ||
                StatutLivraisonHelper.GetLabel(x.StatutLivraisonId).Contains(searchTerm));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            if (int.TryParse(status, out var statusId))
            {
                query = query.Where(x => x.StatutLivraisonId == statusId);
            }
            else
            {
                query = query.Where(x => StatutLivraisonHelper.GetLabel(x.StatutLivraisonId) == status);
            }
        }

        query = (sortBy.ToLower(), sortDirection.ToLower()) switch
        {
            ("montant", "asc") => query.OrderBy(x => x.Montant),
            ("montant", _) => query.OrderByDescending(x => x.Montant),
            ("poids", "asc") => query.OrderBy(x => x.Poids),
            ("poids", _) => query.OrderByDescending(x => x.Poids),
            ("libelle", "asc") => query.OrderBy(x => x.Libelle),
            ("libelle", _) => query.OrderByDescending(x => x.Libelle),
            (_, "asc") => query.OrderBy(x => x.DateLivraison),
            _ => query.OrderByDescending(x => x.DateLivraison)
        };

        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<Colis>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<IEnumerable<Colis>> GetByClientIdAsync(int clientId) =>
        await Context.Colis
            .Where(x => x.ClientId == clientId)
            .OrderByDescending(x => x.DateLivraison)
            .ToListAsync();
}
