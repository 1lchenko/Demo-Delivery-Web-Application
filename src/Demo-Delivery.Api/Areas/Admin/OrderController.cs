using Demo_Delivery.Api.Extensions;
using Demo_Delivery.Application.Order.Commands.UpdateOrder;
using Demo_Delivery.Application.Order.Queries.GetAllOrder;
using Demo_Delivery.Application.Order.Queries.GetCustomerOrderDetailsById;
using Demo_Delivery.Application.Order.Queries.GetCustomerOrderForUpdateById;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Delivery.Api.Areas.Admin;

public class OrderController : BaseAdminController
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllOrderQuery query)
    {
        
        var orders = await Mediator.Send(query);
        HttpContext.Response.AddPagination(orders.CurrentPage, orders.TotalCount, orders.TotalPages, orders.HasNextPage,
            orders.HasPreviousPage);
        return Ok(orders);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetCustomerOrderDetailsById(int orderId)
    {
         
        return Ok(await Mediator.Send(new GetCustomerOrderDetailsByIdQuery(orderId)));
    }
    
    [HttpGet("GetCustomerOrderForUpdateById/{orderId}")]
    public async Task<IActionResult> GetCustomerOrderForUpdateById(int orderId)
    {
        return Ok(await Mediator.Send(new GetCustomerOrderForUpdateByIdQuery(orderId)));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOrder(UpdateOrderCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
}