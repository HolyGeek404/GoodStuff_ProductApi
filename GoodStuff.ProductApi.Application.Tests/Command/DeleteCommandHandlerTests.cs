using System.Net;
using GoodStuff.ProductApi.Application.Features.Product.Commands.Delete;
using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Domain.Products;
using GoodStuff.ProductApi.Domain.Products.Models;
using Moq;

namespace GoodStuff.ProductApi.Application.Tests.Command
{
    public class DeleteCommandHandlerTests
    {
        [Theory]
        [InlineData(ProductCategories.Gpu)]
        [InlineData(ProductCategories.Cpu)]
        [InlineData(ProductCategories.Cooler)]
        public async Task Handle_WhenTypeIsSupported_CallsCorrectRepository(string type)
        {
            // Arrange
            var request = new DeleteCommand { Type = type, Id = Guid.NewGuid() };
            var gpuRepoMock = new Mock<IWriteRepository<Gpu>>();
            var cpuRepoMock = new Mock<IWriteRepository<Cpu>>();
            var coolerRepoMock = new Mock<IWriteRepository<Cooler>>();

            gpuRepoMock.Setup(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(HttpStatusCode.OK);
            cpuRepoMock.Setup(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(HttpStatusCode.OK);
            coolerRepoMock.Setup(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(HttpStatusCode.OK);

            var uowMock = new Mock<IWriteRepoCollection>();
            uowMock.SetupGet(x => x.GpuRepository).Returns(gpuRepoMock.Object);
            uowMock.SetupGet(x => x.CpuRepository).Returns(cpuRepoMock.Object);
            uowMock.SetupGet(x => x.CoolerRepository).Returns(coolerRepoMock.Object);

            var handler = new DeleteCommandHandler(uowMock.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result);

            switch (type)
            {
                case ProductCategories.Gpu:
                    gpuRepoMock.Verify(r => r.DeleteAsync(request.Id, type), Times.Once);
                    cpuRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
                    coolerRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
                    break;
                case ProductCategories.Cpu:
                    cpuRepoMock.Verify(r => r.DeleteAsync(request.Id, type), Times.Once);
                    gpuRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
                    coolerRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
                    break;
                case ProductCategories.Cooler:
                    coolerRepoMock.Verify(r => r.DeleteAsync(request.Id, type), Times.Once);
                    gpuRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
                    cpuRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
                    break;
            }
        }

        [Fact]
        public async Task Handle_WhenTypeIsUnsupported_ReturnsBadRequest()
        {
            // Arrange
            var request = new DeleteCommand { Type = "RAM", Id = Guid.NewGuid() };
            var uowMock = new Mock<IWriteRepoCollection>();
            var handler = new DeleteCommandHandler(uowMock.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result);
        }

        [Fact]
        public async Task Handle_WhenCancellationRequested_ThrowsOperationCanceledException()
        {
            // Arrange
            var request = new DeleteCommand { Type = ProductCategories.Gpu, Id = Guid.NewGuid() };
            var uowMock = new Mock<IWriteRepoCollection>();
            var handler = new DeleteCommandHandler(uowMock.Object);

            using var cts = new CancellationTokenSource();
            await cts.CancelAsync();

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(() => handler.Handle(request, cts.Token));
        }
    }
}
