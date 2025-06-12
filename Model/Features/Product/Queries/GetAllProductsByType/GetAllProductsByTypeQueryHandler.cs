using MediatR;
using Model.DataAccess;

namespace Model.Features.Product.Queries.GetAllProductsByType;

public class GetAllProductsByTypeQueryHandler(IProductDao productDao) : IRequestHandler<GetAllProductsByTypeQuery, object?>
{
    public Task<object?> Handle(GetAllProductsByTypeQuery request, CancellationToken cancellationToken)
    {
        return productDao.GetAllProductsByType(request.Type);
    }
}
