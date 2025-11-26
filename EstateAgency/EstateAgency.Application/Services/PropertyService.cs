using AutoMapper;
using EstateAgency.Application.Contracts.Property;
using EstateAgency.Domain.Entitites;
using EstateAgency.Domain;
using EstateAgency.Domain.Enums;

namespace EstateAgency.Application.Services;

public class PropertyService(
    IRepository<Property> repository,
    IMapper mapper) : CrudService<Property, PropertyGetDto, PropertyCreateEditDto>(repository, mapper)
{
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