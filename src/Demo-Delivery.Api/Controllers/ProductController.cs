using Demo_Delivery.Api.Extensions;
using Demo_Delivery.Application.Product.Queries.GetAllProductsByFilter;
using Demo_Delivery.Application.Product.Queries.GetProductById;
using Demo_Delivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Delivery.Api.Controllers;

[Authorize(Policy = GlobalConstants.Policies.AuthenticatedUserPolicy)]
public class ProductController : BaseController
{
    public ProductController(ILogger<BaseController> logger) : base(logger)
    {
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] GetAllProductsByFilterQuery baseQuery)
    {
        var list = await Mediator.Send(baseQuery);
        HttpContext.Response.AddPagination(list.CurrentPage, list.TotalCount, list.TotalPages, list.HasNextPage,
            list.HasPreviousPage);
        return Ok(list);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await Mediator.Send(new GetProductByIdQuery(id)));
    }
}