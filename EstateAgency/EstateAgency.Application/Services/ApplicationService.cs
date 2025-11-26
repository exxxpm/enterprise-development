using AutoMapper;
using EstateAgency.Application.Contracts.Application;
using EstateAgency.Domain;
using EstateAgency.Domain.Entitites;
using EstateAgency.Domain.Enums;

namespace EstateAgency.Application.Services;

/// <summary>
/// Service for managing applications, extending the generic CRUD service with validation for enum fields
/// and ensuring related counterparty and property entities exist and are loaded.
/// </summary>
/// <param name="repository">The repository used for application CRUD operations.</param>
/// <param name="counterpartyRepo">The repository used for accessing counterparties.</param>
/// <param name="propertyRepo">The repository used for accessing properties.</param>
/// <param name="mapper">The AutoMapper instance used for mapping between entities and DTOs.</param>
public class ApplicationService(
    IRepository<Domain.Entitites.Application> repository,
    IRepository<Counterparty> counterpartyRepo,
    IRepository<Property> propertyRepo,
    IMapper mapper
) : CrudService<Domain.Entitites.Application, ApplicationGetDto, ApplicationCreateEditDto>(repository, mapper)
{
    /// <summary>
    /// Retrieves all applications, including their related counterparty and property data.
    /// </summary>
    /// <returns>A collection of ApplicationGetDto objects with related entities populated.</returns>
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

    /// <summary>
    /// Retrieves a single application by ID, including its related counterparty and property data.
    /// </summary>
    /// <param name="id">The application ID.</param>
    /// <returns>The ApplicationGetDto with related entities populated, or null if not found.</returns>
    public override async Task<ApplicationGetDto?> GetByIdAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null)
            return null;

        entity.Counterparty = await counterpartyRepo.GetByIdAsync(entity.CounterpartyId);
        entity.Property = await propertyRepo.GetByIdAsync(entity.PropertyId);

        return mapper.Map<ApplicationGetDto>(entity);
    }

    /// <summary>
    /// Creates a new application after validating the Type and existence of related entities.
    /// </summary>
    /// <param name="dto">The DTO containing application data.</param>
    /// <returns>The created ApplicationGetDto with related entities populated.</returns>
    /// <exception cref="ArgumentException">Thrown if Type is invalid.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the related Counterparty or Property does not exist.</exception>
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

    /// <summary>
    /// Updates an existing application after validating the Type and existence of related entities.
    /// </summary>
    /// <param name="id">The ID of the application to update.</param>
    /// <param name="dto">The DTO containing updated application data.</param>
    /// <returns>The updated ApplicationGetDto with related entities populated.</returns>
    /// <exception cref="ArgumentException">Thrown if Type is invalid.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the related Counterparty or Property does not exist.</exception>
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

        var updated = await repository.UpdateAsync(entity);
        updated.Counterparty = await counterpartyRepo.GetByIdAsync(dto.CounterpartyId);
        updated.Property = await propertyRepo.GetByIdAsync(dto.PropertyId);

        return mapper.Map<ApplicationGetDto>(updated);
    }
}
