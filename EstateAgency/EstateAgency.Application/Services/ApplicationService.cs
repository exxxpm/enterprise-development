using AutoMapper;
using EstateAgency.Application.Contracts.Application;
using EstateAgency.Domain;
using EstateAgency.Domain.Entitites;
using EstateAgency.Domain.Enums;

namespace EstateAgency.Application.Services;

public class ApplicationService(
    IRepository<Domain.Entitites.Application> repository,
    IRepository<Counterparty> counterpartyRepo,
    IRepository<Property> propertyRepo,
    IMapper mapper) : CrudService<Domain.Entitites.Application, ApplicationGetDto, ApplicationCreateEditDto>(repository, mapper)
{
    public override async Task<IEnumerable<ApplicationGetDto>> GetAllAsync()
    {
        var entities = await repository.GetAllAsync();

        foreach (var entity in entities)
        {
            entity.Counterparty = await counterpartyRepo.GetByIdAsync(entity.CounterpartyId);
            entity.Property = await propertyRepo.GetByIdAsync(entity.PropertyId);
        }

        return mapper.Map<IEnumerable<ApplicationGetDto>>(entities);
    }

    public override async Task<ApplicationGetDto?> GetByIdAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null)
            return null;

        entity.Counterparty = await counterpartyRepo.GetByIdAsync(entity.CounterpartyId);
        entity.Property = await propertyRepo.GetByIdAsync(entity.PropertyId);

        return mapper.Map<ApplicationGetDto>(entity);
    }

    public override async Task<ApplicationGetDto> CreateAsync(ApplicationCreateEditDto dto)
    {
        if (!Enum.TryParse<ApplicationType>(dto.Type, true, out var parsedType))
        {
            var allowedTypes = string.Join(", ", Enum.GetNames(typeof(ApplicationType)));
            throw new ArgumentException($"Invalid ApplicationType '{dto.Type}'. Allowed values: {allowedTypes}");
        }

        if (!await counterpartyRepo.ExistsAsync(dto.CounterpartyId))
            throw new KeyNotFoundException($"Counterparty with Id {dto.CounterpartyId} does not exist.");

        if (!await propertyRepo.ExistsAsync(dto.PropertyId))
            throw new KeyNotFoundException($"Property with Id {dto.PropertyId} does not exist.");

        var entity = mapper.Map<Domain.Entitites.Application>(dto);
        entity.Type = parsedType;

        var created = await repository.AddAsync(entity);
        created.Counterparty = await counterpartyRepo.GetByIdAsync(dto.CounterpartyId);
        created.Property = await propertyRepo.GetByIdAsync(dto.PropertyId);

        return mapper.Map<ApplicationGetDto>(created);
    }

    public override async Task<ApplicationGetDto> UpdateAsync(int id, ApplicationCreateEditDto dto)
    {
        if (!Enum.TryParse<ApplicationType>(dto.Type, true, out var parsedType))
        {
            var allowedTypes = string.Join(", ", Enum.GetNames(typeof(ApplicationType)));
            throw new ArgumentException($"Invalid ApplicationType '{dto.Type}'. Allowed values: {allowedTypes}");
        }

        if (!await counterpartyRepo.ExistsAsync(dto.CounterpartyId))
            throw new KeyNotFoundException($"Counterparty with Id {dto.CounterpartyId} does not exist.");

        if (!await propertyRepo.ExistsAsync(dto.PropertyId))
            throw new KeyNotFoundException($"Property with Id {dto.PropertyId} does not exist.");

        var entity = mapper.Map<Domain.Entitites.Application>(dto);
        entity.Type = parsedType;

        var created = await repository.UpdateAsync(entity);
        created.Counterparty = await counterpartyRepo.GetByIdAsync(dto.CounterpartyId);
        created.Property = await propertyRepo.GetByIdAsync(dto.PropertyId);

        return mapper.Map<ApplicationGetDto>(created);
    }
}
