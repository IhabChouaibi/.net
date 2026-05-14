using LivreurService.Data;
using LivreurService.Interfaces;
using LivreurService.Models;
using Microsoft.EntityFrameworkCore;

namespace LivreurService.Repositories;

public class LivreurRepository : GenericRepository<Livreur>, ILivreurRepository
{
    public LivreurRepository(LivreurDbContext context)
        : base(context)
    {
    }

    public async Task<PagedResult<Livreur>> GetPagedAsync(string searchTerm, string sortBy, string sortDirection, int page, int pageSize)
    {
        var query = Context.Livreurs.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x =>
                x.CIN.Contains(searchTerm) ||
                x.RaisonSocial.Contains(searchTerm) ||
                x.Ville.Contains(searchTerm) ||
                x.CodePostal.Contains(searchTerm));
        }

        query = (sortBy.ToLower(), sortDirection.ToLower()) switch
        {
            ("ville", "desc") => query.OrderByDescending(x => x.Ville),
            ("ville", _) => query.OrderBy(x => x.Ville),
            ("cin", "desc") => query.OrderByDescending(x => x.CIN),
            ("cin", _) => query.OrderBy(x => x.CIN),
            (_, "desc") => query.OrderByDescending(x => x.RaisonSocial),
            _ => query.OrderBy(x => x.RaisonSocial)
        };

        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<Livreur>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
