using GoodStuff.ProductApi.Domain.Products.Models;

namespace GoodStuff.ProductApi.Application.Interfaces;

public interface IRepoCollection
{
    IRepository<Cpu> CpuRepository { get; }
    IRepository<Gpu> GpuRepository { get; }
    IRepository<Cooler> CoolerRepository { get; }
}