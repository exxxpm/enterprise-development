using AutoMapper;
using EstateAgency.Domain.Entitites;
using EstateAgency.Domain;
using EstateAgency.Application.Contracts.Counterparty;

namespace EstateAgency.Application.Services;

public class CounterpartyService(
    IRepository<Counterparty> repository,
    IMapper mapper
    ) : CrudService<Counterparty, CounterpartyGetDto, CounterpartyCreateEditDto>(repository, mapper);
