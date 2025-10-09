using GoodStuff.ProductApi.Application.Services;
using MediatR;

namespace GoodStuff.ProductApi.Application.Features.Product.Queries.GetAllProductsByType;

public class GetAllProductsByTypeQueryHandler(IRepositoryFactory daoFactory) : IRequestHandler<GetAllProductsByTypeQuery, object?>
{
    public async Task<object?> Handle(GetAllProductsByTypeQuery request, CancellationToken cancellationToken)
    {
        var dao = daoFactory.GetRepository(request.Type);
        return dao == null ? Task.FromResult<object?>(null) : await dao.GetAllByTypeAsync(request.Type);
    }
}