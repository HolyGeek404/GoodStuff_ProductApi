using Microsoft.Azure.Cosmos;
namespace Model.DataAccess;

public class ProductDao(CosmosClient cosmosClient) : IProductDao
{
    public async Task<object?> GetAllProductsByType(string type)
    {
        var container = cosmosClient.GetContainer("GoodStuff", "Products");
        var query = new QueryDefinition("SELECT * FROM c WHERE c.Category = @category")
            .WithParameter("@category", type);

        var queryOptions = new QueryRequestOptions
        {
            PartitionKey = new PartitionKey(type)
        };

        using var iterator = container.GetItemQueryIterator<Dictionary<string, object>>(query, requestOptions: queryOptions);
        var response = await iterator.ReadNextAsync();

        if (response.Count == 0)
        {
            return null;
        }
        return response;
    }
}