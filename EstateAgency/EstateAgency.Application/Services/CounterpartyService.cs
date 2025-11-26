using AutoMapper;
using EstateAgency.Domain.Entitites;
using EstateAgency.Domain;
using EstateAgency.Application.Contracts.Counterparty;

namespace EstateAgency.Application.Services;

/// <summary>
/// Service for managing counterparties, extending the generic CRUD service.
/// </summary>
/// <param name="repository">The repository used for counterparty CRUD operations.</param>
/// <param name="mapper">The AutoMapper instance used for mapping between entities and DTOs.</param>
public class CounterpartyService(
    IRepository<Counterparty> repository,
    IMapper mapper) : CrudService<Counterparty, CounterpartyGetDto, CounterpartyCreateEditDto>(repository, mapper);
