using GoodStuff.ProductApi.Application.Features.Product.Queries.GetByType;
using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Application.Services;
using GoodStuff.ProductApi.Domain.Products;
using GoodStuff.ProductApi.Domain.Products.Models;
using Moq;

namespace GoodStuff.ProductApi.Application.Tests.Queries;

public class GetByTypeQueryHandlerTest
{
    private readonly Mock<IReadRepository<Gpu>> _gpuRepoMock = new();
    private readonly Mock<IReadRepository<Cpu>> _cpuRepoMock = new();
    private readonly Mock<IReadRepository<Cooler>> _coolerRepoMock = new();
    private readonly IReadRepoCollection _uow;

    public GetByTypeQueryHandlerTest()
    {
        _uow = new ReadRepoCollection(_cpuRepoMock.Object, _gpuRepoMock.Object, _coolerRepoMock.Object);

    }
    
    [Fact]
    public async Task Handle_WhenTypeIsGpu_CallsGpuRepositoryAndReturnsResult()
    {
        // Arrange
        var listOfGpus = new List<Gpu>();
        var gpu = new Gpu
        {
            Name = "Test GPU",
            Category =  ProductCategories.Gpu,
            Team = "AMD",
            Price = "3900",
            Id = "123",
            ProductId = "321",
            Warranty = "5 Years",
            ProducerCode = "ZXC123"
        };
        listOfGpus.Add(gpu);
        _gpuRepoMock.Setup(r => r.GetByType(ProductCategories.Gpu)).ReturnsAsync(listOfGpus);
        var handler = new GetByTypeQueryHandler(_uow);

        var query = new GetByTypeQuery
        {
            Type = ProductCategories.Gpu
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        var typedResult = Assert.IsType<IEnumerable<Gpu>>(result, exactMatch: false);
        Assert.Equal(listOfGpus, typedResult);
        _gpuRepoMock.Verify(r => r.GetByType(ProductCategories.Gpu), Times.Once);
        _cpuRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
        _coolerRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_WhenTypeIsCpu_CallsCpuRepositoryAndReturnsResult()
    {
        // Arrange
        var listOfCpus = new List<Cpu>();
        var gpu = new Cpu
        {
            Name = "Test CPU",
            Category =  ProductCategories.Cpu,
            Team = "AMD",
            Price = "3900",
            Id = "123",
            ProductId = "321",
            Warranty = "5 Years",
            ProducerCode = "ZXC123"
        };
        listOfCpus.Add(gpu);
        _cpuRepoMock.Setup(r => r.GetByType(ProductCategories.Cpu)).ReturnsAsync(listOfCpus);
        var handler = new GetByTypeQueryHandler(_uow);
        var query = new GetByTypeQuery
        {
            Type = ProductCategories.Cpu
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        var typedResult = Assert.IsType<IEnumerable<Cpu>>(result, exactMatch: false);
        Assert.Equal(listOfCpus, typedResult);
        _gpuRepoMock.Verify(r => r.GetByType(ProductCategories.Cpu), Times.Never);
        _cpuRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Once);
        _coolerRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_WhenTypeIsCooler_CallsCoolerRepositoryAndReturnsResult()
    {
        // Arrange
        var listOfCoolers = new List<Cooler>();
        var cooler = new Cooler
        {
            Name = "Test Cooler",
            Category = ProductCategories.Cooler,
            Team = "Noctua",
            Price = "120",
            Id = "456",
            ProductId = "654",
            Warranty = "6 Years",
            ProducerCode = "COOL123"
        };
        listOfCoolers.Add(cooler);
        _coolerRepoMock.Setup(r => r.GetByType(ProductCategories.Cooler)).ReturnsAsync(listOfCoolers);
        var handler = new GetByTypeQueryHandler(_uow);
        var query = new GetByTypeQuery
        {
            Type = ProductCategories.Cooler
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        var typedResult = Assert.IsType<IEnumerable<Cooler>>(result, exactMatch: false);
        Assert.Equal(listOfCoolers, typedResult);
        _coolerRepoMock.Verify(r => r.GetByType(ProductCategories.Cooler), Times.Once);
        _gpuRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
        _cpuRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_WhenTypeIsNotSupported_DoesNotCallAnyRepositoryAndReturnsNull()
    {
        // Arrange
        var handler = new GetByTypeQueryHandler(_uow);
        var query = new GetByTypeQuery
        {
            Type = "unknown"
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _coolerRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
        _gpuRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
        _cpuRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
    }
}