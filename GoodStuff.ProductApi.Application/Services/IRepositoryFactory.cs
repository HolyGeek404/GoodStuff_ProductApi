using GoodStuff.ProductApi.Application.Interfaces;

namespace GoodStuff.ProductApi.Application.Services;

public interface IRepositoryFactory
{
    IRepository? GetRepository(string category);
}