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

    [Theory]
    [InlineData(ProductCategories.Gpu)]
    [InlineData(ProductCategories.Cpu)]
    [InlineData(ProductCategories.Cooler)]
    public async Task Handle_WhenTypeIsSupported_CallsCorrectRepositoryAndReturnsResult(string type)
    {
        // Arrange
        var handler = new GetByTypeQueryHandler(_uow);
        var query = new GetByTypeQuery { Type = type };

        switch (type)
        {
            case ProductCategories.Gpu:
                var gpus = new List<Gpu> { new() { Name = "Test GPU", Category = ProductCategories.Gpu, Team = "AMD", Price = "3900", Id = "123", ProductId = "321", Warranty = "5 Years", ProducerCode = "ZXC123" } };
                _gpuRepoMock.Setup(r => r.GetByType(type)).ReturnsAsync(gpus);
                
                var gpuResult = await handler.Handle(query, CancellationToken.None);
                var typedGpuResult = Assert.IsType<IEnumerable<Gpu>>(gpuResult, exactMatch: false);
                
                Assert.Equal(gpus, typedGpuResult);
                _gpuRepoMock.Verify(r => r.GetByType(type), Times.Once);
                _cpuRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
                _coolerRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
                break;

            case ProductCategories.Cpu:
                var cpus = new List<Cpu> { new() { Name = "Test CPU", Category = ProductCategories.Cpu, Team = "AMD", Price = "3900", Id = "123", ProductId = "321", Warranty = "5 Years", ProducerCode = "ZXC123" } };
                _cpuRepoMock.Setup(r => r.GetByType(type)).ReturnsAsync(cpus);
                
                var cpuResult = await handler.Handle(query, CancellationToken.None);
                var typedCpuResult = Assert.IsType<IEnumerable<Cpu>>(cpuResult, exactMatch: false);
                
                Assert.Equal(cpus, typedCpuResult);
                _cpuRepoMock.Verify(r => r.GetByType(type), Times.Once);
                _gpuRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
                _coolerRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
                break;

            case ProductCategories.Cooler:
                var coolers = new List<Cooler> { new() { Name = "Test Cooler", Category = ProductCategories.Cooler, Team = "Noctua", Price = "120", Id = "456", ProductId = "654", Warranty = "6 Years", ProducerCode = "COOL123" } };
                _coolerRepoMock.Setup(r => r.GetByType(type)).ReturnsAsync(coolers);
               
                var coolerResult = await handler.Handle(query, CancellationToken.None);
                var typedCoolerResult = Assert.IsType<IEnumerable<Cooler>>(coolerResult, exactMatch: false);
                
                Assert.Equal(coolers, typedCoolerResult);
                _coolerRepoMock.Verify(r => r.GetByType(type), Times.Once);
                _gpuRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
                _cpuRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
                break;
        }
    }

    [Fact]
    public async Task Handle_WhenTypeIsNotSupported_ReturnsNull()
    {
        // Arrange
        var handler = new GetByTypeQueryHandler(_uow);
        var query = new GetByTypeQuery { Type = "unknown" };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _gpuRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
        _cpuRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
        _coolerRepoMock.Verify(r => r.GetByType(It.IsAny<string>()), Times.Never);
    }
}