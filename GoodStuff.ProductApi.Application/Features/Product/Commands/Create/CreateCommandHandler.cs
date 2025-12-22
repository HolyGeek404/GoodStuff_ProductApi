using System.Text.Json;
using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Domain.Products;
using GoodStuff.ProductApi.Domain.Products.Models;
using MediatR;

namespace GoodStuff.ProductApi.Application.Features.Product.Commands.Create;

public class CreateCommandHandler(IWriteRepoCollection uow) : IRequestHandler<CreateCommand, BaseProduct?>
{
    public async Task<BaseProduct?> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        switch (request.Type.ToUpper())
        {
            case ProductCategories.Gpu:
                var gpu = request.Product.Deserialize<Gpu>()!;
                return await uow.GpuRepository.CreateAsync(gpu, gpu.Id, gpu.Category);
            case ProductCategories.Cpu:
                var cpu = request.Product.Deserialize<Cpu>()!;
                return await uow.CpuRepository.CreateAsync(cpu, cpu.Id, cpu.Category);
            case ProductCategories.Cooler:
                var cooler = request.Product.Deserialize<Cooler>()!;
                return await uow.CoolerRepository.CreateAsync(cooler, cooler.Id, cooler.Category);
            default:
                return null;
        }
    }
}