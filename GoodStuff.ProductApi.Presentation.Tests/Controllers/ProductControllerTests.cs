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
    [InlineData(ProductCategories.Cpu, "db7fcee5-da4f-45ec-9ad0-ce92e57fd2b2", "Ryzen 9 5900X")]
    [InlineData(ProductCategories.Gpu, "2db725bb-cc8b-4b3f-8184-ea25a004396e", "Inno3D GeForce RTX 4070 Ti X3 12GB GDDR6X")]
    [InlineData(ProductCategories.Cooler, "11556f4f-3d9e-4343-99bd-42941e4186fd", "Noctua NH-D15")]
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
    [MemberData(nameof(ProductData))]
    public async Task Update_ShouldUpdateProduct_ReturnsOk(string category, JsonElement product)
    {
        // Arrange
        Authenticate("Update");
        
        var productNode = JsonNode.Parse(product.GetRawText())!.AsObject();
        var expectedName = Guid.NewGuid().ToString();
        productNode["Name"] = expectedName;
        var jsonContent = new StringContent(productNode.ToJsonString(), Encoding.UTF8, "application/json");
        
        // Act
        var responsePatch = await _client.PatchAsync($"/Product/{category}",jsonContent);
        var responseGet = await _client.GetAsync($"/Product/{category}/{productNode["id"]}");
        
        var jsonString = await responseGet.Content.ReadAsStringAsync();
        var rootNode = JsonNode.Parse(jsonString)!;
        var nameResult = rootNode["name"]!.GetValue<string>();
        
        // Assert
        responsePatch.EnsureSuccessStatusCode();
        responseGet.EnsureSuccessStatusCode();
        Assert.Equal(expectedName, nameResult);
    }

    [Theory]
    [MemberData(nameof(ProductData))]
    public async Task CreateAndDelete_ShouldCreateAndDeleteProduct_ReturnsOk(string category, JsonElement product)
    {
        // Arrange 
        Authenticate("Create");
        var id = new Guid("11111111-0000-3333-4444-555555555555");
        
        var productNode = JsonNode.Parse(product.GetRawText())!.AsObject();
        productNode["id"] = id;
        StringContent jsonContent = new(JsonSerializer.Serialize(productNode), Encoding.UTF8, "application/json");   
        
        // Act
        var responsePatch = await _client.PostAsync($"/Product/{category}",jsonContent);
        Authenticate();
        var responseGet = await _client.GetAsync($"/Product/{category}/{id}");
        Authenticate("Delete");
        var responseDelete = await _client.DeleteAsync($"/Product/{category}?id={id}");
        
        // Assert
        responsePatch.EnsureSuccessStatusCode();
        responseGet.EnsureSuccessStatusCode();
        responseDelete.EnsureSuccessStatusCode();
    }
    
    // [Theory]
    // [MemberData(nameof(ProductData))]
    // public async Task CreateAndDelete_ShouldCreateProduct_ReturnsOk(string category, JsonElement product)
    // {
    //     // Arrange 
    //     Authenticate("Create");
    //     var id = new Guid("11111111-2222-3333-4444-555555555555");
    //     
    //     var productNode = JsonNode.Parse(product.GetRawText())!.AsObject();
    //     productNode["id"] = id;
    //     StringContent jsonContent = new(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");   
    //     
    //     // Act
    //     var responsePatch = await _client.PostAsync($"/Product/{ProductCategories.Cooler}",jsonContent);
    //     var responseGet = await _client.GetAsync($"/Product/{category}/{id}");
    //     var responseDelete = await _client.DeleteAsync($"/Product/{category}?id={id}");
    //     
    //     // Assert
    //     responsePatch.EnsureSuccessStatusCode();
    //     responseGet.EnsureSuccessStatusCode();
    //     responseDelete.EnsureSuccessStatusCode();
    // }

    
    public static TheoryData<string, JsonElement> ProductData => new() {
            { ProductCategories.Cpu, ProductFactory.CreateTestCpu() },
            { ProductCategories.Gpu, ProductFactory.CreateTestGpu() },
            { ProductCategories.Cooler, ProductFactory.CreateTestCooler() },
        };
    private void Authenticate(string role = "Get")
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        _client.DefaultRequestHeaders.Remove("Role");
        _client.DefaultRequestHeaders.Add("Role", role);
    }
}