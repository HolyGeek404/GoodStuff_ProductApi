using System.Net;
using GoodStuff.ProductApi.Application.Features.Product.Commands.Create;
using GoodStuff.ProductApi.Application.Features.Product.Commands.Delete;
using GoodStuff.ProductApi.Application.Features.Product.Commands.Update;
using GoodStuff.ProductApi.Application.Features.Product.Queries.GetById;
using GoodStuff.ProductApi.Application.Features.Product.Queries.GetByType;
using GoodStuff.ProductApi.Domain;
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
        Logger.LogCallingGetbytypenameByUnknownTypeType(logger, nameof(GetByType), caller, type);

        try
        {
            var result = await mediator.Send(new GetByTypeQuery { Type = type });
            if (result == null)
            {
                Logger.LogNoProductsFoundInGetbytypenameByUnknownTypeType(logger, nameof(GetByType), caller, type);
                return NotFound($"No products found for type: {type}");
            }

            Logger.LogSuccessfullyCalledGetbytypenameByUnknownTypeType(logger, nameof(GetByType), caller, type);
            return new JsonResult(result);
        }
        catch (Exception ex)
        {
            Logger.LogExceptionInGetbytypenameByUnknownTypeType(logger, ex, nameof(GetByType), caller, type);
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Get")]
    [Route("{id}")]
    public async Task<IActionResult> GetById(string type, string id)
    {
        var caller = User.FindFirst("appid")?.Value ?? "Unknown";
        Logger.LogCallingGetbyidnameByUnknownTypeTypeIdId(logger, nameof(GetById), caller, type, id);

        if (string.IsNullOrEmpty(id))
        {
            Logger.LogBadRequestInGetbyidnameByUnknownTypeTypeIdIsEmpty(logger, nameof(GetById), caller, type);
            return BadRequest("Product id cannot be empty.");
        }

        try
        {
            var products = await mediator.Send(new GetByIdQuery { Type = type, Id = id });
            if (products == null)
            {
                Logger.LogNoProductFoundInGetbyidnameByUnknownTypeTypeIdId(logger, nameof(GetById), caller, type, id);
                return NotFound($"No product found for type: {type} and id: {id}");
            }

            Logger.LogSuccessfullyCalledGetbyidnameByUnknownTypeTypeIdId(logger, nameof(GetById), caller, type, id);
            return new JsonResult(products);
        }
        catch (Exception ex)
        {
            Logger.LogExceptionInGetbyidnameByUnknownTypeTypeIdId(logger, ex, nameof(GetById), caller, type, id);
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPatch]
    [Authorize(Roles = "Update")]
    [Route("")]
    public async Task<IActionResult> Update([FromBody] string product, string type)
    {
        var caller = User.FindFirst("appid")?.Value ?? "Unknown";
        Logger.LogCallingUpdatenameByUnknownTypeTypeProductProduct(logger, nameof(Update), caller, type, product);

        if (string.IsNullOrEmpty(product))
        {
            Logger.LogBadRequestInUpdatenameByUnknownTypeTypeProductIsEmpty(logger, nameof(Update), caller, type);
            return BadRequest("Product cannot be empty.");
        }

        try
        {
            var result = await mediator.Send(new UpdateCommand { BaseProduct = product, Type = type });

            switch (result)
            {
                case HttpStatusCode.NoContent:
                case HttpStatusCode.OK:
                    Logger.LogSuccessfullyCalledUpdatenameByUnknownTypeTypeProductProduct(logger, nameof(Update), caller, type, product);
                    return NoContent();

                case HttpStatusCode.NotFound:
                    Logger.LogNoProductFoundInUpdatenameByUnknownTypeTypeProductProduct(logger, nameof(Update), caller, type, product);
                    return NotFound($"No product found for type: {type} and product: {product}");

                case HttpStatusCode.BadRequest:
                    Logger.LogUpdateReturnedBadRequestInUpdatenameByUnknownTypeTypeProductProduct(logger, nameof(Update), caller, type, product);
                    return BadRequest();

                default:
                    Logger.LogUpdateReturnedUnexpectedStatusStatusInUpdatenameByUnknownTypeTypeProduct(logger, result, nameof(Update), caller, type, product);
                    return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        catch (Exception ex)
        {
            Logger.LogExceptionInUpdatenameByUnknownTypeTypeProductProduct(logger, ex, nameof(Update), caller, type, product);
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Create")]
    [Route("")]
    public async Task<IActionResult> Create([FromBody] CreateCommand request, string type)
    {
        var caller = User.FindFirst("appid")?.Value ?? "Unknown";
        Logger.LogCallingCreatenameByCallerTypeTypeProductProduct(logger, nameof(Create), caller, request.Type, request.Product);

        if (string.IsNullOrEmpty(request.Product))
        {
            Logger.LogBadRequestInCreatenameByCallerTypeTypeProductIsEmpty(logger, nameof(Create), caller, request.Type);
            return BadRequest("Product cannot be empty.");
        }

        try
        {
            var result = await mediator.Send(request);

            if (result == null || string.IsNullOrEmpty(result.ProductId))
            {
                Logger.LogCreateFailedOrReturnedNullInCreatenameByCallerTypeTypeProductProduct(logger, nameof(Create), caller, request.Type, request.Product);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Logger.LogSuccessfullyCreatedProductInCreatenameByCallerTypeTypeProductProductIdId(logger, nameof(Create), caller, request.Type, request.Product, result.ProductId);

            return CreatedAtAction(nameof(GetById), new { type = result.Category, id = result.ProductId }, result);
        }
        catch (Exception ex)
        {
            Logger.LogExceptionInCreatenameByCallerTypeTypeProductProduct(logger, ex, nameof(Create), caller, request.Type, request.Product);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete]
    // [Authorize(Roles = "Delete")]
    [Route("")]
    public async Task<IActionResult> Delete(Guid id, string type)
    {
        var caller = User.FindFirst("appid")?.Value ?? "Unknown";
        Logger.LogDeleteRequestReceivedByCallerIdIdTypeType(logger, caller, id, type);

        if (id == Guid.Empty || string.IsNullOrEmpty(type))
        {
            Logger.LogDeleteFailedDueToMissingParametersCallerCallerIdIdTypeType(logger, caller, id, type);
            return BadRequest("Both 'id' and 'type' are required.");
        }

        try
        {
            var result = await mediator.Send(new DeleteCommand { Id = id, Type = type });

            switch (result)
            {
                case HttpStatusCode.NoContent:
                    Logger.LogSuccessfullyDeletedItemCallerCallerIdIdTypeType(logger, caller, id, type);
                    return NoContent();

                case HttpStatusCode.NotFound:
                    Logger.LogItemNotFoundForDeletionCallerCallerIdIdTypeType(logger, caller, id, type);
                    return NotFound();

                case HttpStatusCode.BadRequest:
                    Logger.LogBadRequestDuringDeletionCallerCallerIdIdTypeType(logger, caller, id, type);
                    return BadRequest();

                default:
                    Logger.LogUnexpectedStatusCodeStatusDuringDeletionCallerCallerIdIdTypeType(logger, result, caller, id, type);
                    return StatusCode((int)result);
            }
        }
        catch (Exception ex)
        {
            Logger.LogExceptionOccurredDuringDeletionCallerCallerIdIdTypeType(logger, ex, caller, id, type);
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
}