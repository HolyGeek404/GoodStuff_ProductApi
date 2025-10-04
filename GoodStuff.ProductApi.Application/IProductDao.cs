using GoodStuff_DomainModels.Models.Enums;

namespace GoodStuff.ProductApi.Application;

public interface IProductDao
{
    Task<object?> GetAllProductsByType(ProductCategories type);
    Task<object?> GetProductByIdQuery(ProductCategories type, string id);
}