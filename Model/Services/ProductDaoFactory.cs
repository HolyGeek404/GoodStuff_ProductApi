using Autofac.Features.Indexed;
using GoodStuff_DomainModels.Models.Enums;
using Model.DataAccess;

namespace Model.Services;

public class ProductDaoFactory(IIndex<ProductCategories, IProductDao> daoCollection) : IProductDaoFactory
{
    public IProductDao? GetProductDao(ProductCategories type)
    {
        return daoCollection.TryGetValue(type, out var dao) ? dao : null;
    }
}