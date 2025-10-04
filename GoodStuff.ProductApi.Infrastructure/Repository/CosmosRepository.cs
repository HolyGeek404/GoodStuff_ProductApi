
using GoodStuff.ProductApi.Application.Interfaces;
using Microsoft.Azure.Cosmos;

namespace GoodStuff.ProductApi.Infrastructure.Repository;

public class CosmosRepository<T>(CosmosClient cosmosClient) : IRepository<T> where T : class
{
    private readonly Container _container = cosmosClient.GetContainer("GoodStuff", "Products");

    public async Task<IEnumerable<T>> GetAllByTypeAsync(string category)
    {
        var query = QueryBuilder.SelectAllProductsByType(category);
        var iterator = _container.GetItemQueryIterator<T>(queryDefinition: query);
        var results = new List<T>();
        while (iterator.HasMoreResults)
        {
            var item = await iterator.ReadNextAsync();
            results.AddRange(item.Resource);
        }
        return results;
    }

    public Task<T> GetSingleById(string category, string id)
    {
        throw new NotImplementedException();
    }

    public Task<T> CreateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<T> UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(T entity)
    {
        throw new NotImplementedException();
    }
}