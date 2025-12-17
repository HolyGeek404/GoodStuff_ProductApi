using GoodStuff.ProductApi.Application.Features.Product.Queries.GetByType;
using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Domain.Products;
using GoodStuff.ProductApi.Domain.Products.Models;
using Moq;

namespace GoodStuff.ProductApi.Application.Tests.Queries;

public class GetByTypeQueryHandlerTest
{
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
    
        var gpuRepoMock = new Mock<IReadRepository<Gpu>>();
        var cpuRepoMock = new Mock<IReadRepository<Cpu>>();
        var coolerRepoMock = new Mock<IReadRepository<Cooler>>();
        gpuRepoMock.Setup(r => r.GetByType(ProductCategories.Gpu)).ReturnsAsync(listOfGpus);

        var uowMock = new Mock<IReadRepoCollection>();
        uowMock.SetupGet(x => x.GpuRepository).Returns(gpuRepoMock.Object);
        uowMock.SetupGet(x => x.CpuRepository).Returns(cpuRepoMock.Object);
        uowMock.SetupGet(x => x.CoolerRepository).Returns(coolerRepoMock.Object);

        var handler = new Features.Product.Queries.GetByType.GetByTypeQueryHandler(uowMock.Object);

        var query = new GetByTypeQuery
        {
            Type = ProductCategories.Gpu
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        var typedResult = Assert.IsType<IEnumerable<Gpu>>(result, exactMatch: false);
        Assert.Equal(listOfGpus, typedResult);
        gpuRepoMock.Verify(r => r.GetByType(ProductCategories.Gpu), Times.Once);
        cpuRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
        coolerRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
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
    
        var gpuRepoMock = new Mock<IReadRepository<Gpu>>();
        var cpuRepoMock = new Mock<IReadRepository<Cpu>>();
        var coolerRepoMock = new Mock<IReadRepository<Cooler>>();
        cpuRepoMock.Setup(r => r.GetByType(ProductCategories.Cpu)).ReturnsAsync(listOfCpus);

        var uowMock = new Mock<IReadRepoCollection>();
        uowMock.SetupGet(x => x.GpuRepository).Returns(gpuRepoMock.Object);
        uowMock.SetupGet(x => x.CpuRepository).Returns(cpuRepoMock.Object);
        uowMock.SetupGet(x => x.CoolerRepository).Returns(coolerRepoMock.Object);

        var handler = new GetByTypeQueryHandler(uowMock.Object);

        var query = new GetByTypeQuery
        {
            Type = ProductCategories.Cpu
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        var typedResult = Assert.IsType<IEnumerable<Cpu>>(result, exactMatch: false);
        Assert.Equal(listOfCpus, typedResult);
        gpuRepoMock.Verify(r => r.GetByType(ProductCategories.Cpu), Times.Never);
        cpuRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Once);
        coolerRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
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

        var gpuRepoMock = new Mock<IReadRepository<Gpu>>();
        var cpuRepoMock = new Mock<IReadRepository<Cpu>>();
        var coolerRepoMock = new Mock<IReadRepository<Cooler>>();

        coolerRepoMock.Setup(r => r.GetByType(ProductCategories.Cooler)).ReturnsAsync(listOfCoolers);

        var uowMock = new Mock<IReadRepoCollection>();
        uowMock.SetupGet(x => x.GpuRepository).Returns(gpuRepoMock.Object);
        uowMock.SetupGet(x => x.CpuRepository).Returns(cpuRepoMock.Object);
        uowMock.SetupGet(x => x.CoolerRepository).Returns(coolerRepoMock.Object);

        var handler = new GetByTypeQueryHandler(uowMock.Object);

        var query = new GetByTypeQuery
        {
            Type = ProductCategories.Cooler
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        var typedResult = Assert.IsType<IEnumerable<Cooler>>(result, exactMatch: false);
        Assert.Equal(listOfCoolers, typedResult);
        coolerRepoMock.Verify(r => r.GetByType(ProductCategories.Cooler), Times.Once);
        gpuRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
        cpuRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_WhenTypeIsNotSupported_DoesNotCallAnyRepositoryAndReturnsNull()
    {
        // Arrange
        var gpuRepoMock = new Mock<IReadRepository<Gpu>>();
        var cpuRepoMock = new Mock<IReadRepository<Cpu>>();
        var coolerRepoMock = new Mock<IReadRepository<Cooler>>();
        
        var uowMock = new Mock<IReadRepoCollection>();
        uowMock.SetupGet(x => x.GpuRepository).Returns(gpuRepoMock.Object);
        uowMock.SetupGet(x => x.CpuRepository).Returns(cpuRepoMock.Object);
        uowMock.SetupGet(x => x.CoolerRepository).Returns(coolerRepoMock.Object);

        var handler = new GetByTypeQueryHandler(uowMock.Object);

        var query = new GetByTypeQuery
        {
            Type = "unknown"
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
        coolerRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
        gpuRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
        cpuRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
    }
}