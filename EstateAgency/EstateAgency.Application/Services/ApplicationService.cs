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
    public override async Task<ApplicationGetDto> CreateAsync(ApplicationCreateEditDto dto)
    {
        if (!Enum.IsDefined(typeof(ApplicationType), dto.Type))
        {
            var allowed = string.Join(", ", Enum.GetNames(typeof(ApplicationType)));
            throw new ArgumentException($"Invalid ApplicationType '{dto.Type}'. Allowed values: {allowed}");
        }

        if (!await counterpartyRepo.ExistsAsync(dto.CounterpartyId))
            throw new KeyNotFoundException($"Counterparty with Id {dto.CounterpartyId} does not exist.");

        if (!await propertyRepo.ExistsAsync(dto.PropertyId))
            throw new KeyNotFoundException($"Property with Id {dto.PropertyId} does not exist.");

        return await base.CreateAsync(dto);
    }

    public override async Task<ApplicationGetDto> UpdateAsync(int id, ApplicationCreateEditDto dto)
    {
        if (!Enum.IsDefined(typeof(ApplicationType), dto.Type))
        {
            var allowed = string.Join(", ", Enum.GetNames(typeof(ApplicationType)));
            throw new ArgumentException($"Invalid ApplicationType '{dto.Type}'. Allowed values: {allowed}");
        }

        if (!await counterpartyRepo.ExistsAsync(dto.CounterpartyId))
            throw new KeyNotFoundException($"Counterparty with Id {dto.CounterpartyId} does not exist.");

        if (!await propertyRepo.ExistsAsync(dto.PropertyId))
            throw new KeyNotFoundException($"Property with Id {dto.PropertyId} does not exist.");

        return await base.UpdateAsync(id, dto);
    }
}
