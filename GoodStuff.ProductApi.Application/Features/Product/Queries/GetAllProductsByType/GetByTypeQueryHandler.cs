using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Domain.Products;
using MediatR;

namespace GoodStuff.ProductApi.Application.Features.Product.Queries.GetAllProductsByType;

public class GetByTypeQueryHandler(IReadRepoCollection uow) : IRequestHandler<GetByTypeQuery, object?>
{
    public async Task<object?> Handle(GetByTypeQuery request, CancellationToken cancellationToken)
    {
        return request.Type switch
        {
            ProductCategories.Gpu => await uow.GpuRepository.GetAllAsync(request.Type),
            ProductCategories.Cpu => await uow.CpuRepository.GetAllAsync(request.Type),
            ProductCategories.Cooler => await uow.CoolerRepository.GetAllAsync(request.Type),
            _ => Enumerable.Empty<object>()
        };
    }
}