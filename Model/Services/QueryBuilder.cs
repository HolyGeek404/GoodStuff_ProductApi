using Microsoft.Azure.Cosmos;
using Model.DataAccess;

namespace Model.Services;

public static class QueryBuilder
{
    public static QueryDefinition SelectAllProductsByType(string type)
    {
        return new QueryDefinition(Queries.GetAllByType).WithParameter("@category", type);
    }

    public static QueryDefinition SelectSingleProductById(string type, string id)
    {
        return new QueryDefinition(Queries.GetSingleById).WithParameter("@category", type).WithParameter("@id",id);
    }
}