using Autofac.Features.Indexed;
using GoodStuff_DomainModels.Models.Enums;

namespace GoodStuff.ProductApi.Application.Services;

public class ProductDaoFactory(IIndex<ProductCategories, IProductDao> daoCollection) : IProductDaoFactory
{
    public IProductDao? GetProductDao(ProductCategories type)
    {
        return daoCollection.TryGetValue(type, out var dao) ? dao : null;
    }
}