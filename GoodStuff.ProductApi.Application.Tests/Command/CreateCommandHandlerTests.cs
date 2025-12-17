using GoodStuff.ProductApi.Application.Features.Product.Commands.Create;
using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Application.Services;
using GoodStuff.ProductApi.Domain.Products;
using GoodStuff.ProductApi.Domain.Products.Models;
using Moq;

namespace GoodStuff.ProductApi.Application.Tests.Command;

public class CreateCommandHandlerTests
{
    private readonly Mock<IWriteRepository<Gpu>> _gpuRepo = new();
    private readonly Mock<IWriteRepository<Cpu>> _cpuRepo = new();
    private readonly Mock<IWriteRepository<Cooler>> _coolerRepo = new();
    private readonly IWriteRepoCollection _uow;

    public CreateCommandHandlerTests()
    {
        _uow = new WriteRepoCollection(_cpuRepo.Object, _gpuRepo.Object, _coolerRepo.Object);
    }

    [Fact]
    public async Task Handle_GpuCommand_CallsGpuRepository()
    {
        // Arrange
        var gpu = new Gpu
        {
            Id = "534",
            Category = ProductCategories.Gpu,
            Name = "RX 7600",
            Team = "AMD",
            Price = "1300",
            ProductId = "123",
            Warranty = "ZXC",
            ProducerCode = "ZXC123"
        };
        var command = new CreateCommand
        {
            Type = ProductCategories.Gpu,
            Product = System.Text.Json.JsonSerializer.Serialize(gpu)
        };
        _gpuRepo.Setup(r => r.CreateAsync(It.IsAny<Gpu>(), gpu.Id, gpu.Category)).ReturnsAsync(gpu);

        var handler = new CreateCommandHandler(_uow);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Gpu>(result);
        _gpuRepo.Verify(r => r.CreateAsync(It.IsAny<Gpu>(), gpu.Id, gpu.Category), Times.Once);
    }

    [Fact]
    public async Task Handle_CpuCommand_CallsCpuRepository()
    {
        // Arrange
        var cpu = new Cpu
        {
            Id = "6546",
            Category = ProductCategories.Cpu,
            Name = "Ryzen 5",
            Team = "AMD",
            Price = "130",
            ProductId = "123",
            Warranty = "ZXC",
            ProducerCode = "ZXC123"
        };
        var command = new CreateCommand
        {
            Type = "CPU",
            Product = System.Text.Json.JsonSerializer.Serialize(cpu)
        };
        _cpuRepo.Setup(r => r.CreateAsync(It.IsAny<Cpu>(), cpu.Id, cpu.Category)).ReturnsAsync(cpu);
        var handler = new CreateCommandHandler(_uow);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<Cpu>(result);
        _cpuRepo.Verify(r => r.CreateAsync(It.IsAny<Cpu>(), cpu.Id, cpu.Category), Times.Once);
    }

    [Fact]
    public async Task Handle_UnknownType_ReturnsNull()
    {
        var command = new CreateCommand
        {
            Type = "unknown",
            Product = "{}"
        };

        var handler = new CreateCommandHandler(_uow);
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Null(result);
    }
}