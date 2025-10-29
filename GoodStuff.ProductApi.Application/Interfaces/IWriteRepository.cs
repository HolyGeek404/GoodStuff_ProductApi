using System.Net;

namespace GoodStuff.ProductApi.Application.Interfaces;

public interface IWriteRepository<TProduct>
{
    public Task CreateAsync(TProduct entity);
    public Task<HttpStatusCode> UpdateAsync(TProduct entity, string id, string pk);
    public Task DeleteAsync(string id, string partitionKey);
}