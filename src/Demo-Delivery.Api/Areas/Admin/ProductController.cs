using Demo_Delivery.Api.Extensions;
using Demo_Delivery.Application.Product.Commands.CreateProduct;
using Demo_Delivery.Application.Product.Commands.DeleteProduct;
using Demo_Delivery.Application.Product.Commands.UpdateProduct;
using Demo_Delivery.Application.Product.Queries.GetAdminDetailedProductById;
using Demo_Delivery.Application.Product.Queries.GetAllAdminProducts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Delivery.Api.Areas.Admin;

public class ProductController : BaseAdminController
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllProductsForAdminQuery baseQuery)
    {
        var list = await Mediator.Send(baseQuery);
        HttpContext.Response.AddPagination(list.CurrentPage, list.TotalCount, list.TotalPages, list.HasNextPage,
            list.HasPreviousPage);
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromQuery] int id) 
        => Ok(await Mediator.Send(new GetAdminDetailedProductByIdQuery(id)));
    
     
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateProductCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdateProductCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteProductCommand(id));
        return NoContent();
    }
}