using EstateAgency.Application.Contracts.Counterparty;
using EstateAgency.Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

[ApiController]
[Route("api/counterparties")]
public class CounterpartiesController(ICrudService<CounterpartyGetDto, CounterpartyCreateEditDto> service) 
    : CrudControllerBase<CounterpartyGetDto, CounterpartyCreateEditDto>(service);
