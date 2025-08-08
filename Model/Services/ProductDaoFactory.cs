using Autofac.Features.Indexed;
using Model.DataAccess;

namespace Model.Services;

public class ProductDaoFactory(IIndex<string, IProductDao> daoCollection) : IProductDaoFactory
{
    public IProductDao? GetProductDao(string type)
    {
        return daoCollection.TryGetValue(type, out var dao) ? dao : null;
    }
}