using GoodStuff.ProductApi.Domain.Products;
using GoodStuff.ProductApi.Domain.Products.Models;

namespace GoodStuff.ProductApi.Application.Tests.Helpers;

public static class ProductFactory
{
    public static Gpu CreateGpu() => new()
    {
        Name = "Test GPU",
        Category = ProductCategories.Gpu,
        Team = "AMD",
        Price = "3900",
        Id = "123",
        Warranty = "5 Years",
        ProducerCode = "GPU123"
    };

    public static Cpu CreateCpu() => new()
    {
        Name = "Test CPU",
        Category = ProductCategories.Cpu,
        Team = "Intel",
        Price = "2500",
        Id = "456",
        Warranty = "3 Years",
        ProducerCode = "CPU123"
    };

    public static Cooler CreateCooler() => new()
    {
        Name = "Test Cooler",
        Category = ProductCategories.Cooler,
        Team = "Noctua",
        Price = "120",
        Id = "789",
        Warranty = "6 Years",
        ProducerCode = "COOL123"
    };
}