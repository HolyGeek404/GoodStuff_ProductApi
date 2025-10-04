namespace GoodStuff.ProductApi.Application.Interfaces;

public interface IRepository<T> where T : class 
{
    public Task<IEnumerable<T>> GetAllByTypeAsync(string category);
    public Task<T> GetSingleById(string category,  string id);
    public Task<T> CreateAsync(T entity);
    public Task<T> UpdateAsync(T entity);
    public Task DeleteAsync(T entity);
}