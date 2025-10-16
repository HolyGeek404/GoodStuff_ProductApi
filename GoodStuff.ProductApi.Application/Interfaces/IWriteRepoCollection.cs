using GoodStuff.ProductApi.Domain.Products.Models;

namespace GoodStuff.ProductApi.Application.Interfaces;

public interface IWriteRepoCollection
{
    IWriteRepository<Cpu> CpuRepository { get; }
    IWriteRepository<Gpu> GpuRepository { get; }
    IWriteRepository<Cooler> CoolerRepository { get; }
}