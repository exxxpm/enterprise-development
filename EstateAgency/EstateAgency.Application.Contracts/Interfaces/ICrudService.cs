namespace EstateAgency.Application.Contracts.Interfaces;

/// <summary>
/// Generic service interface providing CRUD operations for entities using DTOs.
/// </summary>
/// <typeparam name="TEntityGetDto">The DTO type returned for retrieval operations.</typeparam>
/// <typeparam name="TEntityCreateEditDto">The DTO type used for create and update operations.</typeparam>
public interface ICrudService<TEntityGetDto, TEntityCreateEditDto>
{
    /// <summary>
    /// Retrieves all entities.
    /// </summary>
    /// <returns>A collection of DTOs representing all entities.</returns>
    public Task<IEnumerable<TEntityGetDto>> GetAllAsync();

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The entity's identifier.</param>
    /// <returns>The DTO of the entity if found; otherwise, null.</returns>
    public Task<TEntityGetDto?> GetByIdAsync(int id);

    /// <summary>
    /// Creates a new entity using the provided DTO.
    /// </summary>
    /// <param name="dto">The DTO containing data for the new entity.</param>
    /// <returns>The DTO of the created entity.</returns>
    public Task<TEntityGetDto> CreateAsync(TEntityCreateEditDto dto);

    /// <summary>
    /// Updates an existing entity identified by its ID with the provided DTO.
    /// </summary>
    /// <param name="id">The entity's identifier.</param>
    /// <param name="dto">The DTO containing updated data.</param>
    /// <returns>The DTO of the updated entity.</returns>
    public Task<TEntityGetDto> UpdateAsync(int id, TEntityCreateEditDto dto);

    /// <summary>
    /// Deletes an entity identified by its ID.
    /// </summary>
    /// <param name="id">The entity's identifier.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    public Task<bool> DeleteAsync(int id);
}
