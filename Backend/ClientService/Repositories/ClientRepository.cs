using ClientService.Data;
using ClientService.Interfaces;
using ClientService.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientService.Repositories;

public class ClientRepository : GenericRepository<Client>, IClientRepository
{
    public ClientRepository(ClientDbContext context)
        : base(context)
    {
    }

    public async Task<PagedResult<Client>> GetPagedAsync(string searchTerm, string sortBy, string sortDirection, int page, int pageSize)
    {
        var query = Context.Clients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x =>
                x.Nom.Contains(searchTerm) ||
                x.Prenom.Contains(searchTerm) ||
                x.Email.Contains(searchTerm) ||
                x.Telephone.Contains(searchTerm) ||
                x.CIN.Contains(searchTerm) ||
                x.Adresse.Contains(searchTerm) ||
                x.Ville.Contains(searchTerm) ||
                x.CodePostal.Contains(searchTerm));
        }

        query = (sortBy.ToLower(), sortDirection.ToLower()) switch
        {
            ("ville", "desc") => query.OrderByDescending(x => x.Ville),
            ("ville", _) => query.OrderBy(x => x.Ville),
            ("prenom", "desc") => query.OrderByDescending(x => x.Prenom),
            ("prenom", _) => query.OrderBy(x => x.Prenom),
            ("codepostal", "desc") => query.OrderByDescending(x => x.CodePostal),
            ("codepostal", _) => query.OrderBy(x => x.CodePostal),
            (_, "desc") => query.OrderByDescending(x => x.Nom),
            _ => query.OrderBy(x => x.Nom)
        };

        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<Client>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<Client?> GetByCompteIdAsync(int compteId) =>
        await Context.Clients.FirstOrDefaultAsync(x => x.CompteId == compteId);
}
