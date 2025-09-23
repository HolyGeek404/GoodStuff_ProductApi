using GoodStuff_DomainModels.Models.Enums;
using GoodStuff_DomainModels.Models.Products;
using Microsoft.Azure.Cosmos;
using Model.Services;

namespace Model.DataAccess;

public class CpuDao(CosmosClient cosmosClient) : BaseProductDao(cosmosClient), IProductDao
{
    public async Task<object?> GetAllProductsByType(ProductCategories type)
    {
        var query = QueryBuilder.SelectAllProductsByType(type);
        return await GetList<CpuModel>(query, type);
    }

    public async Task<object?> GetProductByIdQuery(ProductCategories type, string id)
    {
        var query = QueryBuilder.SelectSingleProductById(type, id);
        return await Get<CpuModel>(query, type);
    }
}