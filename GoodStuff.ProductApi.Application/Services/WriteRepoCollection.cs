using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Domain.Products.Models;

namespace GoodStuff.ProductApi.Application.Services;

public class WriteRepoCollection(
    IWriteRepository<Cpu> cpuRepository,
    IWriteRepository<Gpu> gpuRepository,
    IWriteRepository<Cooler> coolerRepository) : IWriteRepoCollection
{
    public IWriteRepository<Cpu> CpuRepository { get; } = cpuRepository;
    public IWriteRepository<Gpu> GpuRepository { get; } = gpuRepository;
    public IWriteRepository<Cooler> CoolerRepository { get; } = coolerRepository;
}