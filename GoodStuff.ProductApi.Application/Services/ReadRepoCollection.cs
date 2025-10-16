using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Domain.Products.Models;

namespace GoodStuff.ProductApi.Application.Services;

public class ReadRepoCollection(
    IReadRepository<Cpu> cpuRepository,
    IReadRepository<Gpu> gpuRepository,
    IReadRepository<Cooler> coolerRepository) : IReadRepoCollection
{
    public IReadRepository<Cpu> CpuRepository { get; } = cpuRepository;
    public IReadRepository<Gpu> GpuRepository { get; } = gpuRepository;
    public IReadRepository<Cooler> CoolerRepository { get; } = coolerRepository;
}