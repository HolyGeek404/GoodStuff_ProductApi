using Microsoft.Azure.Cosmos;
using Model.Services;
namespace Model.DataAccess;

public class ProductDao(CosmosClient cosmosClient) : IProductDao
{
    private async Task<object?> Get(QueryDefinition queryDefinition, QueryRequestOptions queryRequestOptions)
    {
        var container = cosmosClient.GetContainer("GoodStuff", "Products");
        using var iterator = container.GetItemQueryIterator<Dictionary<string, object>>(queryDefinition, requestOptions: queryRequestOptions);
        var response = await iterator.ReadNextAsync();

        if (response.Count == 0)
        {
            return null;
        }
        return response;
    }

    public async Task<object?> GetAllProductsByType(string type)
    {
        var query = QueryBuilder.SelectAllProductsByType(type);
        var queryOptions = new QueryRequestOptions
        {
            PartitionKey = new PartitionKey(type)
        };

        return await Get(query, queryOptions);
    }

    public async Task<object?> GetProductByIdQuery(string type, int id)
    {
        var query = QueryBuilder.SelectSingleProductById(type, id);
        var queryOptions = new QueryRequestOptions
        {
            PartitionKey = new PartitionKey(type)
        };

        return await Get(query, queryOptions);
    }
}