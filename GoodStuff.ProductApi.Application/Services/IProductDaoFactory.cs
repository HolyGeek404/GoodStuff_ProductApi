namespace GoodStuff.ProductApi.Application.Services;

public interface IProductDaoFactory
{
    IProductDao? GetProductDao(string category);
}