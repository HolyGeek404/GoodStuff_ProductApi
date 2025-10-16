using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Domain.Products.Models;

namespace GoodStuff.ProductApi.Application.Services;

public interface IUnitOfWork
{
    IRepository<Cpu> CpuRepository { get; }
    IRepository<Gpu> GpuRepository { get; }
    IRepository<Cooler> CoolerRepository { get; }
}