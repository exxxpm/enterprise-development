namespace EstateAgency.Domain;

/// <summary>
/// Generic repository interface for basic CRUD operations on entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Retrieves all entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <returns>A collection of entities.</returns>
    public Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The entity's identifier.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    public Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>The added entity.</returns>
    public Task<T> AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>The updated entity.</returns>
    public Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>True if deletion was successful; otherwise, false.</returns>
    public Task<bool> DeleteAsync(T entity);

    /// <summary>
    /// Checks whether an entity with the specified identifier exists.
    /// </summary>
    /// <param name="id">The entity's identifier.</param>
    /// <returns>True if the entity exists; otherwise, false.</returns>
    public Task<bool> ExistsAsync(int id);
}
