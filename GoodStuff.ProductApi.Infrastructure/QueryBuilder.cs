using GoodStuff_DomainModels.Models.Enums;
using GoodStuff.ProductApi.Domain;
using Microsoft.Azure.Cosmos;

namespace GoodStuff.ProductApi.Infrastructure;

public static class QueryBuilder
{
    public static QueryDefinition SelectAllProductsByType(ProductCategories type)
    {
        return new QueryDefinition(Queries.GetAllByType).WithParameter("@category", Enum.GetName(type)?.ToUpper());
    }

    public static QueryDefinition SelectSingleProductById(ProductCategories type, string id)
    {
        return new QueryDefinition(Queries.GetSingleById).WithParameter("@category", Enum.GetName(type)?.ToUpper())
            .WithParameter("@id", id);
    }
}