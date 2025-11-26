using EstateAgency.Application.Contracts.Interfaces;
using EstateAgency.Application.Contracts.Property;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

[ApiController]
[Route("api/properties")]
public class PropertiesController(ICrudService<PropertyGetDto, PropertyCreateEditDto> service)
    : CrudControllerBase<PropertyGetDto, PropertyCreateEditDto>(service);
