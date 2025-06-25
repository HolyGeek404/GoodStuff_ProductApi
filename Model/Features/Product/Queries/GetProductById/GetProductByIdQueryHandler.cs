using MediatR;
using Model.DataAccess;

namespace Model.Features.Product.Queries.GetProductById;

public class GetProductByIdQueryHandler(IProductDao productDao) : IRequestHandler<GetProductByIdQuery, object?>
{
    public Task<object?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return productDao.GetProductByIdQuery(request.Type, request.Id);
    }
}