using GoodStuff.ProductApi.Application.Features.Product.Queries.GetById;
using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Application.Services;
using GoodStuff.ProductApi.Domain.Products;
using GoodStuff.ProductApi.Domain.Products.Models;
using Moq;

namespace GoodStuff.ProductApi.Application.Tests.Queries;

public class GetByIdQueryHandlerTests
{
    private readonly Mock<IReadRepository<Gpu>> _gpuRepoMock = new();
    private readonly Mock<IReadRepository<Cpu>> _cpuRepoMock = new();
    private readonly Mock<IReadRepository<Cooler>> _coolerRepoMock = new();
    private readonly IReadRepoCollection _uow;

    public GetByIdQueryHandlerTests()
    {
        _uow = new ReadRepoCollection(_cpuRepoMock.Object, _gpuRepoMock.Object, _coolerRepoMock.Object);
    }
    
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
        _gpuRepoMock.Setup(r => r.GetById(ProductCategories.Gpu, "321")).ReturnsAsync(gpu);
        var handler = new GetByIdQueryHandler(_uow);
        var query = new GetByIdQuery { Type = ProductCategories.Gpu, Id = "321" };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        var typedResult = Assert.IsType<Gpu>(result);
        Assert.Equal(gpu, typedResult);
        _gpuRepoMock.Verify(r => r.GetById(ProductCategories.Gpu, "321"), Times.Once);
        _cpuRepoMock.Verify(r => r.GetById(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _coolerRepoMock.Verify(r => r.GetById(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
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
        _cpuRepoMock.Setup(r => r.GetById(ProductCategories.Cpu, "654")).ReturnsAsync(cpu);
        var handler = new GetByIdQueryHandler(_uow);
        var query = new GetByIdQuery { Type = ProductCategories.Cpu, Id = "654" };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        var typedResult = Assert.IsType<Cpu>(result);
        Assert.Equal(cpu, typedResult);
        _cpuRepoMock.Verify(r => r.GetById(ProductCategories.Cpu, "654"), Times.Once);
        _gpuRepoMock.Verify(r => r.GetById(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _coolerRepoMock.Verify(r => r.GetById(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
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
        _coolerRepoMock.Setup(r => r.GetById(ProductCategories.Cooler, "987")).ReturnsAsync(cooler);
        var handler = new GetByIdQueryHandler(_uow);
        var query = new GetByIdQuery { Type = ProductCategories.Cooler, Id = "987" };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        var typedResult = Assert.IsType<Cooler>(result);
        Assert.Equal(cooler, typedResult);
        _coolerRepoMock.Verify(r => r.GetById(ProductCategories.Cooler, "987"), Times.Once);
        _gpuRepoMock.Verify(r => r.GetById(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _cpuRepoMock.Verify(r => r.GetById(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
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
        var handler = new GetByIdQueryHandler(_uow);
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();
        var query = new GetByIdQuery { Type = ProductCategories.Gpu, Id = "123" };

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => handler.Handle(query, cts.Token));
    }
}