using GoodStuff.ProductApi.Application.Services;
using MediatR;

namespace GoodStuff.ProductApi.Application.Features.Product.Queries.GetAllProductsByType;

public class GetAllProductsByTypeQueryHandler(IProductDaoFactory daoFactory)
    : IRequestHandler<GetAllProductsByTypeQuery, object?>
{
    public Task<object?> Handle(GetAllProductsByTypeQuery request, CancellationToken cancellationToken)
    {
        var dao = daoFactory.GetProductDao(request.Type);
        return dao == null ? Task.FromResult<object?>(null) : dao.GetAllProductsByType(request.Type);
    }
}