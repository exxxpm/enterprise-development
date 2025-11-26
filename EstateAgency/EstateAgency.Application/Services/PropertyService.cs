using AutoMapper;
using EstateAgency.Application.Contracts.Property;
using EstateAgency.Domain.Entitites;
using EstateAgency.Domain;
using EstateAgency.Domain.Enums;

namespace EstateAgency.Application.Services;

/// <summary>
/// Service for managing properties, extending the generic CRUD service with validation for enum fields.
/// </summary>
/// <param name="repository">The repository used for property CRUD operations.</param>
/// <param name="mapper">The AutoMapper instance used for mapping between entities and DTOs.</param>
public class PropertyService(
    IRepository<Property> repository,
    IMapper mapper) : CrudService<Property, PropertyGetDto, PropertyCreateEditDto>(repository, mapper)
{
    /// <summary>
    /// Creates a new property after validating the Type and Purpose fields against the corresponding enums.
    /// </summary>
    /// <param name="dto">The DTO containing property data.</param>
    /// <returns>The DTO of the created property.</returns>
    /// <exception cref="ArgumentException">Thrown if Type or Purpose is invalid.</exception>
    public override async Task<PropertyGetDto> CreateAsync(PropertyCreateEditDto dto)
    {
        if (!Enum.TryParse<PropertyType>(dto.Type, true, out var parsedType))
        {
            var allowedTypes = string.Join(", ", Enum.GetNames(typeof(PropertyType)));
            throw new ArgumentException($"Invalid PropertyType '{dto.Type}'. Allowed values: {allowedTypes}");
        }

        if (!Enum.TryParse<PropertyPurpose>(dto.Purpose, true, out var parsedPurpose))
        {
            var allowedPurposes = string.Join(", ", Enum.GetNames(typeof(PropertyPurpose)));
            throw new ArgumentException($"Invalid PropertyPurpose '{dto.Purpose}'. Allowed values: {allowedPurposes}");
        }

        var entityDto = dto with
        {
            Type = parsedType.ToString(),
            Purpose = parsedPurpose.ToString()
        };

        return await base.CreateAsync(entityDto);
    }

    /// <summary>
    /// Updates an existing property after validating the Type and Purpose fields against the corresponding enums.
    /// </summary>
    /// <param name="id">The property ID.</param>
    /// <param name="dto">The DTO containing updated property data.</param>
    /// <returns>The DTO of the updated property.</returns>
    /// <exception cref="ArgumentException">Thrown if Type or Purpose is invalid.</exception>
    public override async Task<PropertyGetDto> UpdateAsync(int id, PropertyCreateEditDto dto)
    {
        if (!Enum.TryParse<PropertyType>(dto.Type, true, out var parsedType))
        {
            var allowedTypes = string.Join(", ", Enum.GetNames(typeof(PropertyType)));
            throw new ArgumentException($"Invalid PropertyType '{dto.Type}'. Allowed values: {allowedTypes}");
        }

        if (!Enum.TryParse<PropertyPurpose>(dto.Purpose, true, out var parsedPurpose))
        {
            var allowedPurposes = string.Join(", ", Enum.GetNames(typeof(PropertyPurpose)));
            throw new ArgumentException($"Invalid PropertyPurpose '{dto.Purpose}'. Allowed values: {allowedPurposes}");
        }

        var entityDto = dto with
        {
            Type = parsedType.ToString(),
            Purpose = parsedPurpose.ToString()
        };

        return await base.UpdateAsync(id, entityDto);
    }
}
