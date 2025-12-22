using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
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

    
    [Theory]
    [MemberData(nameof(UpdateProductData))]
    public async Task Update_ShouldUpdateProduct_ReturnsOk(string category, string productId, JsonElement product)
    {
        // Arrange
        Authenticate("Update");
        
        var productNode = JsonNode.Parse(product.GetRawText())!.AsObject();
        var expectedName = Guid.NewGuid().ToString();
        productNode["Name"] = expectedName;
        var jsonContent = new StringContent(productNode.ToJsonString(), Encoding.UTF8, "application/json");
        
        // Act
        var responsePatch = await _client.PatchAsync($"/Product/{category}",jsonContent);
        var responseGet = await _client.GetAsync($"/Product/{category}/{productId}");
        
        var jsonString = await responseGet.Content.ReadAsStringAsync();
        var rootNode = JsonNode.Parse(jsonString)!;
        var nameResult = rootNode["name"]!.GetValue<string>();
        
        // Assert
        responsePatch.EnsureSuccessStatusCode();
        responseGet.EnsureSuccessStatusCode();
        Assert.Equal(expectedName, nameResult);
    }

    [Fact]
    public async Task Create_ShouldCreateProduct_ReturnsOk()
    {
        Authenticate("Create");
        var product = ProductFactory.CreateTestCooler();
        StringContent jsonContent = new(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");   
        var responsePatch = await _client.PostAsync($"/Product/{ProductCategories.Cooler}",jsonContent);
        
    }

    public static TheoryData<string, string, JsonElement> UpdateProductData =>
        new()
        {
            { ProductCategories.Cpu, "-1", ProductFactory.CreateTestCpu() },
            { ProductCategories.Gpu, "-2", ProductFactory.CreateTestGpu() },
            { ProductCategories.Cooler, "-3", ProductFactory.CreateTestCooler() },
        };

    
    private void Authenticate(string role = "Get")
    {
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Test");

        _client.DefaultRequestHeaders.Remove("Role");
        _client.DefaultRequestHeaders.Add("Role", role);
    }
}