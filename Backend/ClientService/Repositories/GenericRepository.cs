using ClientService.Data;
using ClientService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClientService.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ClientDbContext Context;
    protected readonly DbSet<T> DbSet;

    public GenericRepository(ClientDbContext context)
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
