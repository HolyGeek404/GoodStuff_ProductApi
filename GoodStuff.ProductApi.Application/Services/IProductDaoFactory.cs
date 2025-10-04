using GoodStuff_DomainModels.Models.Enums;

namespace GoodStuff.ProductApi.Application.Services;

public interface IProductDaoFactory
{
    IProductDao? GetProductDao(ProductCategories type);
}