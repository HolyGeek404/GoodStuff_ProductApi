using Autofac.Features.Indexed;
using GoodStuff.ProductApi.Application.Interfaces;
using GoodStuff.ProductApi.Application.Services;

namespace GoodStuff.ProductApi.Infrastructure;

public class CosmosRepositoryFactory(IIndex<string, IRepository> repoCollection) : IRepositoryFactory
{
    public IRepository? GetRepository(string category)
    {
        return repoCollection.TryGetValue(category, out var repo) ? repo : null;
    }   
}