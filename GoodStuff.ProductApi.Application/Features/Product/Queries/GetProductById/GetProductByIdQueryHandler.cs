using GoodStuff.ProductApi.Application.Services;
using MediatR;

namespace GoodStuff.ProductApi.Application.Features.Product.Queries.GetProductById;

public class GetProductByIdQueryHandler(IRepositoryFactory daoFactory) : IRequestHandler<GetProductByIdQuery, object?>
{
    public async Task<object?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var dao = daoFactory.GetRepository(request.Type);
        return dao == null ? Task.FromResult<object?>(null) : await dao.GetSingleById(request.Type, request.Id);
    }
}