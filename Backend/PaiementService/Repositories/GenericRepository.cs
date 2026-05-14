using Microsoft.EntityFrameworkCore;
using PaiementService.Data;
using PaiementService.Interfaces;

namespace PaiementService.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly PaiementDbContext Context;
    protected readonly DbSet<T> DbSet;

    public GenericRepository(PaiementDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await DbSet.ToListAsync();
    public async Task<T?> GetByIdAsync(int id) => await DbSet.FindAsync(id);
    public async Task AddAsync(T entity) => await DbSet.AddAsync(entity);
    public void Update(T entity) => DbSet.Update(entity);
    public void Delete(T entity) => DbSet.Remove(entity);
    public async Task SaveAsync() => await Context.SaveChangesAsync();
}
