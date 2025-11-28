using EstateAgency.Domain;
using EstateAgency.Infrastructrure.EfCore.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EstateAgency.Infrastructrure.EfCore.Repositories;

/// <summary>
/// Generic repository implementation for CRUD operations on entities of type <typeparamref name="T"/> using EF Core.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
/// <param name="context">Database context options.</param>
public class Repository<T>(EstateAgencyDbContext context) : IRepository<T> where T : class
{
    /// <summary>
    /// Retrieves all entities of type <typeparamref name="T"/> from the database.
    /// </summary>
    /// <returns>A collection of all entities.</returns>
    public virtual async Task<IEnumerable<T>> GetAllAsync() =>
        await context.Set<T>().ToListAsync();

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The entity's identifier.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    public virtual async Task<T?> GetByIdAsync(int id) =>
        await context.Set<T>().FindAsync(id);

    /// <summary>
    /// Adds a new entity to the database.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>The added entity.</returns>
    public virtual async Task<T> AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Updates an existing entity in the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>The updated entity.</returns>
    public virtual async Task<T> UpdateAsync(T entity)
    {
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Deletes an entity from the database.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    public virtual async Task<bool> DeleteAsync(T entity)
    {
        context.Set<T>().Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// Checks if an entity with the specified identifier exists in the database.
    /// </summary>
    /// <param name="id">The entity's identifier.</param>
    /// <returns>True if the entity exists; otherwise, false.</returns>
    public virtual async Task<bool> ExistsAsync(int id) =>
        await context.Set<T>().FindAsync(id) is not null;
}
