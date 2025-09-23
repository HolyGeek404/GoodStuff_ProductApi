using GoodStuff_DomainModels.Models.Enums;
using MediatR;

namespace Model.Features.Product.Queries.GetAllProductsByType;

public record GetAllProductsByTypeQuery : IRequest<object?>
{
    public ProductCategories Type { get; init; }
}