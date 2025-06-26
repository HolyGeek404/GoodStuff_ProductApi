using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Features.Product.Queries.GetAllProductsByType;
using Model.Features.Product.Queries.GetProductById;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController(IMediator mediator, ILogger<ProductController> logger) : Controller
{
    [HttpGet]
    [Authorize(Roles = "GetProducts")]
    [Route("GetAllProductsByType")]
    public async Task<IActionResult> GetAllProductsByType(string type)
    {
        logger.LogInformation($"Calling {nameof(this.GetAllProductsByType)} by {User.FindFirst("appid")?.Value ?? "Unknown"}. Type: {type}");

        if (string.IsNullOrEmpty(type))
            return BadRequest("Product type cannot be null or empty.");

        var products = await mediator.Send(new GetAllProductsByTypeQuery { Type = type.ToUpper() });
        if (products == null)
            return NotFound($"No products found for type: {type}");

        logger.LogInformation($"Successfully called {nameof(this.GetAllProductsByType)} by {User.FindFirst("appid")?.Value ?? "Unknown"}. Type: {type}");

        return new JsonResult(products);
    }

    [HttpGet]
    [Authorize(Roles = "GetSingleProduct")]
    [Route("GetProductById")]
    public async Task<IActionResult> GetProductById(string type, string id)
    {
        logger.LogInformation($"Calling {nameof(this.GetProductById)} by {User.FindFirst("appid")?.Value ?? "Unknown"}. Type: {type}, Id: {id}");

        if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(id))
            return BadRequest("Product type or id is invalid.");

        var products = await mediator.Send(new GetProductByIdQuery { Type = type.ToUpper(), Id = id });
        if (products == null)
            return NotFound($"No product found for type: {type} and id: {id}");

        logger.LogInformation($"Successfully called {nameof(this.GetProductById)} by {User.FindFirst("appid")?.Value ?? "Unknown"}. Type: {type}, Id: {id}");

        return new JsonResult(products);
    }
}