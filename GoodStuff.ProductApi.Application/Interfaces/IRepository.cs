namespace GoodStuff.ProductApi.Application.Interfaces;

public interface IRepository<TProduct>
{
    public Task<IEnumerable<TProduct>> GetAllAsync(string category);
    public Task<TProduct?> GetById(string category, string id);
    public Task CreateAsync(TProduct entity);
    public Task UpdateAsync(TProduct entity);
    public Task DeleteAsync(string id, string partitionKey);
}