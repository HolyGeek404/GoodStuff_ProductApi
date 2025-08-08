using Microsoft.Azure.Cosmos;
namespace Model.DataAccess;

public abstract class BaseProductDao(CosmosClient cosmosClient)
{
    protected async Task<object?> Get<T>(QueryDefinition queryDefinition, string type)
    {
        var container = cosmosClient.GetContainer("GoodStuff", "Products");
        var queryRequestOptions = new QueryRequestOptions
        {
            PartitionKey = new PartitionKey(type)
        };
        using var iterator = container.GetItemQueryIterator<T>(queryDefinition, requestOptions: queryRequestOptions);
        var response = await iterator.ReadNextAsync();  

        return response.Count == 0 ? null : response.Resource;
    }
    protected async Task<object?> GetList<T>(QueryDefinition queryDefinition, string type)
    {
        var container = cosmosClient.GetContainer("GoodStuff", "Products");
        var queryRequestOptions = new QueryRequestOptions
        {
            PartitionKey = new PartitionKey(type)
        };
        using var iterator = container.GetItemQueryIterator<T>(queryDefinition, requestOptions: queryRequestOptions);
        var results = new List<T>();

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response.Resource);
        }
        return results;
    }
}