using GoodStuff_DomainModels.Models.Enums;
using MediatR;

namespace Model.Features.Product.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<object?>
    {
        public ProductCategories Type { get; init; }
        public string Id { get; init; }
    }
}