using GoodStuff_DomainModels.Models.Products;
using GoodStuff.ProductApi.Application.Interfaces;
using Microsoft.Azure.Cosmos;

namespace GoodStuff.ProductApi.Infrastructure.Repository;

public class CosmosRepository<T>(CosmosClient cosmosClient) : IRepository
{
    private readonly Container _container = cosmosClient.GetContainer("GoodStuff", "Products");
    public async Task<IEnumerable<BaseProductModel>> GetAllByTypeAsync(string category)
    {
        var query = QueryBuilder.SelectAllProductsByType(category);
        var iterator = _container.GetItemQueryIterator<T>(queryDefinition: query);
        var results = new List<T>();
        while (iterator.HasMoreResults)
        {
            var item = await iterator.ReadNextAsync();
            results.AddRange(item.Resource);
        }
        return (IEnumerable<BaseProductModel>)results;
    }

    public Task<BaseProductModel> GetSingleById(string category, string id)
    {
        throw new NotImplementedException();
    }

    public Task<BaseProductModel> CreateAsync(BaseProductModel entity)
    {
        throw new NotImplementedException();
    }

    public Task<BaseProductModel> UpdateAsync(BaseProductModel entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(BaseProductModel entity)
    {
        throw new NotImplementedException();
    }
}