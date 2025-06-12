using MediatR;

namespace Model.Features.Product.Queries.GetAllProductsByType;

public record GetAllProductsByTypeQuery : IRequest<object?>
{
    public string Type { get; init; }
}