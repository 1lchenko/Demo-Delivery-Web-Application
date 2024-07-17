 
 
using Demo_Delivery.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Delivery.Api.Areas.Admin;

 
[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Policy = GlobalConstants.Policies.AdminRolePolicy)]
public class BaseAdminController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;
}