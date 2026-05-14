using AuthService.Data;
using AuthService.Interfaces;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repositories;

public class CompteRepository : GenericRepository<Compte>, ICompteRepository
{
    public CompteRepository(AuthDbContext context)
        : base(context)
    {
    }

    public async Task<Compte?> GetByLoginAsync(string login) =>
        await Context.Comptes.FirstOrDefaultAsync(x => x.Login == login);

    public async Task<PagedResult<Compte>> GetPagedAsync(string searchTerm, string status, int page, int pageSize)
    {
        var query = Context.Comptes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x => x.Login.Contains(searchTerm) || x.Role.Contains(searchTerm));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(x => x.Role == status);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(x => x.Login)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Compte>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
