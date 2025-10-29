using System.Net;
using GoodStuff.ProductApi.Domain.Products.Models;
using MediatR;

namespace GoodStuff.ProductApi.Application.Features.Product.Commands.Update;

public class UpdateCommand:IRequest<HttpStatusCode>
{
    public required string  BaseProduct { get; set; }
    public required string Type { get; set; }
}