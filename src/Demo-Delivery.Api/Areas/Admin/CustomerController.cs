using Demo_Delivery.Api.Extensions;
using Demo_Delivery.Application.Customers.Commands.UpdateCustomer;
using Demo_Delivery.Application.Customers.Queries.GetAllCustomers;
using Demo_Delivery.Application.Customers.Queries.GetCustomerById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Delivery.Api.Areas.Admin;

public class CustomerController : BaseAdminController
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllCustomersQuery query)
    {
        var customers = await Mediator.Send(query);
        HttpContext.Response.AddPagination(customers.TotalCount, customers.TotalCount, customers.TotalPages, customers.HasNextPage,
            customers.HasPreviousPage);
        return Ok(customers);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await Mediator.Send(new GetCustomerByIdQuery(id));
        return Ok(customer);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update(UpdateCustomerCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
}