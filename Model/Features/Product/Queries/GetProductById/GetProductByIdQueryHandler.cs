using MediatR;
using Model.Services;

namespace Model.Features.Product.Queries.GetProductById;

public class GetProductByIdQueryHandler(IProductDaoFactory daoFactory) : IRequestHandler<GetProductByIdQuery, object?>
{
    public Task<object?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var dao = daoFactory.GetProductDao(request.Type);
        return dao == null ? Task.FromResult<object?>(null) : dao.GetProductByIdQuery(request.Type, request.Id);
    }
}