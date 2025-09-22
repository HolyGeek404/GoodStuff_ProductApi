using GoodStuff_DomainModels.Models.Products;
using Microsoft.Azure.Cosmos;
using Model.Services;

namespace Model.DataAccess;

public class CoolerDao(CosmosClient cosmosClient) : BaseProductDao(cosmosClient), IProductDao
{
    public async Task<object?> GetAllProductsByType(string type)
    {
        var query = QueryBuilder.SelectAllProductsByType(type);
        return await GetList<CoolerModel>(query, type);
    }

    public async Task<object?> GetProductByIdQuery(string type, string id)
    {
        var query = QueryBuilder.SelectSingleProductById(type, id);
        return await Get<CoolerModel>(query, type);
    }
}