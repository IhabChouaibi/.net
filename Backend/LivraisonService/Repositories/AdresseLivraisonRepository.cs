using LivraisonService.Data;
using LivraisonService.Interfaces;
using LivraisonService.Models;
using Microsoft.EntityFrameworkCore;

namespace LivraisonService.Repositories;

public class AdresseLivraisonRepository : GenericRepository<AdresseLivraison>, IAdresseLivraisonRepository
{
    public AdresseLivraisonRepository(LivraisonDbContext context)
        : base(context)
    {
    }

    public async Task<PagedResult<AdresseLivraison>> GetPagedAsync(string searchTerm, string sortBy, string sortDirection, int page, int pageSize)
    {
        var query = Context.AdressesLivraison.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x =>
                x.Adresse.Contains(searchTerm) ||
                x.Ville.Contains(searchTerm) ||
                x.CodePostal.Contains(searchTerm) ||
                x.ColisId.ToString().Contains(searchTerm));
        }

        query = (sortBy.ToLower(), sortDirection.ToLower()) switch
        {
            ("adresse", "desc") => query.OrderByDescending(x => x.Adresse),
            ("adresse", _) => query.OrderBy(x => x.Adresse),
            ("codepostal", "desc") => query.OrderByDescending(x => x.CodePostal),
            ("codepostal", _) => query.OrderBy(x => x.CodePostal),
            (_, "desc") => query.OrderByDescending(x => x.Ville),
            _ => query.OrderBy(x => x.Ville)
        };

        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<AdresseLivraison>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
