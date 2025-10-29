using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Domain.Products;
using GoodStuff.ProductApi.Domain.Products.Models;
using MediatR;
using Newtonsoft.Json;

namespace GoodStuff.ProductApi.Application.Features.Product.Commands.Create;

public class CreateCommandHandler(IWriteRepoCollection uow) : IRequestHandler<CreateCommand, BaseProduct?>
{
    public async Task<BaseProduct?> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        switch (request.Type.ToUpper())
        {
            case ProductCategories.Gpu:
                var gpu = JsonConvert.DeserializeObject<Gpu>(request.Product);
                return await uow.GpuRepository.CreateAsync(gpu, gpu.Id, gpu.Category);
            case ProductCategories.Cpu:
                var cpu = JsonConvert.DeserializeObject<Cpu>(request.Product);
                return await uow.CpuRepository.CreateAsync(cpu, cpu.Id, cpu.Category);
            case ProductCategories.Cooler:
                var cooler = JsonConvert.DeserializeObject<Cooler>(request.Product);
                return await uow.CoolerRepository.CreateAsync(cooler, cooler.Id, cooler.Category);
            default:
                return null;
        }
    }
}