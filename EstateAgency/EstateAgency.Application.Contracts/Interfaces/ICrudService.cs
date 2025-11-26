namespace EstateAgency.Application.Contracts.Interfaces;

public interface ICrudService<TEntityGetDto, TEntityCreateEditDto>
{
    public Task<IEnumerable<TEntityGetDto>> GetAllAsync();

    public Task<TEntityGetDto?> GetByIdAsync(int id);

    public Task<TEntityGetDto> CreateAsync(TEntityCreateEditDto dto);

    public Task<TEntityGetDto> UpdateAsync(int id, TEntityCreateEditDto dto);

    public Task<bool> DeleteAsync(int id);
}
