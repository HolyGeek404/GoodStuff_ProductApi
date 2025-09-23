using GoodStuff_DomainModels.Models.Enums;
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
    public async Task<IActionResult> GetAllProductsByType(ProductCategories type)
    {
        logger.LogInformation(
            $"Calling {nameof(GetAllProductsByType)} by {User.FindFirst("appid")?.Value ?? "Unknown"}. Type: {type}");

        var products = await mediator.Send(new GetAllProductsByTypeQuery { Type = type });
        if (products == null)
            return NotFound($"No products found for type: {type}");

        logger.LogInformation(
            $"Successfully called {nameof(GetAllProductsByType)} by {User.FindFirst("appid")?.Value ?? "Unknown"}. Type: {type}");

        return new JsonResult(products);
    }

    [HttpGet]
    [Authorize(Roles = "GetSingleProduct")]
    [Route("GetProductById")]
    public async Task<IActionResult> GetProductById(ProductCategories type, string id)
    {
        logger.LogInformation(
            $"Calling {nameof(GetProductById)} by {User.FindFirst("appid")?.Value ?? "Unknown"}. Type: {type}, Id: {id}");

        if (string.IsNullOrEmpty(id))
            return BadRequest("Product id cannot be empty.");

        var products = await mediator.Send(new GetProductByIdQuery { Type = type, Id = id });
        if (products == null)
            return NotFound($"No product found for type: {type} and id: {id}");

        logger.LogInformation(
            $"Successfully called {nameof(GetProductById)} by {User.FindFirst("appid")?.Value ?? "Unknown"}. Type: {type}, Id: {id}");

        return new JsonResult(products);
    }
}