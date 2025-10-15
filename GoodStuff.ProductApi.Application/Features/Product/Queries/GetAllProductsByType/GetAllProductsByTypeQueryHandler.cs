using GoodStuff.ProductApi.Application.Services;
using MediatR;

namespace GoodStuff.ProductApi.Application.Features.Product.Queries.GetAllProductsByType;

public class GetAllProductsByTypeQueryHandler(UnitOfWork uow) : IRequestHandler<GetAllProductsByTypeQuery, object?>
{
    public async Task<object?> Handle(GetAllProductsByTypeQuery request, CancellationToken cancellationToken)
    {
        var dao = uow.CpuRepository.GetAllAsync()
        return dao == null ? Task.FromResult<object?>(null) : await dao.GetAllByTypeAsync(request.Type);
    }
}