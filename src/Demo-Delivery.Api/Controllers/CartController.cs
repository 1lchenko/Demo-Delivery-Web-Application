using Demo_Delivery.Application.Cart.Commands.AddProductToCart;
using Demo_Delivery.Application.Cart.Commands.ChangeCartQuantity;
using Demo_Delivery.Application.Cart.Commands.ClearCart;
using Demo_Delivery.Application.Cart.Queries.GetCartCount;
using Demo_Delivery.Application.Cart.Queries.GetCartItemInfo;
using Demo_Delivery.Application.Cart.Queries.GetCustomerCartQuery;
using Demo_Delivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Delivery.Api.Controllers;

[Authorize(Policy = GlobalConstants.Policies.AuthenticatedUserPolicy)]
public class CartController : BaseController
{
    public CartController(ILogger<BaseController> logger) : base(logger)
    {
    }

    [HttpGet]
    [Route(nameof(GetCartCount))]
    public async Task<IActionResult> GetCartCount()
    {
        var r = HttpContext;
        return Ok(await Mediator.Send(new GetCartCountQuery()));
    }

    [HttpGet]
    [Route(nameof(GetCustomerCart))]
    public async Task<IActionResult> GetCustomerCart()
    {
        var r = HttpContext;
        return Ok(await Mediator.Send(new GetCustomerCartQuery()));
    }

    [HttpGet]
    [Route(nameof(GetCartItemQuantity))]
    public async Task<IActionResult> GetCartItemQuantity([FromQuery] GetCartItemQuantityQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut]
    [Route(nameof(ChangeCartQuantity))]
    public async Task<IActionResult> ChangeCartQuantity([FromBody] ChangeCartQuantityCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPut]
    [Route(nameof(AddProductToCart))]
    public async Task<IActionResult> AddProductToCart(AddProductToCartCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPut]
    [Route(nameof(RemoveProductFromCart))]
    public async Task<IActionResult> RemoveProductFromCart(AddProductToCartCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpDelete(Name = nameof(ClearCart))]
    public async Task<IActionResult> ClearCart()
    {
        await Mediator.Send(new ClearCartCommand());
        return Ok();
    }
}