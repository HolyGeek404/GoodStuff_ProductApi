
using MediatR;

namespace GoodStuff.ProductApi.Application.Features.Product.Queries.GetAllProductsByType;

public record GetAllProductsByTypeQuery : IRequest<object?>
{
    public ProductCategories Type { get; init; }
}