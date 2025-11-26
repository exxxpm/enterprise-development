using EstateAgency.Application.Contracts.Counterparty;
using EstateAgency.Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

/// <summary>
/// Controller for managing counterparties.
/// Inherits generic CRUD operations from CrudControllerBase.
/// </summary>
[ApiController]
[Route("api/counterparties")]
public class CounterpartiesController(
    ICrudService<CounterpartyGetDto, CounterpartyCreateEditDto> service) 
    : CrudControllerBase<CounterpartyGetDto, CounterpartyCreateEditDto>(service);
