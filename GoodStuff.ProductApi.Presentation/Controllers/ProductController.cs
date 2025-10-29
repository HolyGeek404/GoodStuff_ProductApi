using System.Net;
using GoodStuff.ProductApi.Application.Features.Product.Commands.Create;
using GoodStuff.ProductApi.Application.Features.Product.Commands.Delete;
using GoodStuff.ProductApi.Application.Features.Product.Commands.Update;
using GoodStuff.ProductApi.Application.Features.Product.Queries.GetById;
using GoodStuff.ProductApi.Application.Features.Product.Queries.GetByType;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        var caller = User.FindFirst("appid")?.Value ?? "Unknown";
        logger.LogInformation("Calling {GetByTypeName} by {Unknown}. Type: {Type}", nameof(GetByType), caller, type);

        try
        {
            var result = await mediator.Send(new GetByTypeQuery { Type = type });
            if (result == null)
            {
                logger.LogInformation("No products found in {GetByTypeName} by {Unknown}. Type: {Type}",
                    nameof(GetByType), caller, type);
                return NotFound($"No products found for type: {type}");
            }

            logger.LogInformation("Successfully called {GetByTypeName} by {Unknown}. Type: {Type}", nameof(GetByType),
                caller, type);
            return new JsonResult(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in {GetByTypeName} by {Unknown}. Type: {Type}", nameof(GetByType), caller,
                type);
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Get")]
    [Route("{id}")]
    public async Task<IActionResult> GetById(string type, string id)
    {
        var caller = User.FindFirst("appid")?.Value ?? "Unknown";
        logger.LogInformation("Calling {GetByIdName} by {Unknown}. Type: {Type}, Id: {Id}", nameof(GetById), caller,
            type, id);

        if (string.IsNullOrEmpty(id))
        {
            logger.LogWarning("Bad request in {GetByIdName} by {Unknown}. Type: {Type}, Id is empty", nameof(GetById),
                caller, type);
            return BadRequest("Product id cannot be empty.");
        }

        try
        {
            var products = await mediator.Send(new GetByIdQuery { Type = type, Id = id });
            if (products == null)
            {
                logger.LogInformation("No product found in {GetByIdName} by {Unknown}. Type: {Type}, Id: {Id}",
                    nameof(GetById), caller, type, id);
                return NotFound($"No product found for type: {type} and id: {id}");
            }

            logger.LogInformation("Successfully called {GetByIdName} by {Unknown}. Type: {Type}, Id: {Id}",
                nameof(GetById), caller, type, id);
            return new JsonResult(products);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in {GetByIdName} by {Unknown}. Type: {Type}, Id: {Id}", nameof(GetById),
                caller, type, id);
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPatch]
    [Authorize(Roles = "Update")]
    [Route("")]
    public async Task<IActionResult> Update([FromBody] string product, string type)
    {
        var caller = User.FindFirst("appid")?.Value ?? "Unknown";
        logger.LogInformation("Calling {UpdateName} by {Unknown}. Type: {Type}, Product: {Product}", nameof(Update),
            caller, type, product);

        if (string.IsNullOrEmpty(product))
        {
            logger.LogWarning("Bad request in {UpdateName} by {Unknown}. Type: {Type}, Product is empty",
                nameof(Update), caller, type);
            return BadRequest("Product cannot be empty.");
        }

        try
        {
            var result = await mediator.Send(new UpdateCommand { BaseProduct = product, Type = type });

            switch (result)
            {
                case HttpStatusCode.NoContent:
                case HttpStatusCode.OK:
                    logger.LogInformation(
                        "Successfully called {UpdateName} by {Unknown}. Type: {Type}, Product: {Product}",
                        nameof(Update), caller, type, product);
                    return NoContent();

                case HttpStatusCode.NotFound:
                    logger.LogInformation(
                        "No product found in {UpdateName} by {Unknown}. Type: {Type}, Product: {Product}",
                        nameof(Update), caller, type, product);
                    return NotFound($"No product found for type: {type} and product: {product}");

                case HttpStatusCode.BadRequest:
                    logger.LogWarning(
                        "Update returned bad request in {UpdateName} by {Unknown}. Type: {Type}, Product: {Product}",
                        nameof(Update), caller, type, product);
                    return BadRequest();

                default:
                    logger.LogWarning(
                        "Update returned unexpected status {Status} in {UpdateName} by {Unknown}. Type: {Type}, Product: {Product}",
                        result, nameof(Update), caller, type, product);
                    return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in {UpdateName} by {Unknown}. Type: {Type}, Product: {Product}",
                nameof(Update), caller, type, product);
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Create")]
    [Route("")]
    public async Task<IActionResult> Create([FromBody] CreateCommand request)
    {
        var caller = User.FindFirst("appid")?.Value ?? "Unknown";
        logger.LogInformation("Calling {CreateName} by {Caller}. Type: {Type}, Product: {Product}", nameof(Create),
            caller, request.Type, request.Product);

        if (string.IsNullOrEmpty(request.Product))
        {
            logger.LogWarning("Bad request in {CreateName} by {Caller}. Type: {Type}, Product is empty", nameof(Create),
                caller, request.Type);
            return BadRequest("Product cannot be empty.");
        }

        try
        {
            var result = await mediator.Send(request);

            if (result == null || string.IsNullOrEmpty(result.ProductId))
            {
                logger.LogWarning(
                    "Create failed or returned null in {CreateName} by {Caller}. Type: {Type}, Product: {Product}",
                    nameof(Create), caller, request.Type, request.Product);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            logger.LogInformation(
                "Successfully created product in {CreateName} by {Caller}. Type: {Type}, Product: {Product}, Id: {Id}",
                nameof(Create), caller, request.Type, request.Product, result.ProductId);

            return CreatedAtAction(nameof(GetById), new { type = result.Category, id = result.ProductId }, result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in {CreateName} by {Caller}. Type: {Type}, Product: {Product}",
                nameof(Create), caller, request.Type, request.Product);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete]
    // [Authorize(Roles = "Delete")]
    [Route("")]
    public async Task<IActionResult> Delete(Guid id, string type)
    {
        var caller = User.FindFirst("appid")?.Value ?? "Unknown";
        logger.LogInformation("Delete request received by {Caller}. Id: {Id}, Type: {Type}", caller, id, type);

        if (id == Guid.Empty || string.IsNullOrEmpty(type))
        {
            logger.LogWarning("Delete failed due to missing parameters. Caller: {Caller}, Id: {Id}, Type: {Type}",
                caller, id, type);
            return BadRequest("Both 'id' and 'type' are required.");
        }

        try
        {
            var result = await mediator.Send(new DeleteCommand { Id = id, Type = type });

            switch (result)
            {
                case HttpStatusCode.NoContent:
                    logger.LogInformation("Successfully deleted item. Caller: {Caller}, Id: {Id}, Type: {Type}", caller,
                        id, type);
                    return NoContent();

                case HttpStatusCode.NotFound:
                    logger.LogWarning("Item not found for deletion. Caller: {Caller}, Id: {Id}, Type: {Type}", caller,
                        id, type);
                    return NotFound();

                case HttpStatusCode.BadRequest:
                    logger.LogWarning("Bad request during deletion. Caller: {Caller}, Id: {Id}, Type: {Type}", caller,
                        id, type);
                    return BadRequest();

                default:
                    logger.LogError(
                        "Unexpected status code {Status} during deletion. Caller: {Caller}, Id: {Id}, Type: {Type}",
                        result, caller, id, type);
                    return StatusCode((int)result);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred during deletion. Caller: {Caller}, Id: {Id}, Type: {Type}", caller,
                id, type);
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
}