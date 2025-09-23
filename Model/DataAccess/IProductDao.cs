using GoodStuff_DomainModels.Models.Enums;

namespace Model.DataAccess;

public interface IProductDao
{
    Task<object?> GetAllProductsByType(ProductCategories type);
    Task<object?> GetProductByIdQuery(ProductCategories type, string id);
}