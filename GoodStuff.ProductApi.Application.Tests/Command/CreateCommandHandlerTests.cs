using GoodStuff.ProductApi.Application.Features.Product.Commands.Create;
using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Application.Services;
using GoodStuff.ProductApi.Domain.Products;
using GoodStuff.ProductApi.Domain.Products.Models;
using Moq;
using System.Text.Json;
using GoodStuff.ProductApi.Application.Tests.Helpers;

namespace GoodStuff.ProductApi.Application.Tests.Command;

public class CreateCommandHandlerTests
{
    private readonly Mock<IWriteRepository<Gpu>> _gpuRepo = new();
    private readonly Mock<IWriteRepository<Cpu>> _cpuRepo = new();
    private readonly Mock<IWriteRepository<Cooler>> _coolerRepo = new();
    private readonly CreateCommandHandler _handler;

    public CreateCommandHandlerTests()
    {
        IWriteRepoCollection uow = new WriteRepoCollection(_cpuRepo.Object, _gpuRepo.Object, _coolerRepo.Object);
        _handler = new CreateCommandHandler(uow);
    }

    [Fact]
    public async Task Handle_WhenTypeIsGpu_CallsGpuRepository()
    {
        // Arrange
        var gpu = ProductFactory.CreateGpu();
        var command = new CreateCommand
        {
            Type = ProductCategories.Gpu,
            Product = JsonSerializer.Serialize(gpu)
        };

        _gpuRepo.Setup(r => r.CreateAsync(It.IsAny<Gpu>(), gpu.Id, gpu.Category)).ReturnsAsync(gpu);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Gpu>(result);
        _gpuRepo.Verify(r => r.CreateAsync(It.IsAny<Gpu>(), gpu.Id, gpu.Category), Times.Once);

        VerifyOnly(command.Type);
    }

    [Fact]
    public async Task Handle_WhenTypeIsCpu_CallsCpuRepository()
    {
        // Arrange
        var cpu = ProductFactory.CreateCpu();
        var command = new CreateCommand
        {
            Type = ProductCategories.Cpu,
            Product = JsonSerializer.Serialize(cpu)
        };
        _cpuRepo.Setup(r => r.CreateAsync(It.IsAny<Cpu>(), cpu.Id, cpu.Category)).ReturnsAsync(cpu);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Cpu>(result);
        _cpuRepo.Verify(r => r.CreateAsync(It.IsAny<Cpu>(), cpu.Id, cpu.Category), Times.Once);

        VerifyOnly(command.Type);
    }
    
    [Fact]
    public async Task Handle_WhenTypeIsCooler_CallsCoolerRepository()
    {
        // Arrange
        var cooler = ProductFactory.CreateCooler();
        var command = new CreateCommand
        {
            Type = ProductCategories.Cooler,
            Product = JsonSerializer.Serialize(cooler)
        };
        _coolerRepo.Setup(r => r.CreateAsync(It.IsAny<Cooler>(), cooler.Id, cooler.Category)).ReturnsAsync(cooler);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Cooler>(result);
        _coolerRepo.Verify(r => r.CreateAsync(It.IsAny<Cooler>(), cooler.Id, cooler.Category), Times.Once);

        VerifyOnly(command.Type);
    }

    [Fact]
    public async Task Handle_WhenTypeIsUnsupported_ReturnsNull()
    {
        // Arrange
        var command = new CreateCommand
        {
            Type = "unknown",
            Product = "{}"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);
        VerifyOnly(command.Type);
    }

    // ---------- Helpers ----------

    private void VerifyOnly(string type)
    {
        _gpuRepo.Verify(r => r.CreateAsync(It.IsAny<Gpu>(), It.IsAny<string>(),It.IsAny<string>()), type == ProductCategories.Gpu ? Times.Once() : Times.Never());
        _cpuRepo.Verify(r => r.CreateAsync(It.IsAny<Cpu>(), It.IsAny<string>(),It.IsAny<string>()), type == ProductCategories.Cpu ? Times.Once() : Times.Never());
        _coolerRepo.Verify(r => r.CreateAsync(It.IsAny<Cooler>(), It.IsAny<string>(),It.IsAny<string>()), type == ProductCategories.Cooler ? Times.Once() : Times.Never());
    }
}