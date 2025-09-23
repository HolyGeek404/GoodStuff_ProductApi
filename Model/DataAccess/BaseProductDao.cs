using GoodStuff_DomainModels.Models.Enums;
using Microsoft.Azure.Cosmos;

namespace Model.DataAccess;

public abstract class BaseProductDao(CosmosClient cosmosClient)
{
    protected async Task<T?> Get<T>(QueryDefinition queryDefinition, ProductCategories type)
    {
        var container = cosmosClient.GetContainer("GoodStuff", "Products");
        var queryRequestOptions = new QueryRequestOptions
        {
            PartitionKey = new PartitionKey(Enum.GetName(type).ToUpper())
        };
        using var iterator = container.GetItemQueryIterator<T>(queryDefinition, requestOptions: queryRequestOptions);
        var response = await iterator.ReadNextAsync();

        return response.Count == 0 ? default : response.Resource.FirstOrDefault();
    }

    protected async Task<List<T>?> GetList<T>(QueryDefinition queryDefinition, ProductCategories type)
    {
        var container = cosmosClient.GetContainer("GoodStuff", "Products");
        var queryRequestOptions = new QueryRequestOptions
        {
            PartitionKey = new PartitionKey(Enum.GetName(type).ToUpper())
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