using Microsoft.EntityFrameworkCore;
using VehiculeService.Data;
using VehiculeService.Interfaces;
using VehiculeService.Models;

namespace VehiculeService.Repositories;

public class VehiculeRepository : GenericRepository<Vehicule>, IVehiculeRepository
{
    public VehiculeRepository(VehiculeDbContext context)
        : base(context)
    {
    }

    public async Task<PagedResult<Vehicule>> GetPagedAsync(string searchTerm, string sortBy, string sortDirection, int page, int pageSize)
    {
        var query = Context.Vehicules.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x =>
                x.Marque.Contains(searchTerm) ||
                x.Couleur.Contains(searchTerm) ||
                x.Matricule.Contains(searchTerm) ||
                x.Type.Contains(searchTerm));
        }

        query = (sortBy.ToLower(), sortDirection.ToLower()) switch
        {
            ("type", "desc") => query.OrderByDescending(x => x.Type),
            ("type", _) => query.OrderBy(x => x.Type),
            ("matricule", "desc") => query.OrderByDescending(x => x.Matricule),
            ("matricule", _) => query.OrderBy(x => x.Matricule),
            (_, "desc") => query.OrderByDescending(x => x.Marque),
            _ => query.OrderBy(x => x.Marque)
        };

        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<Vehicule>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
