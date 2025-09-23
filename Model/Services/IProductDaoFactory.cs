using GoodStuff_DomainModels.Models.Enums;
using Model.DataAccess;

namespace Model.Services;

public interface IProductDaoFactory
{
    IProductDao? GetProductDao(ProductCategories type);
}