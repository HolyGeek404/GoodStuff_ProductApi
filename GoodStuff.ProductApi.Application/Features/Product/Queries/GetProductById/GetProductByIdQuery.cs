using MediatR;

namespace GoodStuff.ProductApi.Application.Features.Product.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<object?>
{
    public required string Type { get; init; }
    public required string Id { get; init; }
}