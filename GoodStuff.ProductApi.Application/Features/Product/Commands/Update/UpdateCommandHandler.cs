using System.Net;
using System.Text.Json;
using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Domain.Products;
using GoodStuff.ProductApi.Domain.Products.Models;
using MediatR;

namespace GoodStuff.ProductApi.Application.Features.Product.Commands.Update;

public class UpdateCommandHandler(IWriteRepoCollection uow) : IRequestHandler<UpdateCommand, HttpStatusCode>
{
    public async Task<HttpStatusCode> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        switch (request.Type.ToUpper())
        {
            case ProductCategories.Gpu:
                var gpu = request.BaseProduct.Deserialize<Gpu>()!;
                return await uow.GpuRepository.UpdateAsync(gpu, gpu.Id, gpu.Category);
            case ProductCategories.Cpu:
                var cpu = request.BaseProduct.Deserialize<Cpu>()!;
                return await uow.CpuRepository.UpdateAsync(cpu, cpu.Id, cpu.Category);
            case ProductCategories.Cooler:
                var cooler = request.BaseProduct.Deserialize<Cooler>()!;
                return await uow.CoolerRepository.UpdateAsync(cooler, cooler.Id, cooler.Category);
            default:
                return HttpStatusCode.BadRequest;
        }
    }
}