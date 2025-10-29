using System.Net;
using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Domain.Products.Models;
using Microsoft.Azure.Cosmos;

namespace GoodStuff.ProductApi.Infrastructure.Repositories;

public class CosmosRepository<TProduct>(CosmosClient cosmosClient) : IReadRepository<TProduct>, IWriteRepository<TProduct>
{
    private readonly Container _container = cosmosClient.GetContainer("GoodStuff", "Products");

    public async Task<IEnumerable<TProduct>> GetByType(string category)
    {
        var query = QueryBuilder.SelectAllProductsByType(category);
        var iterator = _container.GetItemQueryIterator<TProduct>(query);
        var results = new List<TProduct>();
        while (iterator.HasMoreResults)
        {
            var item = await iterator.ReadNextAsync();
            results.AddRange(item.Resource);
        }
        
        return results;
    }

    public async Task<TProduct?> GetById(string category, string id)
    {
        var query = QueryBuilder.SelectSingleProductById(category, id);
        var iterator = _container.GetItemQueryIterator<TProduct>(query);
        var results = await iterator.ReadNextAsync();
        return (TProduct)results.Resource.First()!;
    }

    public async Task CreateAsync(TProduct entity)
    {
        await _container.CreateItemAsync(entity);
    }

    public async Task<HttpStatusCode> UpdateAsync(TProduct entity, string id, string pk)
    {
        var partitionKey = new PartitionKey(pk);
        var result = await _container.ReplaceItemAsync(entity, id,partitionKey);
        return result.StatusCode;
    }

    public async Task DeleteAsync(string id, string partitionKey)
    {
        await _container.DeleteItemAsync<TProduct>(id, new PartitionKey(partitionKey));
    }
}