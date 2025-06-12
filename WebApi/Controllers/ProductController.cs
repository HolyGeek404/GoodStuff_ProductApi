using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Features.Product.Queries.GetAllProductsByType;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController(IMediator mediator, ILogger<ProductController> logger) : Controller
{
    [HttpGet]
    [Authorize(Roles = "Base")]
    [Route("getallproductsbytype")]
    public async Task<IActionResult> GetAllProductsByType(string type)
    {
        logger.LogInformation($"Calling {nameof(this.GetAllProductsByType)} by {User.FindFirst("appid")?.Value ?? "Unknown"}. Type: {type}");

        if (string.IsNullOrEmpty(type))
            return BadRequest("Product type cannot be null or empty.");

        var products = await mediator.Send(new GetAllProductsByTypeQuery{Type = type.ToUpper()});
        if (products == null)
            return NotFound($"No products found for type: {type}");

        logger.LogInformation($"Successfully called {nameof(this.GetAllProductsByType)} by {User.FindFirst("appid")?.Value ?? "Unknown"}. Type: {type}");

        return new JsonResult(products);
    }
}