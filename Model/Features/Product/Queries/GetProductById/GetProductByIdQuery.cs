using MediatR;

namespace Model.Features.Product.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<object?>
    {
        public string Type { get; init; }
        public int Id { get; init; }
    }
}