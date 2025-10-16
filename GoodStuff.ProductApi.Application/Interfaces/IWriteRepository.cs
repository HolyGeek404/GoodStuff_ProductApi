namespace GoodStuff.ProductApi.Application.Interfaces;

public interface IWriteRepository<TProduct>
{
    public Task CreateAsync(TProduct entity);
    public Task UpdateAsync(TProduct entity);
    public Task DeleteAsync(string id, string partitionKey);
}