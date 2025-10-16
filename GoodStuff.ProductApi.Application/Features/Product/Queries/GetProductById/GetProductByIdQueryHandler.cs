using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Application.Services;
using GoodStuff.ProductApi.Domain.Products;
using MediatR;

namespace GoodStuff.ProductApi.Application.Features.Product.Queries.GetProductById;

public class GetProductByIdQueryHandler(IReadRepoCollection uow) : IRequestHandler<GetProductByIdQuery, object?>
{
    public async Task<object?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return request.Type switch
        {
            ProductCategories.Gpu => await uow.GpuRepository.GetById(request.Type, request.Id),
            ProductCategories.Cpu => await uow.CpuRepository.GetById(request.Type, request.Id),
            ProductCategories.Cooler => await uow.CoolerRepository.GetById(request.Type, request.Id),
            _ => Enumerable.Empty<object>()
        };
    }
}