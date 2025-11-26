using AutoMapper;
using EstateAgency.Application.Contracts.Interfaces;
using EstateAgency.Domain;

namespace EstateAgency.Application.Services;

/// <summary>
/// Generic CRUD service implementation for entities using DTOs and AutoMapper.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TEntityGetDto">The DTO type returned for retrieval operations.</typeparam>
/// <typeparam name="TEntityCreateEditDto">The DTO type used for create and update operations.</typeparam>
public class CrudService<TEntity, TEntityGetDto, TEntityCreateEditDto>(
    IRepository<TEntity> repository,
    IMapper mapper) : ICrudService<TEntityGetDto, TEntityCreateEditDto>
    where TEntity : class
{
    /// <summary>
    /// Retrieves all entities and maps them to the corresponding DTOs.
    /// </summary>
    /// <returns>A collection of DTOs representing all entities.</returns>
    public virtual async Task<IEnumerable<TEntityGetDto>> GetAllAsync()
    {
        var entities = await repository.GetAllAsync();
        return mapper.Map<IEnumerable<TEntityGetDto>>(entities);
    }

    /// <summary>
    /// Retrieves an entity by its ID and maps it to the corresponding DTO.
    /// </summary>
    /// <param name="id">The entity's identifier.</param>
    /// <returns>The DTO if found; otherwise, null.</returns>
    public virtual async Task<TEntityGetDto?> GetByIdAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null)
            return default;
        return mapper.Map<TEntityGetDto>(entity);
    }

    /// <summary>
    /// Creates a new entity from the provided DTO and returns the mapped DTO.
    /// </summary>
    /// <param name="dto">The DTO containing data for the new entity.</param>
    /// <returns>The DTO of the created entity.</returns>
    public virtual async Task<TEntityGetDto> CreateAsync(TEntityCreateEditDto dto)
    {
        var entity = mapper.Map<TEntity>(dto);
        await repository.AddAsync(entity);
        return mapper.Map<TEntityGetDto>(entity);
    }

    /// <summary>
    /// Updates an existing entity identified by its ID using the provided DTO.
    /// </summary>
    /// <param name="id">The entity's identifier.</param>
    /// <param name="dto">The DTO containing updated data.</param>
    /// <returns>The DTO of the updated entity.</returns>
    public virtual async Task<TEntityGetDto> UpdateAsync(int id, TEntityCreateEditDto dto)
    {
        var entity = await repository.GetByIdAsync(id) ??
            throw new KeyNotFoundException($"Entity with id {id} not found");

        mapper.Map(dto, entity);
        await repository.UpdateAsync(entity);

        return mapper.Map<TEntityGetDto>(entity);
    }

    /// <summary>
    /// Deletes an entity by its ID.
    /// </summary>
    /// <param name="id">The entity's identifier.</param>
    /// <returns>True if deletion was successful; otherwise, false.</returns>
    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null)
            return false;

        return await repository.DeleteAsync(entity);
    }

    /// <summary>
    /// Checks whether an entity with the specified ID exists.
    /// </summary>
    /// <param name="id">The entity's identifier.</param>
    /// <returns>True if the entity exists; otherwise, false.</returns>
    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await repository.ExistsAsync(id);
    }
}
