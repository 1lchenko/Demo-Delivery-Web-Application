using Demo_Delivery.Application.Category.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Delivery.Api.Controllers;

public class CategoryController : BaseController
{
    public CategoryController(ILogger<BaseController> logger) : base(logger)
    {
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] GetAllCategoriesQuery query)
    {
        var categoryViewModels = await Mediator.Send(query);
        return Ok(categoryViewModels);
    }
}