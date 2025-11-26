using AutoMapper;
using EstateAgency.Application.Contracts.Interfaces;
using EstateAgency.Domain;

namespace EstateAgency.Application.Services;

public class CrudService<TEntity, TEntityGetDto, TEntityCreateEditDto>(
    IRepository<TEntity> repository, 
    IMapper mapper) : ICrudService<TEntityGetDto, TEntityCreateEditDto>
    where TEntity : class
{
    public virtual async Task<IEnumerable<TEntityGetDto>> GetAllAsync()
    {
        var entities = await repository.GetAllAsync();
        return mapper.Map<IEnumerable<TEntityGetDto>>(entities);
    }

    public virtual async Task<TEntityGetDto?> GetByIdAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null) return default;
        return mapper.Map<TEntityGetDto>(entity);
    }

    public virtual async Task<TEntityGetDto> CreateAsync(TEntityCreateEditDto dto)
    {
        var entity = mapper.Map<TEntity>(dto);
        await repository.AddAsync(entity);
        return mapper.Map<TEntityGetDto>(entity);
    }

    public virtual async Task<TEntityGetDto> UpdateAsync(int id, TEntityCreateEditDto dto)
    {
        var entity = await repository.GetByIdAsync(id) ?? 
            throw new KeyNotFoundException($"Entity with id {id} not found");

        mapper.Map(dto, entity);
        await repository.UpdateAsync(entity);

        return mapper.Map<TEntityGetDto>(entity);
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null) return false;

        return await repository.DeleteAsync(entity);
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await repository.ExistsAsync(id);
    }
}
