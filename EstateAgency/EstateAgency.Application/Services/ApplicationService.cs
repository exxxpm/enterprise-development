using AutoMapper;
using EstateAgency.Application.Contracts.Application;
using EstateAgency.Domain;
using EstateAgency.Domain.Enums;

namespace EstateAgency.Application.Services;

/// <summary>
/// Service for managing applications, extending the generic CRUD service with validation for enum fields
/// and ensuring related counterparty and property entities exist and are loaded.
/// </summary>
/// <param name="repository">The repository used for application CRUD operations.</param>
/// <param name="mapper">The AutoMapper instance used for mapping between entities and DTOs.</param>
public class ApplicationService(
    IRepository<Domain.Entitites.Application> repository,
    IMapper mapper
) : CrudService<Domain.Entitites.Application, ApplicationGetDto, ApplicationCreateEditDto>(repository, mapper)
{
    /// <summary>
    /// Repository for accessing Application entities.
    /// </summary>
    private readonly IRepository<Domain.Entitites.Application> _repository = repository;

    /// <summary>
    /// AutoMapper instance for mapping between entities and DTOs.
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Retrieves all applications, including their related counterparty and property data.
    /// </summary>
    /// <returns>A collection of ApplicationGetDto objects with related entities populated.</returns>
    public override async Task<IEnumerable<ApplicationGetDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ApplicationGetDto>>(entities);
    }

    /// <summary>
    /// Retrieves a single application by ID, including its related counterparty and property data.
    /// </summary>
    /// <param name="id">The application ID.</param>
    /// <returns>The ApplicationGetDto with related entities populated, or null if not found.</returns>
    public override async Task<ApplicationGetDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return _mapper.Map<ApplicationGetDto>(entity);
    }

    /// <summary>
    /// Creates a new application after validating the Type and existence of related entities.
    /// </summary>
    /// <param name="dto">The DTO containing application data.</param>
    /// <returns>The created ApplicationGetDto with related entities populated.</returns>
    /// <exception cref="ArgumentException">Thrown if Type is invalid.</exception>
    public override async Task<ApplicationGetDto> CreateAsync(ApplicationCreateEditDto dto)
    {
        if (!Enum.TryParse<ApplicationType>(dto.Type, true, out var parsedType))
        {
            var allowedTypes = string.Join(", ", Enum.GetNames(typeof(ApplicationType)));
            throw new ArgumentException($"Invalid ApplicationType '{dto.Type}'. Allowed values: {allowedTypes}");
        }

        var entity = _mapper.Map<Domain.Entitites.Application>(dto);
        entity.Type = parsedType;

        var created = await _repository.AddAsync(entity);

        return _mapper.Map<ApplicationGetDto>(created);
    }

    /// <summary>
    /// Updates an existing application after validating the Type and existence of related entities.
    /// </summary>
    /// <param name="id">The ID of the application to update.</param>
    /// <param name="dto">The DTO containing updated application data.</param>
    /// <returns>The updated ApplicationGetDto with related entities populated.</returns>
    /// <exception cref="ArgumentException">Thrown if Type is invalid.</exception>
    public override async Task<ApplicationGetDto> UpdateAsync(int id, ApplicationCreateEditDto dto)
    {
        if (!Enum.TryParse<ApplicationType>(dto.Type, true, out var parsedType))
        {
            var allowedTypes = string.Join(", ", Enum.GetNames(typeof(ApplicationType)));
            throw new ArgumentException($"Invalid ApplicationType '{dto.Type}'. Allowed values: {allowedTypes}");
        }

        var entity = _mapper.Map<Domain.Entitites.Application>(dto);
        entity.Type = parsedType;
        entity.Id = id;

        var updated = await _repository.UpdateAsync(entity);

        return _mapper.Map<ApplicationGetDto>(updated);
    }
}
