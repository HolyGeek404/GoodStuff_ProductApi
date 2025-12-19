using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using GoodStuff.ProductApi.Domain.Products;
using GoodStuff.ProductApi.Domain.Products.Models;
using GoodStuff.ProductApi.Presentation.Tests.Helpers;

namespace GoodStuff.ProductApi.Presentation.Tests.Controllers;

public class ProductControllerTests(TestingWebAppFactory factory) : IClassFixture<TestingWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Theory]
    [InlineData(ProductCategories.Cpu)]
    [InlineData(ProductCategories.Gpu)]
    [InlineData(ProductCategories.Cooler)]
    public async Task GetByType_SupportedType_ReturnsListOfProducts(string category)
    {
        // Arrange
        Authenticate();
        
        // Act
        var response = await _client.GetAsync($"/Product/{category}");
        var content = await response.Content.ReadFromJsonAsync<List<BaseProduct>>();
        
        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(content);
        Assert.NotEmpty(content);

    }
    
    [Theory]
    [InlineData(ProductCategories.Cpu, "6", "Ryzen 9 5900X")]
    [InlineData(ProductCategories.Gpu, "7", "Inno3D GeForce RTX 4070 Ti X3 12GB GDDR6X")]
    [InlineData(ProductCategories.Cooler, "9", "Noctua NH-D15")]
    public async Task GetById_ExistingProduct_ReturnsProduct(string category, string id, string expectedName)
    {
        // Arrange
        Authenticate();
        
        // Act
        var response = await _client.GetAsync($"/Product/{category}/{id}");
        var content = await response.Content.ReadFromJsonAsync<BaseProduct>();
        
        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(content);
        Assert.Equal(expectedName, content.Name);
    }

    [Fact]
    public async Task Update_ShouldUpdateProduct_ReturnsOk()
    {
        // Arrange
        Authenticate("Update");
        var newName = Guid.NewGuid().ToString();
        var product = GetTestGpu();
        product.Name = newName;
        const string productId = "11";
        StringContent jsonContent = new(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");   
        
        // Act
        var responsePatch = await _client.PatchAsync($"/Product/{ProductCategories.Gpu}",jsonContent);
        var responseGet = await _client.GetAsync($"/Product/{ProductCategories.Gpu}/{productId}");
        var content = await responseGet.Content.ReadFromJsonAsync<Gpu>();
        
        // Assert
        responsePatch.EnsureSuccessStatusCode();
        responseGet.EnsureSuccessStatusCode();
        Assert.NotNull(content);
        Assert.Equal(newName, content.Name);
    }

    
    private void Authenticate(string role = "Get")
    {
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Test");

        _client.DefaultRequestHeaders.Remove("Role");
        _client.DefaultRequestHeaders.Add("Role", role);
    }
    private static Gpu GetTestGpu() => new()
    {
        Id = "0a5112a8-fa42-42a0-9fd4-2ed8cd817594",
        ProductId = "11",
        Category = ProductCategories.Gpu,
        Name = "TEST",
        Price = "132",
        Team = "TEST",
        Manufacturer = "TEST",
        ProductImg = "~/defaultProductImg.jpg",
        Warranty = "TEST",
        GpuProcessorLine = "TEST",
        GpuProcessorName = "TEST",
        PcieType = "TEST",
        MemorySize = "TEST",
        MemoryType = "TEST",
        MemoryBus = "TEST",
        MemoryRatio = "TEST",
        CoreRatio = "TEST",
        CoresNumber = "TEST",
        CoolingType = "TEST",
        OutputsType = "TEST",
        SupportedLibraries = "TEST",
        PowerConnector = "TEST",
        RecommendedPsuPower = "TEST",
        Length = "TEST",
        Width = "TEST",
        Height = "TEST",
        ProducerCode = "Test"
    };
}