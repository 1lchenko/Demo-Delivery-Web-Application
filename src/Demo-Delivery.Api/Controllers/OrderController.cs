using Demo_Delivery.Api.Extensions;
using Demo_Delivery.Application.Order.Commands.CreateOrder;
using Demo_Delivery.Application.Order.Commands.DeleteOrder;
using Demo_Delivery.Application.Order.Queries.GetAllOrder;
using Demo_Delivery.Application.Order.Queries.GetAllOrdersByCustomerId;
using Demo_Delivery.Application.Order.Queries.GetOrderById;
using Demo_Delivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Delivery.Api.Controllers;
 
[Authorize(Policy = GlobalConstants.Policies.AuthenticatedUserPolicy)]
public class OrderController : BaseController
{
    public OrderController(ILogger<BaseController> logger) : base(logger)
    {
    }

    [HttpGet(nameof(GetByCustomer))]
    public async Task<IActionResult> GetByCustomer([FromQuery] GetAllOrdersByCustomerIdQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetById(int orderId)
    {
        return Ok(await Mediator.Send(new GetOrderByIdQuery(orderId)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}