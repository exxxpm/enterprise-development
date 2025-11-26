using EstateAgency.Domain;
using EstateAgency.Infrastructrure.EfCore.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EstateAgency.Infrastructrure.EfCore.Repositories;

public class Repository<T>(EstateAgencyDbContext context) : IRepository<T> where T : class
{
    public async Task<IEnumerable<T>> GetAllAsync() => 
        await context.Set<T>().ToListAsync();

    public async Task<T?> GetByIdAsync(int id) => 
        await context.Set<T>().FindAsync(id);

    public async Task<T> AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        context.Set<T>().Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ExistsAsync(int id) => 
        await context.Set<T>().FindAsync(id) is not null;
}
