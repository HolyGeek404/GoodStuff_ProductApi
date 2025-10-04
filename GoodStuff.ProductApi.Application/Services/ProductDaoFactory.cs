using Autofac.Features.Indexed;

namespace GoodStuff.ProductApi.Application.Services;

public class ProductDaoFactory(IIndex<string, IProductDao> daoCollection) : IProductDaoFactory
{
    public IProductDao? GetProductDao(string category)
    {
        return daoCollection.TryGetValue(category, out var dao) ? dao : null;
    }
}