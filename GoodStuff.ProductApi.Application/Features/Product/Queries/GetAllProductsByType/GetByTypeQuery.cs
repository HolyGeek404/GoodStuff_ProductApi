using MediatR;

namespace GoodStuff.ProductApi.Application.Features.Product.Queries.GetAllProductsByType;

public record GetByTypeQuery : IRequest<object?>
{
    public required string Type { get; init; }
}