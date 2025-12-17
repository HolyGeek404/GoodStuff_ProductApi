using GoodStuff.ProductApi.Application.Features.Product.Queries.GetById;
using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Domain.Products;
using GoodStuff.ProductApi.Domain.Products.Models;
using Moq;

namespace GoodStuff.ProductApi.Application.Tests.Queries;

public class GetByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenTypeIsGpu_CallsGpuRepositoryAndReturnsResult()
    {
        // Arrange
        var gpu = new Gpu
        {
            Name = "Test GPU",
            Category = ProductCategories.Gpu,
            Team = "AMD",
            Price = "3900",
            Id = "123",
            ProductId = "321",
            Warranty = "5 Years",
            ProducerCode = "GPU123"
        };
        var gpuRepoMock = new Mock<IReadRepository<Gpu>>();
        var cpuRepoMock = new Mock<IReadRepository<Cpu>>();
        var coolerRepoMock = new Mock<IReadRepository<Cooler>>();
        gpuRepoMock.Setup(r => r.GetById(ProductCategories.Gpu, "321")).ReturnsAsync(gpu);
        var uowMock = new Mock<IReadRepoCollection>();
        
        uowMock.SetupGet(x => x.GpuRepository).Returns(gpuRepoMock.Object);
        uowMock.SetupGet(x => x.CpuRepository).Returns(cpuRepoMock.Object);
        uowMock.SetupGet(x => x.CoolerRepository).Returns(coolerRepoMock.Object);
        
        var handler = new GetByIdQueryHandler(uowMock.Object);
        var query = new GetByIdQuery { Type = ProductCategories.Gpu, Id = "321" };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        var typedResult = Assert.IsType<Gpu>(result);
        Assert.Equal(gpu, typedResult);
        gpuRepoMock.Verify(r => r.GetById(ProductCategories.Gpu, "321"), Times.Once);
        cpuRepoMock.Verify(r => r.GetById(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        coolerRepoMock.Verify(r => r.GetById(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenTypeIsCpu_CallsCpuRepositoryAndReturnsResult()
    {
        // Arrange
        var cpu = new Cpu
        {
            Name = "Test CPU",
            Category = ProductCategories.Cpu,
            Team = "Intel",
            Price = "2500",
            Id = "456",
            ProductId = "654",
            Warranty = "3 Years",
            ProducerCode = "CPU123"
        };
        var gpuRepoMock = new Mock<IReadRepository<Gpu>>();
        var cpuRepoMock = new Mock<IReadRepository<Cpu>>();
        var coolerRepoMock = new Mock<IReadRepository<Cooler>>();
        cpuRepoMock.Setup(r => r.GetById(ProductCategories.Cpu, "654")).ReturnsAsync(cpu);
        
        var uowMock = new Mock<IReadRepoCollection>();
        uowMock.SetupGet(x => x.GpuRepository).Returns(gpuRepoMock.Object);
        uowMock.SetupGet(x => x.CpuRepository).Returns(cpuRepoMock.Object);
        uowMock.SetupGet(x => x.CoolerRepository).Returns(coolerRepoMock.Object);
        
        var handler = new GetByIdQueryHandler(uowMock.Object);
        var query = new GetByIdQuery { Type = ProductCategories.Cpu, Id = "654" };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        var typedResult = Assert.IsType<Cpu>(result);
        Assert.Equal(cpu, typedResult);
        cpuRepoMock.Verify(r => r.GetById(ProductCategories.Cpu, "654"), Times.Once);
        gpuRepoMock.Verify(r => r.GetById(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        coolerRepoMock.Verify(r => r.GetById(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenTypeIsCooler_CallsCoolerRepositoryAndReturnsResult()
    {
        // Arrange
        var cooler = new Cooler
        {
            Name = "Test Cooler",
            Category = ProductCategories.Cooler,
            Team = "Noctua",
            Price = "120",
            Id = "789",
            ProductId = "987",
            Warranty = "6 Years",
            ProducerCode = "COOL123"
        };
        var gpuRepoMock = new Mock<IReadRepository<Gpu>>();
        var cpuRepoMock = new Mock<IReadRepository<Cpu>>();
        var coolerRepoMock = new Mock<IReadRepository<Cooler>>();
        coolerRepoMock.Setup(r => r.GetById(ProductCategories.Cooler, "987")).ReturnsAsync(cooler);
        
        var uowMock = new Mock<IReadRepoCollection>();
        uowMock.SetupGet(x => x.GpuRepository).Returns(gpuRepoMock.Object);
        uowMock.SetupGet(x => x.CpuRepository).Returns(cpuRepoMock.Object);
        uowMock.SetupGet(x => x.CoolerRepository).Returns(coolerRepoMock.Object);
        
        var handler = new GetByIdQueryHandler(uowMock.Object);
        var query = new GetByIdQuery { Type = ProductCategories.Cooler, Id = "987" };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        var typedResult = Assert.IsType<Cooler>(result);
        Assert.Equal(cooler, typedResult);
        coolerRepoMock.Verify(r => r.GetById(ProductCategories.Cooler, "987"), Times.Once);
        gpuRepoMock.Verify(r => r.GetById(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        cpuRepoMock.Verify(r => r.GetById(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenTypeIsNotSupported_ReturnsEmptyEnumerable()
    {
        // Arrange
        var uowMock = new Mock<IReadRepoCollection>();
        var handler = new GetByIdQueryHandler(uowMock.Object);
        var query = new GetByIdQuery { Type = "UNSUPPORTED", Id = "123" };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<IEnumerable<object>>(result, exactMatch: false);
        Assert.Empty((IEnumerable<object>)result);
    }

    [Fact]
    public async Task Handle_WhenCancellationRequested_ThrowsOperationCanceledException()
    {
        // Arrange
        var uowMock = new Mock<IReadRepoCollection>();
        var handler = new GetByIdQueryHandler(uowMock.Object);
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();
        var query = new GetByIdQuery { Type = ProductCategories.Gpu, Id = "123" };

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => handler.Handle(query, cts.Token));
    }
}