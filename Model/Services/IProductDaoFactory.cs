using Model.DataAccess;

namespace Model.Services;

public interface IProductDaoFactory
{
    IProductDao? GetProductDao(string type);
}