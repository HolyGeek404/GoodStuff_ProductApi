using System.Net;
using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Domain.Products;
using GoodStuff.ProductApi.Domain.Products.Models;
using MediatR;
using Newtonsoft.Json;

namespace GoodStuff.ProductApi.Application.Features.Product.Commands.Update;

public class UpdateCommandHandler(IWriteRepoCollection uow) : IRequestHandler<UpdateCommand, HttpStatusCode>
{
    public async Task<HttpStatusCode> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        switch (request.Type.ToUpper())
        {
            case ProductCategories.Gpu:
                var gpu = JsonConvert.DeserializeObject<Gpu>(request.BaseProduct);
                return await uow.GpuRepository.UpdateAsync(gpu, gpu.Id, gpu.Category);
            case ProductCategories.Cpu:
                var cpu = JsonConvert.DeserializeObject<Cpu>(request.BaseProduct);
                return await uow.CpuRepository.UpdateAsync(cpu, cpu.Id, cpu.Category);
            case ProductCategories.Cooler:
                var cooler = JsonConvert.DeserializeObject<Cooler>(request.BaseProduct);
                return await uow.CoolerRepository.UpdateAsync(cooler, cooler.Id, cooler.Category);
            default:
                return HttpStatusCode.BadRequest;
        }
    }
}