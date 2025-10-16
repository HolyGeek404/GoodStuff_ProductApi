using GoodStuff.ProductApi.Domain.Products.Models;

namespace GoodStuff.ProductApi.Application.Interfaces;

public interface IReadRepoCollection
{
    IReadRepository<Cpu> CpuRepository { get; }
    IReadRepository<Gpu> GpuRepository { get; }
    IReadRepository<Cooler> CoolerRepository { get; }
}