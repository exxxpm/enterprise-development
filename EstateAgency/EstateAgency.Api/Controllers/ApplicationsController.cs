using EstateAgency.Application.Contracts.Application;
using EstateAgency.Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

[ApiController]
[Route("api/application")]
public class ApplicationsController(ICrudService<ApplicationGetDto, ApplicationCreateEditDto> service)
    : CrudControllerBase<ApplicationGetDto, ApplicationCreateEditDto>(service);
