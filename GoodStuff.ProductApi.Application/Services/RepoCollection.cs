using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Domain.Products.Models;

namespace GoodStuff.ProductApi.Application.Services;

public class RepoCollection(
    IRepository<Cpu> cpuRepository,
    IRepository<Gpu> gpuRepository,
    IRepository<Cooler> coolerRepository) : IUnitOfWork
{
    public IRepository<Cpu> CpuRepository { get; } = cpuRepository;
    public IRepository<Gpu> GpuRepository { get; } = gpuRepository;
    public IRepository<Cooler> CoolerRepository { get; } = coolerRepository;
}