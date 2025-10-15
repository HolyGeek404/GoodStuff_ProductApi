using GoodStuff_DomainModels.Models.Products;
using GoodStuff.ProductApi.Application.Interfaces;

namespace GoodStuff.ProductApi.Application.Services;

public class UnitOfWork(
    IRepository<CpuModel> cpuRepository,
    IRepository<GpuModel> gpuRepository,
    IRepository<CoolerModel> coolerRepository)
{
    public IRepository<CpuModel> CpuRepository { get; } = cpuRepository;
    public IRepository<GpuModel> GpuRepository { get; } = gpuRepository;
    public IRepository<CoolerModel> CoolerRepository { get; } = coolerRepository;
}