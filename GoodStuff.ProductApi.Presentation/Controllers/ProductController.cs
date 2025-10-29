using System.Net;
using GoodStuff.ProductApi.Application.Features.Product.Commands.Update;
using GoodStuff.ProductApi.Application.Features.Product.Queries.GetById;
using GoodStuff.ProductApi.Application.Features.Product.Queries.GetByType;
using GoodStuff.ProductApi.Domain.Products.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GoodStuff.ProductApi.Presentation.Controllers;

[ApiController]
[Route("[controller]/{type:alpha}")]
public class ProductController(IMediator mediator, ILogger<ProductController> logger) : Controller
{
    [HttpGet]
    [Authorize(Roles = "Get")]
    [Route("")]
    public async Task<IActionResult> GetByType(string type)
    {
        logger.LogInformation("Calling {GetByTypeName} by {Unknown}. Type: {Type}", nameof(GetByType), User.FindFirst("appid")?.Value ?? "Unknown", type);

        var result = await mediator.Send(new GetByTypeQuery { Type = type });
        if (result == null)
            return NotFound($"No products found for type: {type}");

        logger.LogInformation("Successfully called {GetByTypeName} by {Unknown}. Type: {Type}", nameof(GetByType), User.FindFirst("appid")?.Value ?? "Unknown", type);

        return new JsonResult(result);
    }

    [HttpGet]
    [Authorize(Roles = "Get")]
    [Route("{id}")]
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

    [HttpPatch]
    [Authorize(Roles = "Update")]
    [Route("")]
    public async Task<IActionResult> Update(string product, string type)
    {
        var caller = User.FindFirst("appid")?.Value ?? "Unknown";
        logger.LogInformation("Calling {UpdateName} by {Unknown}. Type: {Type}, Product: {Product}", nameof(Update), caller, type, product);

        if (string.IsNullOrEmpty(product))
        {
            logger.LogWarning("Bad request in {UpdateName} by {Unknown}. Type: {Type}, Product is empty", nameof(Update), caller, type);
            return BadRequest("Product cannot be empty.");
        }

        try
        {
            var result = await mediator.Send(new UpdateCommand { BaseProduct = product, Type = type });

            switch (result)
            {
                case HttpStatusCode.NoContent:
                case HttpStatusCode.OK:
                    logger.LogInformation("Successfully called {UpdateName} by {Unknown}. Type: {Type}, Product: {Product}", nameof(Update), caller, type, product);
                    return NoContent();

                case HttpStatusCode.NotFound:
                    logger.LogInformation("No product found in {UpdateName} by {Unknown}. Type: {Type}, Product: {Product}", nameof(Update), caller, type, product);
                    return NotFound($"No product found for type: {type} and product: {product}");

                case HttpStatusCode.BadRequest:
                    logger.LogWarning("Update returned bad request in {UpdateName} by {Unknown}. Type: {Type}, Product: {Product}", nameof(Update), caller, type, product);
                    return BadRequest();

                default:
                    logger.LogWarning("Update returned unexpected status {Status} in {UpdateName} by {Unknown}. Type: {Type}, Product: {Product}", result, nameof(Update), caller, type, product);
                    return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in {UpdateName} by {Unknown}. Type: {Type}, Product: {Product}", nameof(Update), caller, type, product);
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }


}