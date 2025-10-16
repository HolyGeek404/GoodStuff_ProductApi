using GoodStuff.ProductApi.Application.Features.Product.Queries.GetAllProductsByType;
using GoodStuff.ProductApi.Application.Features.Product.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodStuff.ProductApi.Presentation.Controllers;

[ApiController]
[Route("[controller]/{type:alpha}")]
public class ProductController(IMediator mediator, ILogger<ProductController> logger) : Controller
{
    [HttpGet]
    [Authorize(Roles = "GetProducts")]
    [Route("")]
    public async Task<IActionResult> GetByType(string type)
    {
        logger.LogInformation("Calling {GetByTypeName} by {Unknown}. Type: {Type}", nameof(GetByType), User.FindFirst("appid")?.Value ?? "Unknown", type);

        var result = await mediator.Send(new GetByTypeQuery { Type = type });
        if (result is not List<object> { Count: > 0 } products)
            return NotFound($"No products found for type: {type}");

        logger.LogInformation("Successfully called {GetByTypeName} by {Unknown}. Type: {Type}", nameof(GetByType), User.FindFirst("appid")?.Value ?? "Unknown", type);

        return new JsonResult(products);
    }

    [HttpGet]
    [Authorize(Roles = "GetSingleProduct")]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(string type, string id)
    {
        logger.LogInformation("Calling {GetByIdName} by {Unknown}. Type: {Type}, Id: {Id}", nameof(GetById), User.FindFirst("appid")?.Value ?? "Unknown", type, id);

        if (string.IsNullOrEmpty(id))
            return BadRequest("Product id cannot be empty.");

        var products = await mediator.Send(new GetByIdQuery { Type = type, Id = id });
        if (products == null)
            return NotFound($"No product found for type: {type} and id: {id}");

        logger.LogInformation("Successfully called {GetByIdName} by {Unknown}. Type: {Type}, Id: {Id}", nameof(GetById), User.FindFirst("appid")?.Value ?? "Unknown", type, id);

        return new JsonResult(products);
    }
}