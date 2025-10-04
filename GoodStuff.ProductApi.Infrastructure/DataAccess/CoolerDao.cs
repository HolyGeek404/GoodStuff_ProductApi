using GoodStuff_DomainModels.Models.Products;
using GoodStuff.ProductApi.Application;
using Microsoft.Azure.Cosmos;

namespace GoodStuff.ProductApi.Infrastructure.DataAccess;

public class CoolerDao(CosmosClient cosmosClient) : BaseProductDao(cosmosClient), IProductDao
{
    public async Task<object?> GetAllProductsByType(ProductCategories type)
    {
        var query = QueryBuilder.SelectAllProductsByType(type);
        return await GetList<CoolerModel>(query, type);
    }

    public async Task<object?> GetProductByIdQuery(ProductCategories type, string id)
    {
        var query = QueryBuilder.SelectSingleProductById(type, id);
        return await Get<CoolerModel>(query, type);
    }
}