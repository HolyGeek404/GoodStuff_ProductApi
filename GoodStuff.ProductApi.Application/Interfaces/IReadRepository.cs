namespace GoodStuff.ProductApi.Application.Interfaces;

public interface IReadRepository<TProduct>
{
    public Task<IEnumerable<TProduct>> GetAllAsync(string category);
    public Task<TProduct?> GetById(string category, string id);
}